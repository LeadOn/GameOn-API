# Valorant — récupérer le maximum de données HenrikDev

Plan d'implémentation rédigé le 2026-11-02, **implémenté le même jour** sur `features/valorant/full-data` (phases 1 à 5, une seule migration `Added_Valorant_Full_Data`, état détaillé dans `CLAUDE.md`) à partir de la spec OpenAPI de prod (`https://api.henrikdev.xyz/openapi.json`, version 4.6.0), de la doc `docs.henrikdev.xyz` et du changelog v4.10.0, comparés au code de `master` (`9e4be0e`).

Objectif : stocker tout ce que HenrikDev sait de nos joueurs, puis en dériver les stats que les trackers concurrents n'affichent pas (zones de la carte, side attaque/défense, type d'achat, clutchs tentés, trades, note officielle Riot).

---

## 0. Contraintes à garder en tête

- **Quota : 30 req/min par clé**, partagé entre le job de refresh, les refresh manuels et tout futur backfill (`HenrikDevRateLimitState` est un singleton). Aujourd'hui un refresh joueur coûte 4 appels (account, mmr v3, mmr-history v2, matches v4), toutes les 30 min.
- **v4.10.0 est marquée « Available on BETA »** dans le changelog, alors que la spec de prod décrit déjà ses champs (`performance`, accolades, agent mastery, Premier v2, compléments MMR). Rien ne garantit qu'ils soient remplis en prod : la Phase 0 doit le vérifier avant d'écrire la moindre migration.
- **Règle « zéro donnée »** (déjà appliquée à LoL) : toutes les nouvelles colonnes sont **nullables**, et `null` veut dire « inconnu », jamais 0. La doc HenrikDev précise d'ailleurs que les nouveaux champs MMR valent `null` sur les anciens enregistrements.
- **Peak RR** : depuis HenrikDev v4.5.0, `peak.rr` vaut **0 par défaut** (donnée Riot jugée fausse). `Player.ValorantPeakRr = 0` n'est donc pas un fait, il faut l'afficher comme inconnu.

---

## 0 bis. Résultats de la Phase 0 (2026-11-02)

39 appels réels en prod et en beta, sur les 6 comptes Valorant du crew (LeadOn et oaffew en détail). Réponses brutes dans `scripts/fixtures/henrikdev/` (hors git : PUUID et Riot IDs). **La prod et la beta renvoient exactement la même chose** : les nouveautés v4.10 sont déjà en prod.

**Match v4, sur 40 parties récentes :**

| Champ | Constat |
| - | - |
| `players[].performance` | ✅ Rempli en `competitive`, `unrated` et `swiftplay`. `null` en `skirmish_2v2`, en custom et sur les parties anciennes (2023). Les `ratings` sont des énumérations `double_down` / `down` / `neutral` / `up` / `double_up`. `grade` ne vaut que `pass` sur l'échantillon (on ignore s'il existe autre chose). `breakdown` contient des valeurs par manche (dégâts/manche, trades/manche…) et `score` un nombre de 100 à 320 environ |
| `kills[].location`, `player_locations` | ✅ 100 % / 99,7 %. 9 positions par kill (les vivants, hors victime). `weapon.type` ∈ `Weapon`, `Ability`, `Bomb`, `Melee`, `Fall` |
| `plant` / `defuse` : `location`, `player_locations` | ✅ 100 % |
| `damage_events` | ✅ Environ 220 lignes par partie |
| `metadata.mvp`, `teams[].mvp`, `teams[].placement`, `players[].team_number` | ✅ 100 % sur les parties récentes, `null` sur les anciennes |
| `metadata.party_rr_penaltys` | ✅ Rempli sauf en custom. Valeurs vues : `0.0` et `0.25` |
| `customization.bloomline` | Rempli, mais des GUID nuls (`00000000-…`) quand rien n'est équipé : à stocker en `null` |
| `rounds[].stats[].ability_casts` | ❌ **Toujours `null`** (0 sur 9 374 lignes) : à ne pas stocker |
| `drafted_ability_casts`, `teams[].health`, `teams[].team_number`, `premier_roster`, `metadata.premier`, `observers`, `coaches` | ❌ Toujours vides sur l'échantillon (aucun match Premier dedans) |
| `stayed_in_spawn` (par manche) | Jamais `true` sur l'échantillon |
| Parties anciennes (2023) | `v4/match` les sert encore (250-400 Ko chacune), mais **sans** `first_blood`, `winning_team_role`, `performance`, MVP ni `placement` |
| Custom 2v2 | `queue.id = ""`, `mode_type = "Skirmish"`, 4 joueurs |

**Pourquoi des valeurs vides** (vérifié le 2026-11-02 avec un appel `POST /valorant/v1/raw` qui renvoie la réponse Riot brute) :

- **Casts par manche** : Riot renvoie `roundResults[].playerStats[].ability = { grenadeEffects: null, ability1Effects: null, ability2Effects: null, ultimateEffects: null }` sur toutes les manches, y compris en compétitif. HenrikDev ne fait que relayer ce `null`. **La donnée n'existe pas**, ni chez Riot ni ailleurs.
- **Casts sur la partie** (`players[].ability_casts`) : remplis à 100 % en compétitif et en swiftplay. En **skirmish 2v2** (y compris en custom), Riot envoie `stats.abilityCasts: null`. Le seul `null` vu en non classé est un joueur parti en cours de partie (1 kill, 275 points de score en 13 manches).
- **Parties de 2023** : `first_blood`, `winning_team_role`, `performance` et MVP n'existaient pas encore dans les données Riot de l'époque.
- `performance` vient de champs bruts que Riot appelle littéralement `TempValueA` à `TempValueI` (`hasTempValues: true`), et que HenrikDev renomme. C'est une donnée encore « provisoire » chez Riot, qui peut changer ou disparaître sans préavis.

**Champs qui n'existent que dans la réponse brute** (`POST /valorant/v1/raw`, type `matchdetails`), absents de v4 :

- `participationPeriods` : début et fin de chaque session du joueur dans la partie. Permet de détecter les **déconnexions et les départs**.
- `spent` par manche dans `playerEconomies` (v4 ne donne que `loadout_value` et `remaining` par manche).
- `matchInfo.isEarlyCompletion`, `completionState`, `roundResultCode` (`Surrendered`…).
- `behaviorFactors` : `collisions`, `selfDamage`, `damageParticipationOutgoing`, `mouseMovement`…

L'endpoint brut accepte **plusieurs IDs de match en un seul appel**, donc ces champs coûteraient environ 1 appel par lot de parties. À décider en Phase 1 : le gain (départs, dépense réelle par manche) vaut-il un second format à parser ?

**MMR :**

- **mmr-history v2** : tous les nouveaux champs sont remplis sur les entrées récentes. `competitive_movement` vaut **toujours** `MOVEMENT_UNKNOWN`, donc inutile. Sur les vieilles entrées stockées, `queue_id = ""` et `match_length = 0`, à stocker en `null`.
- ⚠️ **`last_change` n'est pas l'écart réel de RR.** À 0 RR, une défaite protégée annonce `last_change = -22` alors que le RR ne bouge que de -4 (de 4 à 0). Une promotion annonce `+16` pour un vrai `+22`. Sur les 20 dernières parties d'oaffew, la somme des `last_change` vaut **-101 RR**, contre un vrai écart de **-43**. Le vrai écart se calcule par entrée, sans dépendre de l'entrée précédente : `elo − ((tier_before_update.id − 3) × 100 + rr_before_update)`, formule vérifiée sur les 25 entrées classées des deux comptes. **Conséquence directe : le RR net de `GET /val/home` (somme des `LastChange`) et `ValGameParticipant.RrChange` sont faux autour des changements de rang.** À corriger en priorité (Phase 2). Formule non vérifiée en Immortel/Radiant (aucun compte du crew).
- **mmr v3** : `seasonal` remonte loin (19 actes pour LeadOn depuis e1a1, 20 pour oaffew). `rank_protection_shields`, `rank_protection_status` (`Empty` / `Full`), `is_at_rank_protected_tier`, `lifetime_prestige` (ex. `{"DIAMOND": {"count": 6}}`) et `ranked_state` sont remplis. `peak.rr` vaut bien 0 partout.

**Historique :**

- **stored-matches** : 34 parties pour LeadOn (depuis 2023-05), 147 pour oaffew (depuis 2023-01). Le backfill est faisable.
- **stored-mmr-history** : peu profond. 14 entrées pour LeadOn (depuis 2024-10), 26 pour oaffew (depuis 2026-10-21, à peine plus que les 20 de mmr-history). Le gain est faible, mais l'appel ne coûte presque rien.
- `v4/by-puuid/matches?start=10` fonctionne. Une page de 10 parties pèse environ 3,5 Mo.

**Endpoints joueur :**

- **Accolades** : elles ne couvrent que les **11 à 20 dernières parties classées**, mais pour les 10 joueurs de chaque partie, avec un flag de record d'acte. Les types vus : `damage_per_round`, `assists`, `kills`, `plants`, `top_frag`, `first_blood`, `mvp`, `clutches`, `headshot_percentage`, `distinction`, `trades`, `aces`. Elles finissent par sortir de la fenêtre, donc **il faut les relever au moins une fois par jour** pour ne pas en perdre.
- **Maîtrise d'agent** : 30 agents par compte, avec le niveau de track et le niveau de flourish. `tracks[].name` et `modules` sont toujours `null`.
- **Premier** : 3 membres du crew ont une équipe (Oceiros et SkyBlood dans « Chèvre argentée », oaffew dans « Soria »), mais aucune n'est inscrite à la saison en cours. Rôle, date d'arrivée et 4 saisons d'historique sont fournis. 404 pour les autres comptes.
- `SuperMao` : 404 sur mmr v3 (jamais classé).

**Quota** : `x-ratelimit-limit: 30` confirmé. Au premier appel il ne restait que 6 requêtes, ce qui laisse penser que **la clé locale est la même que celle de la prod**, et que les tests locaux mangent le quota du job de prod.

---

## 1. Inventaire : ce qui existe et ce qu'on en garde

### 1.1 Endpoints déjà appelés

| Endpoint | Champs reçus mais jetés aujourd'hui |
| - | - |
| `GET v4/by-puuid/matches` / `v4/match/{id}` | voir 1.2, c'est là que se trouve l'essentiel |
| `GET v3/by-puuid/mmr` | `seasonal[]` (historique par acte), `current.leaderboard_placement`, `games_needed_for_rating`, `games_needed_for_leaderboard`, `rank_protection_shields`, `rank_protection_status`, `is_at_rank_protected_tier`, `latest_update` (détail RR), `lifetime_prestige`, `ranked_state` (badge d'acte / leaderboard masqués), `peak.ranking_schema` |
| `GET v2/by-puuid/mmr-history` | `queue_id`, `match_length`, `rr_before_update`, `tier_before_update`, `rr_performance_bonus`, `rr_penalty`, `afk_penalty`, `new_map_incentive_rr_forgiven`, `competitive_movement`, `is_placement_match`, `was_derank_protection_replenished` |
| `GET v2/by-puuid/account` | `platforms`, `updated_at` (sans intérêt) |

### 1.2 Match v4 : champs absents de nos DTOs ou non persistés

| Bloc | Champ | Intérêt |
| - | - | - |
| `players[]` | `performance.score`, `performance.ratings.grade`, `ratings.combat.{kill_impact, death_impact, damage, trades}`, `ratings.utility.{assists, plants, defuses, utility_usage}`, `breakdown.{adjusted_kills, adjusted_deaths, assists, damage, trades, utility_usage, plants, defuses}` | **La note de performance officielle de Riot** : c'est le gros morceau |
| `players[]` | `team_number`, `drafted_ability_casts`, `customization.bloomline` (niveau, clip, hype, fond), `customization.preferred_level_border` | Secondaire (bloomline = cosmétiques de maîtrise d'agent) |
| `metadata` | `mvp` (MVP de la partie selon Riot/HenrikDev), `party_rr_penaltys[]` (`party_id`, `penalty`), `premier` | Pénalité de RR liée à la taille du groupe, MVP officiel à comparer au nôtre |
| `teams[]` | `mvp`, `placement`, `health.{starting, remaining}`, `premier_roster` (id, nom, tag, membres, couleurs) | MVP par équipe, Premier |
| `kills[]` | `location {x,y}` (position de la victime), `player_locations[]` (position **et orientation** `view_radians` des 10 joueurs à l'instant du kill) | **Heatmaps, zones de la carte, distance de kill** |
| `rounds[].plant` / `defuse` | `location`, `player_locations[]` | Position du plant et placement de tout le monde à ce moment-là |
| `rounds[].stats[]` | `ability_casts` (par manche), `received_penalty`, `stayed_in_spawn` | Utilitaires par manche |
| `rounds[].stats[].damage_events[]` | cible (`player`), `damage`, `headshots`, `bodyshots`, `legshots` | Aujourd'hui réduit à une somme : on perd « qui a touché qui » |
| racine | `observers[]`, `coaches[]` | Faible intérêt |

### 1.3 Endpoints jamais appelés, utiles pour nos joueurs

| Endpoint | Ce qu'il apporte |
| - | - |
| `GET v1/by-puuid/stored-matches/{affinity}/{puuid}` (`mode`, `map`, `size`, `page`) | Historique long stocké chez HenrikDev (résumé + ID de chaque partie). Sans `size`, **tout l'historique stocké en un seul appel**. Base du backfill |
| `GET v2/by-puuid/stored-mmr-history/{affinity}/{platform}/{puuid}` (`size`, `page`) | Historique RR complet stocké, avec les mêmes champs que mmr-history v2. Comble les trous de `ValorantRankHistory` |
| `GET v4/by-puuid/matches?start=` | Pagination des matchs détaillés (10 max par appel). Aujourd'hui on ne lit que la première page |
| `GET v1/by-puuid/accolades/{affinity}/{platform}/{puuid}` | Les « goldstars » de fin de partie (`kills`, `first_blood`, `damage_per_round`, `clutches`, `aces`, `trades`, `headshot_percentage`, `distinction`, `assists`, `top_frag`, `plants`, `mvp`), avec record d'acte, et un récap all-time / par saison (nombre, meilleure valeur) |
| `GET v1/by-puuid/agent-mastery/{affinity}/{platform}/{puuid}[/{agent_id}]` | Maîtrise par agent : tracks et niveaux, flourish, cosmétiques. La version par agent ajoute des « modules » de stats quand ils existent |
| `GET v2/premier/players/{affinity}/{puuid}` | Équipe Premier du joueur : rôle, date d'arrivée, conférence, division, points, bilan V/D et manches par saison, promotion |
| `GET v1/premier/{id}/history` | Matchs Premier de l'équipe (à importer via `v4/match`) |

### 1.4 Volontairement hors périmètre

Esports VLR, boutique, crosshair, contenu du site, statut serveurs, `queue-status`, `content` (valorant-api.com couvre déjà les noms et icônes), `leaderboard v3` (le rang au leaderboard d'un membre est déjà dans `mmr v3`). `POST v1/raw` (données Riot brutes) ne sert que de **repli** si la Phase 0 montre qu'un champ manque dans v4.

---

## 2. Phases

### Phase 0 — Fixtures réelles ✅ faite le 2026-11-02 (39 appels, résultats en 0 bis)

1. Appeler une fois chaque endpoint de 1.1 et 1.3 sur deux comptes du crew (un classé, un avec Premier si possible), en **prod** puis en **beta** (`HENRIKDEV_API_BASE_URL=https://beta.api.henrikdev.xyz`, déjà configurable).
2. Sauver les réponses JSON dans `scripts/fixtures/henrikdev/` **hors git** (PUUID et Riot IDs du crew), ou les anonymiser avant de commiter.
3. Produire le tableau « champ → rempli en prod / null / absent ». Points à trancher avec ce tableau :
   - `performance` est-il rempli en prod, et sur quelles files ?
   - Que contient `drafted_ability_casts`, et dans quel mode ?
   - `teams[].health` : dans quel mode ?
   - `LastChange` inclut-il `refunded_rr` ? Vérifiable avec `rr_before_update` : `LastChange == Rr - RrBeforeUpdate` hors changement de tier. Ça tranche la question laissée ouverte sur le RR net de la home.
   - Format exact des valeurs de `ratings` (des lettres ? des pourcentages ?), en `string` dans la spec.
   - Est-ce que `v4/match/{id}` répond pour un ID ancien sorti de `stored-matches` ?

### Phase 1 — Compléter l'import des matchs v4 (1 migration, 0 appel de plus)

**External** : compléter `ValorantMatchPlayerDto` (`Performance`, `TeamNumber`, `DraftedAbilityCasts`), `ValorantMatchCustomizationDto` (`Bloomline`), `ValorantMatchMetadataDto` (`PartyRrPenalties`, `Premier` en `JToken`), `ValorantMatchTeamDto` (`Mvp`, `Health`, `PremierRoster`), `ValorantMatchKillDto` (`Location`, `PlayerLocations`), `ValorantMatchSpikeEventDto` (`Location`, `PlayerLocations`), `ValorantMatchRoundPlayerStatsDto` (`AbilityCasts`, `ReceivedPenalty`), plus les DTOs `ValorantMatchLocationDto` et `ValorantMatchPlayerLocationDto` (`Player`, `Location`, `ViewRadians`), `ValorantMatchPerformanceDto` et les sous-blocs.

**Domain / Persistence** :

- **`ValGameParticipantPerformance`** (1:1 à clé partagée avec `ValGameParticipant`, même pattern que `LoLGameParticipantChallenge`) : `Score`, `Grade`, les 4 ratings combat et les 4 ratings utilitaires, les 8 valeurs de `breakdown`. Comme pour `LoLGameCoachReport.Rating`, on ne classe pas sur `Grade` tant qu'on ne sait pas s'il est comparable d'une file à l'autre.
- **`ValGameParticipant`** : `TeamNumber`, `PartyRrPenalty` (retrouvé via `PartyId`), `IsRiotMatchMvp`, `IsRiotTeamMvp`, `BloomlineLevel`, `PreferredLevelBorderId`.
- **`ValGameTeam`** : `Placement`, `HealthStarting`, `HealthRemaining`, `PremierRosterId`, `PremierTeamName`, `PremierTeamTag`.
- **`ValGameKill`** : `VictimX`, `VictimY` (depuis `location`), `KillerX`, `KillerY`, `KillerViewRadians` (le tueur retrouvé dans `player_locations`). C'est ce qui sert à 90 % des requêtes (heatmaps, zones, distance de kill), donc en colonnes directes.
- **`ValGameRound`** : `PlantX`, `PlantY`, `DefuseX`, `DefuseY`.
- **`ValGameEventPlayerLocation`** (nouvelle table) : `MatchId`, `RoundNumber`, `EventType` (`Kill`/`Plant`/`Defuse`), `ValGameKillId?`, `Puuid`, `Team`, `X`, `Y`, `ViewRadians`. C'est l'instantané complet des 10 joueurs à chaque événement. Volume : environ 10 lignes × (kills + plant + defuse), soit 1 500 à 2 500 lignes par partie. **À trancher** : la garder (analyses de placement, setups, rotations) ou s'en tenir aux colonnes tueur/victime.
- **`ValGameRoundPlayerStat`** : `ReceivedPenalty`, `StayedInSpawn`. Les casts par manche ne sont **pas** ajoutés : HenrikDev les renvoie toujours `null` (cf. 0 bis).
- **Parties anciennes** : sans `first_blood` ni `winning_team_role`, `ValGameRound` restait à `null` sur ces champs. À l'import, on recalcule le first blood depuis le premier kill de la manche, et le side depuis le plant (l'équipe qui plante attaque, et on propage aux autres manches de la mi-temps).
- **Pas stockés**, parce que toujours vides sur l'échantillon : `drafted_ability_casts`, `teams[].health`, `teams[].team_number`, `observers`, `coaches`. On garde `metadata.premier` / `premier_roster` (les matchs Premier existent, ils n'étaient juste pas dans l'échantillon).
- **`ValGameRoundDamage`** (nouvelle table) : `MatchId`, `RoundNumber`, `AttackerPuuid`, `ReceiverPuuid`, `Damage`, `Headshots`, `Bodyshots`, `Legshots`. Environ 500 à 800 lignes par partie. `ValGameRoundPlayerStat.Damage` reste la somme, pour ne rien casser.
- Index : `ValGameEventPlayerLocation (MatchId, RoundNumber)` et `ValGameRoundDamage (MatchId, ReceiverPuuid)`.

**Application** : mapping dans `UpsertValorantMatchesCommandHandler` (refresh et `POST /val/match/{id}/update` en profitent automatiquement). Exposer les nouveaux champs dans `GET /val/match/{id}` (`performance` nested sur le participant) et `GET /val/match/{id}/rounds` (`locations`, `damages`).

**Rattrapage** : route admin `POST /val/match/reimport?playerId=&since=` (`gameon_admin`) qui repasse les parties en base avec `OverwriteExisting = true`. Coût : 1 appel par partie, ce qui reste négligeable tant que la table `ValorantGame` est petite.

**Pense-bête** : remettre `// <auto-generated />` en tête du `.cs` de migration (cf. `CLAUDE.md`).

### Phase 2 — RR et rang complets (1 migration, 0 appel de plus)

- 🔴 **Corriger le RR par partie en premier** (bug constaté en Phase 0, cf. 0 bis) : nouvelle colonne `ValorantRankHistory.RrDelta`, calculée par `elo − ((TierBeforeId − 3) × 100 + RrBeforeUpdate)` et `null` si l'un des deux tiers est Unrated. `GetValHomeStatsQueryHandler` (RR net, jours, `FactOfTheWeek`) et le croisement `ValGameParticipant.RrChange` passent sur `RrDelta`. `LastChange` reste stocké tel quel : c'est la valeur annoncée par Riot, utile pour afficher « -22, protégé à 0 ».
- **`ValorantRankHistory`** : `QueueId`, `MatchLengthMs`, `RrBeforeUpdate`, `TierBeforeId`, `TierBeforeName`, `RrPerformanceBonus`, `RrPenalty`, `AfkPenalty`, `NewMapIncentiveRrForgiven`, `IsPlacementMatch`, `WasDerankProtectionReplenished`. `""` et `0` des vieilles entrées stockées → `null`. `competitive_movement` n'est pas stocké (toujours `MOVEMENT_UNKNOWN`). Tout est nullable, et les lignes existantes se remplissent au prochain passage de mmr-history (upsert par `(PlayerId, MatchId)` au lieu d'un simple insert si absent).
- **`Player`** (lecture courante de `mmr v3`) : `ValorantRankProtectionShields`, `ValorantRankProtectionStatus`, `ValorantIsAtRankProtectedTier`, `ValorantGamesNeededForRating`, `ValorantGamesNeededForLeaderboard`, `ValorantLeaderboardRank`, `ValorantLeaderboardUpdatedOn`, `ValorantIsActRankBadgeHidden`, `ValorantIsLeaderboardAnonymized`, `ValorantPeakRankingSchema`.
- **`ValorantSeasonResult`** (nouvelle table, une ligne par joueur et par acte, upsert à chaque refresh depuis `mmr v3 → seasonal[]`) : `SeasonId`, `SeasonShort`, `ActRankTierId/Name`, `EndTierId/Name`, `EndRr`, `Games`, `Wins`, `WinsWithPlacements`, `GamesNeededForRating`, `LeaderboardRank`, `RankingSchema`, `PrestigeJson`, et `ActWinsJson` (la liste des tiers des victoires, c'est-à-dire le triangle de badge d'acte ; en JSON parce qu'elle n'est lue qu'en entier).
- **Routes** : `GET /val/player/{id}/seasons` (historique par acte), et un `rrBreakdown` (base, bonus de perf, pénalités, remboursement, protection) sur la participation du joueur dans `GET /val/match/{id}`.
- Une fois la Phase 0 tranchée, revoir le calcul du RR net de `GET /val/home` (inclusion ou non de `RefundedRr`).

### Phase 3 — Backfill de l'historique (gros volume d'appels, étalés)

C'est ce qui corrige les jours « RR ≠ 0 mais 0 partie » de la home : l'historique MMR connaît des parties que `ValorantGame` n'a jamais importées.

**External** : `IValorantStoredDataService` avec `GetStoredMatches(region, puuid, mode, size, page)` et `GetStoredMmrHistory(region, platform, puuid, size, page)`, en typed client derrière `HenrikDevRateLimitHandler` comme les trois autres. Ajouter `start` à `IValorantMatchService.GetLastMatchesByPuuid`.

**Application** :

1. `BackfillValorantRankHistoryCommand(PlayerId?)` : lit `stored-mmr-history v2` page par page et upsert `ValorantRankHistory` par `(PlayerId, MatchId)`. Quelques appels par joueur.
2. `BackfillValorantMatchesCommand(PlayerId?, Since?)` : un appel `stored-matches` sans `size` pour obtenir la liste complète des IDs, différence avec `ValorantGame`, puis `v4/match/{affinity}/{id}` pour chaque partie manquante, envoyée à `UpsertValorantMatchesCommand`. 1 appel par partie. Idempotent : l'état, c'est ce qui est en base, donc on peut l'interrompre et le relancer.
3. Lancement par la route admin `POST /val/admin/backfill?playerId=` et un `BackgroundService` à faible priorité qui traite une file de matchs manquants.

**Débit** : le backfill ne doit jamais affamer le refresh ni les clics des joueurs. Ajouter à `HenrikDevRateLimitState` une notion de **réserve** : une requête marquée basse priorité attend tant que `x-ratelimit-remaining` est sous ~10. Ordre de grandeur : 1 000 parties à ~20 req/min, soit environ 50 min par tranche de 1 000.

**Limite connue** : `stored-matches` est un sous-ensemble **accumulé** par HenrikDev (une partie n'y est que si quelqu'un l'a demandée un jour), pas un miroir complet de Riot. Les plus vieilles parties peuvent manquer, c'est un plafond qu'on ne peut pas dépasser.

**Refresh régulier** : si les 10 parties renvoyées par `v4/by-puuid/matches` sont toutes inconnues, continuer avec `start=10`, `start=20`… jusqu'à retomber sur une partie connue (borné à 3 pages). Ça bouche les trous laissés par un refresh raté (redéploiement, 429) sans attendre un backfill.

### Phase 4 — Référentiel carte et stats dérivées (0 appel HenrikDev)

C'est la phase qui fait la différence face aux trackers.

**Référentiel** : `IValorantMapService` (GameOn.External, `valorant-api.com/v1/maps`, caché 24 h comme `CommunityDragonChampionService`). On récupère `xMultiplier`, `yMultiplier`, `xScalarToAdd`, `yScalarToAdd` (conversion des coordonnées du jeu vers la minimap) et `callouts[]` (`regionName`, `superRegionName`, position). Le callout le plus proche donne une zone lisible (« A Main », « Mid Courtyard »), comme `GetDeathZone` pour LoL. On ajoute `VictimZone` / `KillerZone` sur `ValGameKill`, calculés à l'import. `valorant-api.com/v1/weapons` donne la catégorie et le coût de chaque arme, et `?language=fr-FR` les noms en français.

**Nouvelles stats par participant** (colonnes de `ValGameParticipantStat`, ou une table `ValGameParticipantAdvancedStat` 1:1) :

| Stat | Calcul |
| - | - |
| Attaque / défense : manches, manches gagnées, ACS, K/D, first kills par side | Side de chaque manche déduit de `WinningTeam` + `WinningTeamRole`, avec l'inversion à la manche 13 et l'alternance en prolongations. **Faisable dès aujourd'hui** avec les données en base |
| Pistols (manches 1 et 13), éco / force / full buy / bonus | Seuils sur `ValGameRoundPlayerStat.LoadoutValue` (déjà stocké). Win rate par type d'achat, kills en éco face à un full buy adverse |
| Trades : kills de trade et morts tradées | Même fenêtre de 5 s que le KAST de `ValGameParticipantStatCalculator` |
| Clutchs tentés et gagnés (1v1 à 1v5) | Joueurs en vie reconstruits depuis la timeline des kills. `CeremonyClutch` ne donne que les victoires |
| Post-plant et retake : win % après plant par site, defuses en retake | `ValGameRound` (plant, defuse, vainqueur) |
| Duels d'ouverture par zone, heatmaps kills / morts | Positions de la Phase 1 + callouts |
| Distance moyenne de kill, kills par arme et par catégorie, kills en tir secondaire | Positions + `Weapon*` + `SecondaryFireMode` |
| Utilitaires par manche | Casts par manche (Phase 1) |
| Bête noire / souffre-douleur | `ValGameRoundDamage` + kills par adversaire |

**Routes** :

- `GET /val/player/{id}` : `ValorantPerformanceStats` gagne `sides`, `buyTypes`, `clutches`, `trades`, `weapons`, `zones`, et la note Riot moyenne en plus de notre `AverageRating`.
- `GET /val/player/{id}/heatmap?map=&side=&type=kills|deaths&period=&queues=` : renvoie les points déjà convertis en coordonnées minimap, prêts à dessiner.
- `GET /val/stats/global` : nouveaux awards, avec garde-fou « zéro donnée » et départages déterministes comme les autres : roi du clutch (1vX gagnés), guerrier de l'éco, maître du retake, sniper (kills à l'Operator), « Tu peux pas test » (plus long kill).

Rétroactif : tout le calcul tourne sur les données en base. Un `POST /val/admin/recompute-stats` repasse les parties existantes sans aucun appel HenrikDev.

### Phase 5 — Nouveaux endpoints joueur (+3 appels par joueur et par jour)

À sortir du refresh des 30 min : un `RefreshValorantPlayerExtrasJob` **quotidien**, qui passe par la même file à faible priorité que le backfill.

- **Maîtrise d'agent** : `ValorantAgentMastery` (`PlayerId`, `AgentId`, `AgentName`, `TracksJson`, niveau principal, `FlourishShortLevel`, `FlourishLongLevel`, IDs des cosmétiques, `RefreshedOn`). Snapshot remplacé à chaque passage. Exposée dans `GET /val/player/{id}/mastery` et dans la carte agent du profil.
- **Accolades** (passage quotidien obligatoire : l'endpoint ne couvre que les 11 à 20 dernières parties classées) : `ValorantAccoladeSummary` (`PlayerId`, `Scope` AllTime/Season, `SeasonId`, `AccoladeId`, `Type`, `Count`, `BestValue`), plus `ValGameParticipantAccolade` (`ValGameParticipantId`, `AccoladeId`, `Type`, `Value`, `IsActRecord`) relié par `match_id` + `puuid` quand la partie est en base. Exposées sur le profil (« records perso ») et sur le détail de match.
- **Premier** : `ValorantPremierTeam` (roster id, nom, tag, couleurs, icône, saison courante : conférence, division, points, bilan ; `SeasonsJson`), membres reliés par `Player.ValorantPremierTeamId` + rôle + date d'arrivée. `GET /val/premier/{teamId}`. Ensuite, `v1/premier/{id}/history` alimente la file de backfill avec les matchs Premier.

---

## 3. Budget de quota

| Poste | Appels | Fréquence |
| - | - | - |
| Refresh actuel (account, mmr, mmr-history, matches) | 4 par joueur | toutes les 30 min |
| Pagination `start` du refresh | 0 à 2 de plus, rarement | seulement s'il y a un trou |
| Extras (mastery, accolades, Premier) | 3 par joueur | 1 fois par jour |
| Backfill MMR | ~1 à 5 par joueur | une fois, puis à la demande |
| Backfill matchs | 1 par partie manquante | une fois, étalé |
| Réimport Phase 1 | 1 par partie en base | une fois |

Avec 15 joueurs Valorant, le régime courant tourne autour de 2 req/min : la marge pour le backfill est large. Si le backfill est trop lent, HenrikDev propose des plans Premium (limites plus hautes, cache plus court, webhooks), cf. changelog v4.9.0.

---

## 4. Ordre recommandé

1. ~~**Phase 0**~~ : faite le 2026-11-02, cf. 0 bis.
2. **Phases 2 puis 1** sur une branche `features/valorant/full-data`, chacune avec sa migration, puis le réimport. La Phase 2 passe devant parce qu'elle corrige un chiffre faux déjà affiché (le RR net de la home).
3. **Phase 3** (backfill) : plus on attend, plus HenrikDev risque d'avoir purgé les anciennes parties.
4. **Phase 4** : la partie « anti-concurrence », déjà en partie faisable sans les phases 1 à 3 (side, achats, pistols, trades, clutchs).
5. **Phase 5** : bonus, dépend de la disponibilité réelle de v4.10 en prod.

## 5. Points à trancher

1. ~~`ValGameEventPlayerLocation` complète ou seulement tueur et victime ?~~ Les deux : colonnes tueur/victime sur `ValGameKill` **et** instantané complet (choix « maximum de données »).
2. ~~Archiver la réponse v4 brute dans MinIO~~ : **écarté** (décision utilisateur du 2026-11-02). Un champ ajouté plus tard se rattrape donc par réimport (1 appel par partie), tant que HenrikDev garde la partie.
3. ~~Note Riot à côté ou à la place de notre Rating ?~~ À côté (`AverageRiotPerformanceScore`), notre Rating continue d'élire MVP/ACE. L'élection du MVP/ACE reste sur la nôtre tant que la note Riot n'est pas garantie sur toutes les files.
4. Premium HenrikDev pour accélérer le backfill : oui ou non ?
