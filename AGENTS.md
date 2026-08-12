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
