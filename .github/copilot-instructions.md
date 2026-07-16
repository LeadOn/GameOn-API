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
GameOn.Persistence	EF Core + MySQL (GameOnContext)
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

## 🚧 Chantier en cours : Table des Queue Types League of Legends

**Branche :** `features/league-of-legends/history`

**Objectif :** `LoLGame.QueueType` est aujourd'hui une string libre. On introduit une table `LeagueOfLegendsQueue` alimentée depuis l'endpoint public Riot `https://static.developer.riotgames.com/docs/lol/queues.json`, pour normaliser les types de queue en base.

Plan en 5 étapes, vérifié avec l'utilisateur à chaque étape :

1. **✅ FAIT — Création de la table**
   - `GameOn.Domain/LoLQueue.cs` : entité `Id` (= `queueId` Riot, **clé naturelle**, pas d'auto-increment), `Map`, `Description?`, `Notes?`.
   - `GameOn.Common/Interfaces/IApplicationDbContext.cs` + `GameOn.Persistence/GameOnContext.cs` : `DbSet<LoLQueue> LeagueOfLegendsQueues` + config Fluent API (table `LeagueOfLegendsQueue`, `entity.Property(e => e.Id).ValueGeneratedNever()` car la clé vient de Riot, pas d'identity SQL).
   - Migration `GameOn.Persistence/Migrations/20260716114438_Added_LeagueOfLegends_Queue_Table.cs` générée et appliquée en dev.
   - ⚠️ **Piège rencontré :** `dotnet ef migrations add --startup-project GameOn.Presentation` échoue (`GameOn.Presentation` ne référence pas `EFCore.Design`, et `PrivateAssets=all` sur ce package dans `GameOn.Persistence.csproj` bloque la propagation transitive). Solution : utiliser `--startup-project GameOn.Persistence` à la place (le `DbContext` lit sa connection string directement via `Environment.GetEnvironmentVariable`, pas besoin de factory).

2. **✅ FAIT — Import automatique quotidien**
   - `GameOn.External/RiotGames/Models/DTOs/QueueDto.cs`, `Interfaces/IQueueService.cs`, `Implementations/QueueService.cs` (GET `queues.json`, pas de clé API requise), enregistré dans `GameOn.External/DependencyInjection.cs`.
   - `GameOn.Application/LeagueOfLegends/Queues/Commands/SyncQueues/` : `SyncQueuesCommand` + `SyncQueuesCommandHandler` (upsert par `Id`).
   - `GameOn.Application/Services/Jobs/RefreshLeagueQueuesJob.cs` (`BackgroundService` + `PeriodicTimer` 24h, même pattern que `RefreshLeagueSummonerRanksJob`), enregistré via `AddHostedService` dans `GameOn.Application/DependencyInjection.cs`.
   - **Pas de Validator FluentValidation** ajouté : décision explicite de l'utilisateur — aucun `Command`/`Query` du repo n'en a aujourd'hui et le package n'est même pas installé, malgré ce que ce fichier documente plus haut. Introduire FluentValidation reste un chantier séparé à faire sur un command qui a de vraies règles à valider.

3. **✅ FAIT (endpoint prêt, import PAS ENCORE déclenché)**
   - `GameOn.Presentation/Controllers/LeagueOfLegends/QueueController.cs` : `POST /lol/Queue/sync`, `[Authorize(Roles = "gameon_admin")]`, appelle `SyncQueuesCommand`, retourne `204`.
   - ⏳ **À faire par l'utilisateur :** appeler cet endpoint une première fois (Swagger/Postman, JWT admin `gameon_admin`) pour peupler la table.

4. **⬜ À FAIRE — Lier `LoLGame` à `LoLQueue`**
   - Ajouter la liaison entre `LoLGame` et `LoLQueue` (FK). À trancher avec l'utilisateur : remplacer `LoLGame.QueueType` (string) ou ajouter un nouveau champ à côté le temps de la migration.

5. **⬜ À FAIRE — Migration des données existantes**
   - Script pour définir automatiquement les liens `LoLGame` → `LoLQueue` à partir des `QueueType` (string) déjà en base, en fonction du mapping Riot par version de jeu.

**⚠️ État local à surveiller avant tout commit :** `GameOn.Persistence/GameOnContext.cs` a actuellement sa connection string **en dur** (credentials dev) au lieu de lire `DB_CONNECTION_STRING` — modification volontaire de l'utilisateur pour tester en local. Il a explicitement demandé de **ne pas** la reverter automatiquement. À vérifier avec lui avant tout commit/push de ce fichier.

*(Cette section est un suivi d'avancement temporaire — à supprimer une fois les 5 étapes terminées et validées.)*
