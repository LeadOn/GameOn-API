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
LoLGameParticipantStat — stats de performance dérivées par participant (KDA, CS/min, gold/min, dégâts/min, kill participation %, wards posés/détruits), relation 1:1 à clé partagée avec `LoLGameParticipant` (`LoLGameParticipant.Stats`). Calculées via `LoLGameParticipantStatCalculator` (partagé entre l'import live et le backfill), à partir des données déjà en base (dernière frame de timeline + kills d'équipe + events de wards) — aucun appel Riot supplémentaire
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

`GameOn.Persistence/GameOnContext.cs` a sa connection string **en dur** (credentials dev) au lieu de lire `DB_CONNECTION_STRING` — modification volontaire de l'utilisateur pour tester/migrer en local. Ne pas la reverter automatiquement ; à vérifier avec lui avant tout commit/push de ce fichier.

✅ Rattrapage de masse des `LoLGame.QueueId` fait en prod par l'utilisateur (2026-07-16, via `UPDATE` manuel en DBeaver).

✅ `LoLGame.QueueType` supprimé (front à faire basculer sur `QueueId`/`LoLQueue` via les routes `/lol/Queue` et `/lol/Queue/player/{playerId}` si pas déjà fait).

✅ Filtre par plage de date ajouté sur l'historique de parties LoL (2026-07-23) : `GetLastGamesPlayedQuery.StartDate`/`EndDate` (bornes inclusives sur `LoLGame.GameStart`), exposés en query params `startDate`/`endDate` sur `GET lol/Match/last` et `GET lol/Match/player/{playerId}`.

✅ Entité `LoLGameParticipantStat` ajoutée (2026-07-23) : stats dérivées par participant (KDA, kill participation %, CS/gold/dégâts par minute, wards). Calculées automatiquement à chaque import/update de partie (`UpdateLoLGameCommandHandler`), et backfillées pour tout l'historique via `POST Admin/lol/recompute-participant-stats` (rôle `gameon_admin`) — migration générée et appliquée par l'utilisateur, backfill exécuté en prod. Exposées nested sur `LoLGameParticipant.Stats` dans `GET lol/Match/{matchId}` et `GET lol/Match/player/{playerId}`.

⚠️ Bug identifié mais **non corrigé** : `LoLGame.GameStart`/`GameEnd` sont stockés via `.ToLocalTime()` (fuseau du serveur, sans offset) dans `UpdateLoLGameCommandHandler.cs`, alors que le front (`parseApiDate()` dans `lol-match.util.ts`) traite ces valeurs comme de l'UTC — double décalage horaire à l'affichage. Fix identifié (retirer les `.ToLocalTime()`, aligner les defaults de `LoLGame`/`LoLGameParticipant` sur `DateTime.UtcNow`) mais pas encore appliqué ; la décision sur le backfill des données historiques déjà en base reste à prendre avec l'utilisateur.
