# Explication du projet

GameOn! API un back-end ASP.NET Core (.NET 10) qui sert de plateforme de statistiques gaming et de gestion de tournois, actuellement déployé en production.

Les 6 projets (Onion Architecture)

GameOn.Presentation  ──►  GameOn.Application  ──►  GameOn.Domain
       │                         │                      ▲
       │                  GameOn.Persistence ───────────┘
       │                  GameOn.External
       └──────────────►  GameOn.Common
Projet	Rôle
GameOn.Presentation	Controllers HTTP, pipeline ASP.NET (Program.cs)
GameOn.Application	Toute la logique métier via CQRS (MediatR)
GameOn.Domain	Entités pures — aucune dépendance externe
GameOn.Persistence	EF Core + SQL Server (GameOnContext)
GameOn.External	Client Riot Games API + stockage S3
GameOn.Common	DTOs, interfaces, exceptions partagés entre couches
Domaine métier
Deux jeux sont supportés :

FIFA / Soccer

FifaGamePlayed — matches enregistrés
FifaTeam / FifaTeamPlayer — équipes et compositions
Tournament / TournamentPlayer — tournois
Season — saisons en cours
League of Legends

LoLGame / LoLGameParticipant — historique de parties
LoLGameTimelineFrame — timeline détaillée d'une partie
LeagueOfLegendsRankHistory — suivi du rang
LoLQueue — types de queue Riot normalisés (Id = queueId Riot, clé naturelle), synchronisés quotidiennement depuis `queues.json` ; `LoLGame.QueueId` la référence en FK nullable. L'ancien `LoLGame.QueueType` (string, dictionnaire en dur) a été entièrement supprimé (colonne + propriété + migration)
LoLGameParticipantStat — stats de performance dérivées par participant (KDA, CS/min, gold/min, dégâts/min, kill participation %, wards posés/détruits), relation 1:1 à clé partagée avec `LoLGameParticipant` (`LoLGameParticipant.Stats`). Calculées via `LoLGameParticipantStatCalculator` (partagé entre l'import live et le backfill), à partir des données déjà en base (dernière frame de timeline + kills d'équipe + events de wards) — aucun appel Riot supplémentaire. Préfère désormais `Kda`/`KillParticipation` de Riot (voir `LoLGameParticipantChallenge`) quand disponibles plutôt que de les recalculer
LoLGameParticipantChallenge — miroir 1:1 (clé partagée) de l'objet `challenges` de Riot (~120 stats déjà calculées par Riot : KDA, kill participation, dégâts/min, solo kills...), stocké tel quel sans recalcul. `ParticipantDto.RiotChallenges`/`TeamPosition`/`IndividualPosition`/`VisionScore` sont désormais capturés depuis l'API Riot (auparavant silencieusement jetés par la désérialisation Newtonsoft, `MissingMemberHandling.Ignore` par défaut)
Commun

Player — joueurs (authentifiés via JWT)
Platform — plateformes de jeu
Highlight, Changelog — contenu éditorial
Flux d'une requête (CQRS)

Controller  →  MediatR.Send(Query/Command)  →  Validator (FluentValidation)
                                            →  Handler  →  Repository Interface
                                                               └─ EF Core (Persistence)
Exemple concret : FifaGameController envoie une commande via MediatR → le handler dans GameOn.Application/FIFA/FifaGamePlayed/ l'exécute → passe par une interface de repository → EF Core écrit en base.

Les erreurs ne sont jamais catchées dans les handlers — elles remontent via des exceptions custom vers un middleware global qui les transforme en réponses HTTP standardisées.

Intégrations externes
Riot Games API — récupération des données LoL (matches, summoners, rangs)
S3 — stockage des photos de profil et logos de tournois
JWT — authentification des joueurs

# 🛠 Instructions de Développement .NET 10 - GameOn-API

Tu es un expert en **Clean Architecture**, **CQRS** et **.NET 10**. Tu dois suivre rigoureusement ces directives pour maintenir l'intégrité du projet et éviter le "vibecoding irrégulier".

## 🏗 Architecture des Couches (Onion Architecture)

Le projet est découpé en 6 projets distincts. Respecte strictement les frontières de dépendances :

1. **GameOn.Presentation** :
   - Point d'entrée (Controllers / Minimal APIs).
   - Configuration du pipeline HTTP (`Program.cs`).
2. **GameOn.Application** :
   - Logique métier via les **Features** (CQRS).
   - Dépend du Domaine. Contient les `Interfaces`, `DTOs` et les `Exceptions` applicatives.
3. **GameOn.Domain** :
   - Cœur du système : Entités, Logique métier pure, Interfaces de Repositories.
   - **AUCUNE** dépendance externe.
4. **GameOn.Persistence** :
   - Implémentation de la persistance (EF Core, `GameOnContext`).
   - Implémentation des Repositories définis dans le Domaine.
5. **GameOn.External** :
   - Clients pour services tiers (API Riot Games, ...).
6. **GameOn.Common** :
   - Tout ce qui est commun à tout les projets (DTOs, ...).

---

## ⚡ Patterns & Standards de Code

### 1. CQRS avec MediatR

- Toute action doit passer par une **Catégorie** dans `Application/Category/[NomDeLaCategory]` (exemple : Category LeagueOfLegends, FIFA). Tout ce qui est socle commun est dans `Common`
- Chaque feature doit contenir dans le même dossier :
  - `Commands` ou `Queries` (structuré avec les paramètres d'entrée).
  - `Handler` (la logique d'exécution).
  - `Validator` (FluentValidation).

### 2. Gestion des Erreurs (Exception Middleware)

- **Ne jamais faire de try/catch** dans les Handlers pour formater des erreurs API.
- Lever des **Exceptions Custom** (définies dans `Application/Exceptions`).
- Le middleware global se charge de catcher ces exceptions et de les transformer en réponses standardisées.

### 3. Validation

- Utiliser **FluentValidation**.
- Chaque `Command` ou `Query` doit avoir un validateur associé injecté automatiquement dans le pipeline MediatR.

### 4. Injection de Dépendances

- Chaque couche possède un fichier `DependencyInjection.cs`.
- Toute nouvelle classe (Service, Repository, Handler) doit y être enregistrée.

---

## 🚫 Interdictions Strictes

> [!IMPORTANT]
>
> - **Accès Data :** Ne jamais injecter `GameOnContext` dans la couche Presentation ou dans les Handlers. Toujours passer par les interfaces de Repositories.
> - **Couplage :** La couche `Domain` ne doit jamais référencer `Persistence` ou `Presentation`.
> - **Style :** Respecter strictement `stylecop.json`. Ne pas supprimer les règles de style pour "gagner du temps".
> - **Langue des commentaires :** **tout commentaire de code est en anglais, sans exception.** Ça couvre les `//`, les `///` de documentation XML, le libellé accolé à un `#pragma warning`, et les commentaires des scripts Python de `scripts/`. Piège principal : le quick-fix « Supprimer l'avertissement » de Visual Studio en locale française recopie le message Roslyn traduit (`// Déréférencement d'une éventuelle référence null.`) — le remplacer par le libellé anglais officiel (`// Dereference of a possibly null reference.`). En revanche, les **chaînes de caractères** destinées aux joueurs restent en français (prompt de Raimmus, brief du coach, descriptions de tournoi), et cette documentation Markdown aussi.

---

## ⚠️ État local en cours

✅ Rattrapage de masse des `LoLGame.QueueId` fait en prod par l'utilisateur (2026-07-16, via `UPDATE` manuel en DBeaver).

✅ `LoLGame.QueueType` supprimé (front à faire basculer sur `QueueId`/`LoLQueue` via les routes `/lol/Queue` et `/lol/Queue/player/{playerId}` si pas déjà fait).

✅ Filtre par plage de date ajouté sur l'historique de parties LoL (2026-07-23) : `GetLastGamesPlayedQuery.StartDate`/`EndDate` (bornes inclusives sur `LoLGame.GameStart`), exposés en query params `startDate`/`endDate` sur `GET lol/Match/last` et `GET lol/Match/player/{playerId}`.

✅ Entité `LoLGameParticipantStat` ajoutée (2026-07-23) : stats dérivées par participant (KDA, kill participation %, CS/gold/dégâts par minute, wards). Calculées automatiquement à chaque import/update de partie (`UpdateLoLGameCommandHandler`), et backfillées pour tout l'historique via `POST Admin/lol/recompute-participant-stats` (rôle `gameon_admin`) — migration générée et appliquée par l'utilisateur, backfill exécuté en prod. Exposées nested sur `LoLGameParticipant.Stats` dans `GET lol/Match/{matchId}` et `GET lol/Match/player/{playerId}`.

✅ Phase 1 « maximiser les données Riot » (2026-07-24) : `ParticipantDto` capture désormais `teamPosition`/`individualPosition`/`visionScore`/`challenges` (auparavant jetés silencieusement — `HttpServiceBase.RunRequest` désérialise via Newtonsoft sans `JsonSerializerSettings`, donc tout champ Riot sans propriété C# est ignoré par défaut, pas d'erreur). `ChallengesDto` (stub orphelin trouvé dans le repo, jamais branché) a été complété (~120 champs) et branché via `ParticipantDto.RiotChallenges`. Nouvelle entité `LoLGameParticipantChallenge` (1:1 clé partagée avec `LoLGameParticipant`, même pattern que `Stats`) miroir de `ChallengesDto`, peuplée dans `UpdateLoLGameCommandHandler` — quelques types de champs (int→float) corrigés par l'utilisateur après coup pour matcher les vraies valeurs Riot. `LoLGameParticipantStatCalculator` utilise `Challenges.Kda`/`Challenges.KillParticipation` de Riot en priorité (fallback sur le calcul manuel si absent). Migration générée et appliquée, exposé côté front. **Hors scope de cette phase** (à faire plus tard, Phase 2) : perks/runes (page de runes, table normalisée déjà actée avec l'utilisateur), sorts d'invocateur, multikills, objectifs d'équipe et bans (`TeamDto.Objectives`/`Bans`, déjà récupérés de Riot mais toujours jetés), `InfoDto.GameMode`/`GameDuration`/`MapId`.

✅ Dates 100 % UTC (2026-08-06) — l'ancien bug `.ToLocalTime()` est corrigé. `GameOnContext.ConfigureUtcDates()` applique un `ValueConverter` à **toutes** les propriétés `DateTime`/`DateTime?` du modèle : lecture → `SpecifyKind(Utc)`, écriture → `ToUniversalTime()` si `Local`. Les colonnes `datetime2` de SQL Server n'ayant pas d'offset, EF matérialisait tout en `Unspecified` et System.Text.Json sérialisait sans le `Z` final — l'API renvoie désormais du vrai ISO 8601 UTC. En complément : `.ToLocalTime()` retiré de `UpdateLoLGameCommandHandler` (`DateTime.UnixEpoch.AddMilliseconds(...)` direct), et **tous** les `DateTime.Now` du code applicatif passés en `DateTime.UtcNow`. Aucun backfill nécessaire : le conteneur de prod n'a pas de `TZ` configurée et tourne en UTC, donc les valeurs déjà en base étaient déjà de l'UTC — le converter ne fait que le déclarer. Aucune migration nécessaire non plus (le type de colonne ne change pas). Côté front, `parseApiDate()` reste en place comme filet de sécurité (il laisse passer les chaînes déjà suffixées `Z`).

✅ Audit complet des « stats marrantes » LoL (2026-08-06, `GetLoLGlobalStatsQueryHandler`) :

- **Biggest Inter** : le tri portait sur un score composite (`Deaths - Kills - Assists/2`) alors que la valeur affichée était `Deaths`, d'où des records non monotones entre périodes (16 morts sur 7 j, 14 sur 1 mois). Le tri porte désormais sur `Deaths`, le score composite ne sert plus qu'à départager.
- **Highest Bounty** : `ParticipantDto.BountyLevel` n'est plus renvoyé par match-v5 depuis 2025 (cf. RiotGames/developer-relations#1076) et vaut 0 partout. L'award est reconstruit depuis `LoLGameTimelineEvent` (`MAX(ShutdownBounty)` sur les `CHAMPION_KILL` où le joueur trackés est la victime). La colonne `BountyLevel` reste en base mais **ne doit plus servir de critère de classement**.
- **Ping Machine** : ne sommait que 3 des 13 types de pings de Riot. Les 10 manquants ont été ajoutés à `ParticipantDto` (donc en colonnes) + migration `Added_Missing_Pings_In_LoLGames`. Les games importées avant restent à 0 dessus, faute de ré-import.
- Garde-fou « zéro donnée » sur les 4 awards issus des participations : sans lui, `.First()` sur un dataset entièrement à 0 sacrait un joueur au hasard (c'est ce qui masquait le champ Riot mort).
- Les awards ne supposent plus du 5v5 : le camp vient du vrai `LoLGameParticipant.TeamId` (au lieu de `ParticipantId <= 5`) et le nombre d'ennemis est compté sur le roster (au lieu d'un `/5` en dur).
- **Night Owl** compte désormais les games de minuit à 6 h sur l'horloge des joueurs (`Europe/Paris`) et non sur l'horloge UTC du serveur.
- Les games dont la queue n'est pas résolvable (`LoLGame.Queue == null`) sont exclues : sans ligne `LoLQueue`, elles échappaient au filtre par mots-clés bot/custom/tutorial.
- Départages déterministes ajoutés partout (les ex æquo pouvaient changer de gagnant d'un refresh à l'autre, faute d'`ORDER BY` stable en base).

✅ Bloc « Fait de la semaine » ajouté sur `GET lol/Home` (2026-08-12, `GetLoLHomeStatsQueryHandler`) : `LoLHomeStatsDto.FactOfTheWeek` (nullable) met en avant le joueur avec le meilleur gain net de LP de la semaine calendaire en cours, toutes queues classées confondues (même somme par joueur que `WeeklyActivity.NetLpChangeThisWeek`, mais par joueur au lieu du crew). Toujours le top gainer de la semaine, record historique ou pas (décision utilisateur : pas de comparaison à une date de référence type « depuis février », trop de cas ambigus). Contient aussi `GamesThisWeek`/`WinsThisWeek`/`WinRateThisWeek` et `LongestWinStreakThisWeek` (plus longue série de victoires consécutives de ce joueur sur la semaine, même algo que `LongestLossStreak` dans `GetLoLGlobalStatsQueryHandler` mais sur les victoires). Null si aucun joueur n'a de snapshot de rang comparable (semaine dernière + cette semaine) sur une queue classée. Aucune migration nécessaire (pas de nouvelle donnée persistée).

✅ Bloc « Records du crew » ajouté sur `GET lol/Home` (2026-08-12) : `LoLHomeStatsDto.CrewRecords` (type `LoLGlobalStatsDto`, jamais null) réutilise directement `GetLoLGlobalStatsQueryHandler` via `mediator.Send(new GetLoLGlobalStatsQuery { Period = LoLStatsPeriod.Week })` plutôt que de dupliquer la logique des awards — `GetLoLHomeStatsQueryHandler` prend donc désormais `ISender` en plus de `IApplicationDbContext`. `LoLStatsPeriod.Week` = fenêtre glissante de 7 jours (`DateTime.UtcNow.AddDays(-7)`), volontairement différente du calendrier lundi→dimanche utilisé par `WeeklyActivity`/`FactOfTheWeek` dans le même DTO (demande explicite de l'utilisateur : « ces stats doivent être celles des 7 derniers jours »). Toutes queues confondues (`RankedOnly`/`Queue` par défaut). Chaque award individuel (`BiggestInter`, `NightOwl`, etc.) reste nullable côté `LoLGlobalStatsDto` si personne n'a de record sur la fenêtre.

✅ « Champions du crew » ajouté (2026-08-12, `GetLoLGlobalStatsQueryHandler`) : `LoLGlobalStatsDto.TopChampions` (liste, jamais null, vide si aucune game) — les 5 champions les plus joués sur la période/queue demandée (games groupées par `ChampionName` sur le dataset `participants` déjà filtré remakes/bot/custom/tutorial), triés par nombre de parties puis win rate puis nom pour un ordre stable. Calculé directement dans `GetLoLGlobalStatsQueryHandler` (pas de nouveau handler) donc disponible à la fois sur `GET lol/Stats/global` (toute période/queue) et automatiquement sur `GET lol/Home` via `CrewRecords` (qui appelle ce même handler en `Period = Week`) — la carte « Champions du crew » du front doit donc lire `homeStats.crewRecords.topChampions`, malgré le nom `CrewRecords` pensé à l'origine pour les awards. Aucune donnée d'icône renvoyée : le front reconstruit l'icône depuis `ChampionName` (Data Dragon), comme ailleurs dans le code.

🟡 Import des parties personnalisées (customs) depuis le client LoL (2026-09-17) — **codé, pas encore exécuté en prod**. Les customs sont introuvables via l'API Riot : absentes de `matches/by-puuid/{puuid}/ids`, et `matches/{matchId}` répond soit 404, soit un stub `endOfGameResult = "Abort_Unexpected"` avec `queueId = 0`, `gameCreation = 0` et zéro participant (c'est l'origine des 10 lignes fantômes en base, `GameStart = 1970-01-01`, 0 participant — à supprimer). Seule source restante : l'historique du client League lui-même (LCU), dumpé par `scripts/fetch_custom_games_lcu.py` (lit le lockfile, filtre les `CUSTOM_GAME`, écrit `<platformId>_<gameId>.json` + `.timeline.json`, et pousse vers l'API avec `--push` / `--from-dir`). Côté API : `POST lol/Match/custom/import` (rôle `gameon_admin`) → `ImportCustomLoLGameCommand` → `ImportCustomLoLGameCommandHandler`, qui mappe le payload LCU (`GameOn.Common/DTOs/LeagueOfLegends/LeagueClient/`) vers les entités et renvoie un `ImportCustomLoLGameResultDto`. Points à connaître :

- Le payload LCU est l'**ancien format match-v4** : pas de `challenges` (donc `LoLGameParticipant.Challenges` reste null et le KDA / la kill participation repassent par les formules manuelles), pas de `damageStats`/`championStats` sur les frames de timeline, et seulement 3 types d'events (`CHAMPION_KILL`, `BUILDING_KILL`, `ELITE_MONSTER_KILL` — donc aucun event de ward, d'item ou de level up). Les stats dérivées (`LoLGameParticipantStat`) sont calculées depuis les totaux de fin de partie du participant (que le client, lui, envoie) et non depuis la dernière frame, via le même `LoLGameParticipantStatCalculator`.
- **La dernière frame de timeline est remplie avec ces totaux de fin de partie** (dégâts infligés/subis + leurs splits physique/magique/brut, `TimeEnemySpentControlled`), parce que c'est là que tout le reste du système va chercher le snapshot de fin de partie — le front lit `latestStatsFor(timeline, puuid)` pour le graphe « Dégâts aux champions » et l'award « Punching-Ball ». Les frames intermédiaires restent à 0 : le client ne connaît pas la courbe de dégâts. `ChampExperience`, absent des stats de fin de partie, est repris du `xp` de cette même dernière frame. En revanche les **stats de champion** (AD, AP, armure, vitesse de déplacement…) restent à 0 sur toutes les frames, y compris la dernière : elles n'ont aucun équivalent de fin de partie, le bloc « Stats du champion » du front est donc vide sur une custom.
- **Définitivement absents du payload client**, aucun moyen de les reconstituer : les compteurs de pings, `consumablesPurchased`, et les primes (`ShutdownBounty` sur les `CHAMPION_KILL`). Les awards du front qui reposent dessus (Ping Machine, Shopping Addict, Tête mise à prix) sacrent donc un joueur au hasard avec 0 — c'est exactement le bug « zéro donnée » corrigé côté serveur en 2026-08-06 sur `GetLoLGlobalStatsQueryHandler`, mais côté front cette fois (`LolGameHighlights.vue` de JungleDiff, pas de garde sur `value <= 0`).
- Le client **anonymise les PUUID** (UUID de 36 caractères, pas le vrai PUUID de 78). Les joueurs sont donc reliés sur leur Riot ID (`RiotGamesNickname` + `RiotGamesTagLine`, insensible à la casse) et c'est le vrai PUUID de GameOn qui est stocké quand ça matche ; sinon l'UUID anonymisé sert de clé (l'alternate key `(MatchId, Puuid)` doit rester unique). Les Riot IDs non reconnus remontent dans `UnlinkedRiotIds` — sur le dump de septembre 2026, `RememberV#8888` (probablement `Ryusen#8888` renommé, player 34) et `Zélouf#1360` (probablement `Zéloufis#EUW`, player 52) ne sont pas reliés.
- Le champion n'est donné que par son ID : résolu via `ICommunityDragonChampionService` (`champion-summary.json`, alias type `MonkeyKing` = ce que renvoie match-v5), caché 24 h en mémoire.
- Le rôle du client (`timeline.lane`/`role`) est trop faux pour être recopié (deux junglers par équipe en custom) : `TeamPosition`/`IndividualPosition` sont déduits (Smite → JUNGLE, plus petit CS → UTILITY, le reste par lane).
- `LoLQueue` connaît déjà les queues de customs (3100 aveugle, 3110 draft, 3140 outil d'entraînement), donc `LoLGame.QueueId` est bien renseigné. Ces descriptions venant de Community Dragon sont **en français**, et échappaient donc au filtre anglais de `GetLoLGlobalStatsQueryHandler` : `ExcludedQueueTypeKeywords` s'est vu ajouter `"personnalis"` et `"entraînement"` pour que les customs restent hors des records du crew (vérifié : ça n'exclut que les 9 queues de customs SR/ARAM/TFT, aucune file normale ou classée). Les customs restent en revanche visibles dans l'historique (`GET lol/Match/last`, `GET lol/Match/player/{playerId}`).
- Aucune migration nécessaire (aucun changement de schéma).

🟡 Coach IA sur les parties LoL (2026-09-17) — **codé et compilé, migration générée, pas encore appliquée ni exécutée**. Décision : pas d'auto-hébergement pour l'instant (le mini-PC Proxmox est un Ryzen 7 H 255 / Radeon 780M sans NPU, donc pas de VRAM dédiée : un 30B MoE y tournerait à ~15-22 tok/s, et surtout le prefill d'un contexte de 3 k tokens y coûte 30-60 s en CPU pur). On passe par la **clé gratuite Google AI Studio** (free tier permanent, ~1 500 req/jour sur Gemini 2.5 Flash, aucun compte de facturation rattaché au projet `gen-lang-client-0926924054` — ne jamais cliquer « Configurer la facturation », la promotion en Tier 1 payant est immédiate et sans confirmation). L'abonnement Claude Pro et l'abonnement Google AI Pro n'ouvrent **aucun** accès API, c'est un produit séparé.

- **Couche External** : `GameOn.External/Llm/` avec `ILlmService` (seam volontairement agnostique du provider : `ModelName`, `IsConfigured`, `GenerateJsonAsync(system, user, jsonSchema)`) et **trois implémentations**, choisies au démarrage par `LLM_PROVIDER` dans le `switch` de `GameOn.External/DependencyInjection.cs` (défaut `gemini`) :
  - `GeminiLlmService` — `GEMINI_API_KEY`, `GEMINI_MODEL` (défaut `gemini-3.6-flash`). **Free tier inutilisable pour ce projet, voir plus bas.**
  - `GroqLlmService` (`LLM_PROVIDER=groq`) — `GROQ_API_KEY`, `GROQ_MODEL` (défaut `openai/gpt-oss-120b`). Dialecte OpenAI (`POST /openai/v1/chat/completions`), sortie contrainte par `response_format: json_schema` en mode `strict`. **C'est le provider retenu** (2026-09-17).
  - `OllamaLlmService` (`LLM_PROVIDER=ollama`) — `OLLAMA_BASE_URL`, `OLLAMA_MODEL`, `LLM_NUM_CTX`/`LLM_NUM_PREDICT`/`LLM_NUM_THREADS`. Repli auto-hébergé, sans quota et sans donnée qui sort de la machine.
  - `LLM_TIMEOUT_SECONDS` vaut 180 par défaut, 900 en mode `ollama` (un modèle sur CPU met des minutes, et son premier appel paie en plus le chargement des poids).
- ⚠️ **Le schéma de réponse est du JSON Schema nu dans `LoLCoachPrompt.ResponseSchema`, et chaque provider l'adapte chez lui.** Gemini et Ollama le prennent tel quel ; le mode strict de Groq exige en plus `additionalProperties: false` sur chaque objet, ajouté à la volée par `GroqLlmService.Harden()`. Ne jamais remonter un dialecte de provider dans le contrat partagé.
- ⚠️ **L'API Gemini a changé de surface, vérifié en direct le 2026-09-17** (les exemples qui traînent partout sont périmés) :
  - `gemini-2.5-flash` répond **404 « no longer available to new users »** sur une clé créée aujourd'hui. Les modèles Gemini **3.x ne sont pas servis du tout** par `POST /v1beta/models/{modèle}:generateContent` : cet endpoint renvoie un 404 **à corps vide** pour eux, ce qui est très trompeur au diagnostic.
  - La bonne surface est la **Interactions API** : `POST https://generativelanguage.googleapis.com/v1beta/interactions`, corps `{ model, system_instruction, input, response_format: { type: "text", mime_type: "application/json", schema }, generation_config: { temperature } }`.
  - Le schéma est du **JSON Schema standard en minuscules** (`"type": "object"`), et non le sous-ensemble OpenAPI en majuscules (`"OBJECT"`) qu'attendait `generateContent`. Pas de `propertyOrdering`.
  - La réponse n'a plus de `candidates` : c'est `steps[]`, dont il faut concaténer le `text` des entrées `type == "model_output"` (les entrées `type == "thought"` portent le raisonnement, à ignorer). Plus de `maxOutputTokens` à régler, donc l'ancien piège « le raisonnement consomme tout le budget et renvoie un candidat vide » disparaît.
  - Modèle retenu : **`gemini-3.6-flash`** (~15 s pour un rapport). `gemini-3.8-flash` existe mais renvoyait des 503 « high demand » en rafale. Le raisonnement coûte cher en tokens (~1 900 tokens de pensée pour ~480 de sortie) — sans incidence sur le free tier, mais à surveiller si passage au payant.
  - `LlmTransientException` (ex-`LlmRateLimitedException`) couvre **429 ET 5xx** : sur ce tier, un modèle saturé est aussi banal qu'un quota épuisé, et ni l'un ni l'autre ne doit consommer une tentative.
- **N'utilise volontairement pas `HttpServiceBase`** : à l'époque, son `RunRequest` levait `NotImplementedException` sur tout statut non-200/204, ce qui écrasait un 429 de quota et un 400 de requête invalide dans la même erreur. Or les distinguer est tout l'enjeu ici. (Depuis le 2026-09-17, `RunRequest` lève une `ExternalApiException` qui porte le statut, la route et le corps — mais `GeminiLlmService` garde sa propre logique, qui distingue en plus le transitoire du définitif.) Idem pour le `HttpClient` : `services.AddScoped<HttpClient>()` porte le timeout par défaut de 100 s, trop court pour une génération — d'où un client nommé dédié (`GeminiLlmService.HttpClientName`).
- **Génération à la demande, jamais automatique** (décision utilisateur du 2026-09-17, toujours valable) : aucun rapport n'est écrit sans que quelqu'un l'ait demandé. Pas de génération à la synchro d'une partie, pas de rattrapage du backlog, pas de scan de la base — un ticket existe parce qu'un joueur a cliqué.
- **Mise en file au clic** (2026-09-17, révision du flux synchrone) : le `POST` n'appelle plus le modèle, il enfile et renvoie **202** avec la place dans la file ; le front poll le `GET`. Ce qui a rouvert la décision : le synchrone reposait sur un « ~15 s » qui s'est révélé faux — une génération prend **49 s mesurées en prod**, contre un free tier à **5 req/min**. Sérialisées, ça plafonne à ~1,2 analyse/min : tenir la connexion ouverte ne marchait que pour un joueur à la fois, et un deuxième clic simultané ne pouvait que échouer (le proxy Nuxt coupe à 60 s). `ILoLCoachQueue` / `LoLCoachQueue` (singleton, `LinkedList` + lock — pas un `Channel`, qui ne sait ni donner une position ni remettre un ticket en tête) ; `ProcessLoLCoachQueueJob` est le **consommateur unique**, donc c'est lui le vrai garde-fou de débit. Un refus `LlmTransientException` remet le ticket en tête et attend 30 s, 5 tentatives maximum avant abandon (sans plafond, un ticket définitivement refusé affamerait toute la file). **La file vit en mémoire** : perdue au redéploiement, ce qui est le prix assumé pour que la table reste un cache pur — pas de colonne de statut, pas de migration. Valable tant qu'il n'y a **qu'une seule instance** en prod (confirmé par l'utilisateur le 2026-09-17) ; passer à plusieurs conteneurs casse le polling et imposerait l'état en base.
- **Entité `LoLGameCoachReport`** (table `LeagueOfLegendsGameCoachReport`) : une ligne par (match, joueur), index unique `(MatchId, Puuid)`. Stocke `Summary`, `Rating`, `ContentJson`, `ModelName`, `PromptVersion`, `GeneratedOn`. Pas de statut : le flux étant synchrone, une ligne qui existe contient forcément une analyse terminée — **la table est un cache**, pas un journal, et c'est elle qui garantit qu'une même analyse n'est jamais payée deux fois. `PromptVersion` permet de retrouver et régénérer les rapports écrits par un prompt périmé (`LoLCoachPrompt.Version`, à bumper). `Rating` est éditorial et **non reproductible** : ne jamais l'agréger ni classer dessus, contrairement à `LoLGameParticipantStat.Rating`.
- **Le vrai point dur, `LoLCoachContextBuilder`** : une timeline match-v5 brute fait 500 Ko-2 Mo (150 k-600 k tokens), donc le builder sélectionne ~2-3 k tokens — stats du joueur **en différentiel face à son opposant direct** (identifié via `TeamPosition`), courbe or/CS/XP à 10/15/20 min depuis `LoLGameTimelineFrame`, morts horodatées avec zone de la carte, objectifs d'équipe depuis `LoLGameTeam`, et une sélection de `challenges` Riot. **Règle centrale : une valeur à zéro n'est jamais envoyée au modèle** (wards, challenges…), parce qu'une donnée non capturée présentée comme un fait est exactement ce qui fait inventer une faiblesse — c'est le bug « zéro donnée » de 2026-08-06, transposé au prompt. La zone de mort n'est calculée que si `LoLQueue.Map` contient « Summoner » (sinon on nommerait une lane sur une ARAM).
- ⚠️ **Garde-fou de débit — le `SemaphoreSlim(1)` n'en était pas un** (corrigé le 2026-09-17). Il limite la **concurrence**, pas le **débit**, et ne ressemble à une limite de débit que tant que chaque appel est lent. Or un refus revient en **285 ms** là où une génération tient 49 s : dès que Gemini commence à dire non, le verrou tourne à ~210 fois/min au lieu de 4, un premier 429 devient une rafale, la rafale cloue le quota, et on n'en sort plus. Observé en prod : trois 429 terminés en moins d'une seconde d'écart. Le correctif est `MinimumInterval` — un plancher de **12 s entre deux départs d'appel** (= 5 req/min en espacement), réglable par `LLM_MIN_INTERVAL_SECONDS`, **appliqué quel que soit le résultat** pour qu'un refus coûte un créneau entier comme une génération. Le sémaphore reste, mais depuis la mise en file il ne contend plus : `ProcessLoLCoachQueueJob` est le seul appelant. Le chemin « rapport déjà en cache » ne passe ni par la file ni par le sémaphore. ⚠️ **Le raisonnement est juste, la cible était fausse** : ce plancher espace pour tenir 5 req/min alors que la contrainte réelle de Gemini était journalière (voir ci-dessous), donc il n'a jamais rien empêché. `GroqLlmService` reprend le même mécanisme avec un défaut de **35 s**, calibré cette fois sur ce qui borne vraiment là-bas — le TPM (6 000), pas le nombre de requêtes. `OllamaLlmService` n'en a aucun : pas de quota à respecter.
- ✅ **Le 429 mystérieux est résolu (2026-09-17) : c'est le RPD.** Le tableau de bord AI Studio affiche, pour `gemini-3.6-flash` en free tier, `RPM 3/5`, `TPM 3.43K/250K` et **`RPD 10/20`**. Le quota qui saute est **journalier, à 20 requêtes pour tout le crew** — d'où un 429 après seulement 2 requêtes en 60 s. Ni le RPM ni le TPM n'ont jamais été en cause : le raisonnement du modèle consomme 1,4 % du budget de tokens, donc l'hypothèse « le thinking fait sauter le TPM » était fausse, et réduire le budget de raisonnement n'aurait rien changé. Conséquence : le free tier Gemini n'est pas « trop juste », il est **structurellement inadapté** — 20 rapports/jour ne couvrent ni l'usage de 17 joueurs, ni une régénération d'historique en `force=true`. D'où la bascule sur Groq, dont le free tier donne **14 400 req/jour** sans carte bancaire.
- ⚠️ **Le « middleware global » décrit deux fois dans ce fichier n'existe pas.** `Program.cs` n'a ni `UseExceptionHandler`, ni `IExceptionHandler`, ni filtre d'exception — seulement Swagger, CORS, Auth et `MapControllers`. Conséquence : `CoachController.Generate` contient **le seul try/catch de la couche Presentation**, qui mappe `LlmTransientException` sur un **HTTP 429 + `Retry-After: 30`**. Sans lui, un refus de quota arriverait au client en 500 nu, signalé comme un défaut alors que c'est « reviens dans une minute ». À supprimer le jour où ce middleware sera réellement écrit.
- **Routes** : `GET lol/Coach/{matchId}/player/{playerId}` — libre, **ne déclenche jamais rien**. `200` + rapport, `202` + `LoLCoachQueueStatusDto` (`position`, `queueLength`, `estimatedWaitSeconds`, `enqueuedOn`) quand l'analyse est en file, `404` tant que personne ne l'a demandée (le front doit traiter ce 404 comme « propose le bouton », pas comme une erreur). C'est la route à poller. `POST lol/Coach/{matchId}/player/{playerId}` — `[Authorize]`, n'importe quel joueur connecté sur n'importe quelle partie. Renvoie **200** si le rapport est déjà en cache, **202** + statut de file sinon, **404** si le joueur n'a pas joué ce match. **Ne bloque plus** et ne renvoie plus de 429. Cliquer deux fois n'achète pas deux créneaux (dédup sur `(matchId, playerId)`). Le paramètre `force=true` n'est honoré que si l'appelant a le rôle `gameon_admin` : n'importe qui peut le passer, personne d'autre ne l'obtient.
- ⚠️ **Le seul try/catch de la couche Presentation a disparu avec la mise en file** : plus rien n'appelle le modèle depuis une requête HTTP, donc `CoachController` n'a plus à traduire `LlmTransientException` en 429. Les exceptions du modèle sont désormais attrapées et **logguées** par `ProcessLoLCoachQueueJob`. La remarque sur l'absence de middleware global d'exceptions reste valable pour le reste de l'API.
- **Le coach s'appelle « Raimmus » (Rammus + AI), démonte les défaites et sacre les victoires** — `LoLCoachPrompt.SystemPrompt`, **version 4** (2026-09-18). ⚠️ **Le ton est calibré sur le RÉSULTAT de la partie, plus sur la performance individuelle** (demande du crew : la v2 chambrait trop mollement) : démolition sans filtre en défaite, éloge sans réserve en victoire. ⚠️ **La v4 corrige le registre, pas la virulence** : la v3 fournissait au modèle une liste d'expressions d'argot (« wesh », « mon reuf », « frérot », « de ouf ») et une liste de memes, et il s'appuyait dessus au point que chaque rapport tournait à la caricature — or **l'attendu, c'est le roast**, une remarque marrante n'étant qu'un bonus quand la partie en offre une. Le lexique est remplacé par une consigne d'ironie sèche et de constats francs et chiffrés (un chiffre précis tape plus fort qu'une punchline), et l'argot comme les memes sont désormais explicitement interdits. Deux nuances écrites en dur, parce qu'un ton purement indexé sur le résultat sacrerait un feeder et enterrerait le seul joueur qui a tenu la partie : porter une défaite → on tape sur le déroulé de la partie, se faire porter dans une victoire → on sauce quand même en rappelant le carry. Garde-fous conservés et renforcés : rien d'inventé (**on exagère le ton, jamais un chiffre**), une valeur à zéro reste ignorée, aucune attaque personnelle, et **aucune vanne sur un coéquipier nommé** — ce sont des potes qui ont chacun droit à leur propre rapport. `axesProgression` reste sérieux dans les deux cas, et **`noteSur10` suit la performance individuelle, pas le résultat ni le ton** (un 2/11 gagné reste une mauvaise note) : c'est le seul champ que le ton ne doit pas bouger.
  - Les garde-fous passent **avant** l'humour, et c'est essentiel : un modèle à qui on demande d'être drôle enjolive. Interdiction explicite d'inventer ou d'exagérer un chiffre pour faire marcher une vanne, de chambrer sur une donnée absente (le piège « zéro donnée » à nouveau), et de viser la personne plutôt que le jeu.
  - Le chambrage vit **uniquement dans `synthese`**. Les `axesProgression` restent sérieux et applicables : c'est ce pour quoi le joueur est venu. Vérifié : sur la partie roastée, les trois axes (vision, placement, temporisation) sont propres.
  - `pointsForts` peut sortir **vide**, et c'est voulu — sur la partie à 1/9/8 le modèle n'a rien inventé plutôt que de servir un compliment de politesse. Le front doit gérer ce cas sans paraître cassé.
  - Une allusion au tatou ou un « OK. », une fois par rapport maximum, sinon c'est lourd.
  - ⚠️ Les rapports déjà en base ont `PromptVersion = 1`, `2` ou `3` et restent servis tels quels (ton neutre en v1, chambrage indexé sur la performance en v2, argot en v3). Pour les repasser en v4 : `POST lol/Coach/{matchId}/player/{playerId}?force=true` avec le rôle `gameon_admin`.
  - ⚠️ Léger travers observé : le modèle sur-attribue parfois une stat globale à l'adversaire de lane (« 3 kills en solo contre Viktor » alors que le brief dit seulement « Kills en solo : 3 »). Sans gravité, mais à surveiller si tu ajoutes des stats agrégées au brief.
- **Reste à faire** : brancher le front sur le flux en file (bouton « Analyser cette partie », puis polling du `GET` avec affichage de la position et de l'estimation tant qu'il répond 202). La migration et les variables d'environnement sont posées ; `LLM_MIN_INTERVAL_SECONDS` est optionnelle (défaut 12 s sur Gemini, 35 s sur Groq, sans objet sur Ollama).
- ⚠️ Sur les **customs importées via LCU**, le contexte est nettement plus pauvre (pas de `challenges`, pas de courbe de dégâts, pas d'events de ward, `LoLQueue.Map` vide donc pas de zone de mort). Le builder dégrade proprement, mais la qualité du coaching s'en ressent.
- ⚠️ Les données envoyées à Gemini contiennent les Riot IDs du crew, et le free tier **est utilisé par Google pour entraîner ses modèles**. Si ça pose problème, anonymiser dans `LoLCoachContextBuilder` (« Joueur 1 », « Toplaner adverse »…) ne dégraderait pas le coaching.

✅ `HttpServiceBase.RunRequest` ne lève plus `NotImplementedException` sur les statuts non gérés (2026-09-17) : nouvelle `ExternalApiException` (`GameOn.External/Common/Exceptions/`) qui porte le `StatusCode`, la route appelée et le corps de réponse brut. L'ancien comportement transformait n'importe quelle réponse Riot (400, 403, 404, 429) en un 500 nu sans le moindre indice — c'est ce qui a rendu illisible le bug des PUUID périmés ci-dessous. La route est **expurgée de la clé d'API** avant d'entrer dans le message d'exception (`ExternalApiException.Redact`), Riot prenant sa clé en query param : sans ça, un identifiant vivant se retrouverait dans chaque log et chaque réponse d'erreur. Aucun appelant n'attrapait `NotImplementedException`, le changement est donc sans effet de bord.

⚠️ **Les PUUID Riot sont liés à la clé d'API qui les a obtenus** (constaté le 2026-09-17 en local) : la base `gameondev` contient des PUUID chiffrés avec une autre clé que celle de `launchSettings.json`, donc **tous** les endpoints `by-puuid` (summoner-v4, account-v1, league-v4) répondent `400 Bad Request - Exception decrypting <puuid>` — les 17 joueurs sans exception. Vérifié : le même Riot ID passé à `account-v1/by-riot-id` avec la clé courante renvoie un PUUID **différent**, qui lui fonctionne. Conséquence : une clé de dev régénérée (elles expirent toutes les 24 h) invalide tout l'historique de PUUID en base locale. En prod la clé ne tourne pas, le problème n'y existe pas.

⚠️ `UpdatePlayerSummonerAdminCommandHandler` ne peut **pas** réparer un PUUID périmé : il court-circuite (`return playerInDb`) dès qu'un joueur porte déjà ce couple nickname/tagline — ce qui est toujours vrai quand on rafraîchit un joueur existant. Il ne résout le PUUID que pour un Riot ID encore inconnu de la base. Il cherche aussi le joueur par `KeycloakId` seul, donc il ne peut rien faire sur un smurf (même piège que celui déjà corrigé dans `UpdatePlayerSummonerCommandHandler`).

⚠️ Incohérence repérée : ce fichier documente une route `POST Admin/lol/recompute-participant-stats` qui **n'existe nulle part dans le code** (`AdminController` n'expose que `dashboard`). Soit elle a été retirée, soit elle vit sur une autre branche.

✅ Build sans warning StyleCop (2026-09-17) — on est passé de 48 warnings à 1. Ce qui a été fait, et surtout pourquoi :

- **42 des 48 venaient des migrations EF.** Toutes les migrations jusqu'au 2026-02-18 portent `// <auto-generated />` en première ligne, ce qui fait que StyleCop saute entièrement le fichier ; les 14 écrites depuis l'avaient perdu (`dotnet ef migrations add` n'émet ce marqueur que dans le `.Designer.cs`, pas dans le `.cs` de migration). Le marqueur a été remis sur les 14. **À refaire à la main après chaque `migrations add`**, sinon les SA1633/SA1200/SA1413/SA1122 reviennent — ce n'est pas une suppression de règle, c'est déclarer généré du code qui l'est, et c'est déjà la convention du repo.
- `GetLeaguePlayerByIdQueryHandler` : constantes remontées avant `ExcludedQueueTypeKeywords` (SA1203) et `NormalizeTeamPosition` remontée avant les méthodes d'instance (SA1204). Aucun changement de comportement.
- `MinIOService.UploadFile` : le `try { ... } catch (Exception ex) { throw; }` était intégralement neutre (aucun log, `ex` jamais lu — d'où le CS0168). Supprimé ; l'exception remonte exactement comme avant.
- ⚠️ **SA1009 sur les 3 `BackgroundService` à constructeur primaire est un faux positif**, neutralisé par un `#pragma` local et justifié sur place. StyleCop 1.1.118 date de 2018 et ne reconnaît pas la forme `) : Base` d'un constructeur primaire C# 12. **Vérifié en direct** : retirer l'espace pour satisfaire SA1009 déclenche aussitôt SA1024 (« colon should be preceded by a space ») à la colonne suivante — les deux règles se contredisent, aucun formatage source ne les satisfait toutes les deux. Ne pas « corriger » ces pragmas.
- ⚠️ **Le dernier warning restant (SA1516, sans fichier ni ligne) est irréductible et c'est normal** — décision utilisateur : on le laisse. Il vient des **top-level statements de `Program.cs`**, que StyleCop 1.1.118 ne connaît pas non plus (C# 9). Vérifié en remplaçant `Program.cs` par une classe `Program`/`Main` classique : 0 warning, 0 erreur. Il est signalé avec `Location.None` — d'où le préfixe `CSC :` sans chemin — donc **ni `#pragma` ni `[SuppressMessage]` ciblé ne peuvent l'atteindre** : inutile de réessayer. Les seules sorties seraient de restructurer le point d'entrée en `Main`, ou un `<NoWarn>SA1516</NoWarn>` qui masquerait aussi les vrais SA1516 des Controllers. Les deux ont été écartées.
- Hypothèses testées et **écartées** (ne pas les re-explorer) : les global usings générés ne sont pas en cause (`GameOn.Application` a un `GlobalUsings.g.cs` de structure identique et ne déclenche rien) ; les pragmas SA1200 de `Program.cs` et l'espacement autour de `var builder` non plus.

⚠️ Coquille repérée en passant, non corrigée (hors périmètre) : l'en-tête de copyright de `GameOn.Presentation/Program.cs` se termine par `// </copyright>ddd` — un `ddd` parasite collé à la balise fermante.

✅ Commentaires de code 100 % anglais (2026-09-17) — règle ajoutée aux « Interdictions Strictes » de `CLAUDE.md`, `AGENTS.md` et `CURSOR.md` (les trois fichiers sont des copies qui ont divergé : `AGENTS.md` et `CURSOR.md` sont identiques entre eux et en retard sur `CLAUDE.md`, pensez à les resynchroniser). 40 commentaires français corrigés :

- **28 étaient des `#pragma warning ... // <message Roslyn traduit>`**, dans 9 fichiers (`SummonerController`, `MatchV5Service`, les autres clients Riot, `UpdateLoLGameCommandHandler`, `UpdatePlayerSummonerCommandHandler`…). C'est le quick-fix « Supprimer l'avertissement » de Visual Studio en locale française qui recopie le libellé localisé. Remplacés par le libellé anglais officiel, en s'alignant sur ce que le repo utilisait déjà ailleurs (`GetConnectedPlayerQueryHandler` pour CS8602, `Persistence/DependencyInjection` pour CS8603) : CS8601 → `Possible null reference assignment.`, CS8602 → `Dereference of a possibly null reference.`, CS8603 → `Possible null reference return.`, CS8604 → `Possible null reference argument.`
- Les 12 autres étaient la justification du faux positif SA1009 sur les 3 `BackgroundService`, écrite en français plus tôt dans la même session. Traduite.
- ⚠️ **Ce qui reste accentué dans les sources est volontaire et ne doit pas être traduit** : ce sont des **chaînes**, pas des commentaires — le prompt de `LoLCoachPrompt`, tout le brief construit par `LoLCoachContextBuilder` (Raimmus s'adresse aux joueurs en français), et le `"Aucune description renseignée."` par défaut de `Tournament`/`TournamentDto`. Seule subtilité : `LoLCoachContextBuilder.GetDeathZone` a un `<returns>` anglais qui cite `"bot, moitié ennemie"` — c'est la valeur réellement renvoyée, pas du commentaire à traduire.
