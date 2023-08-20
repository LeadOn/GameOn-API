// <copyright file="FifaGamePlayedBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Business
{
    using YuGames.Business.Contracts;
    using YuGames.DTOs;
    using YuGames.Entities;
    using YuGames.Repository.Contracts;

    /// <summary>
    /// Player business.
    /// </summary>
    public class FifaGamePlayedBusiness : IFifaGamePlayedBusiness
    {
        private IPlayerRepository playerRepo;
        private IFifaGamePlayedRepository gamePlayedRepo;
        private ITeamPlayerRepository teamPlayerRepo;
        private IPlatformRepository platformRepo;
        private IFifaTeamRepository fifaTeamRepo;
        private IHighlightRepository highlightRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="FifaGamePlayedBusiness" /> class.
        /// </summary>
        /// <param name="playerRepo">Player repository, injected.</param>
        /// <param name="gamePlayedRepo">GamePlayed repository, injected.</param>
        /// <param name="teamPlayerRepo">TeamPlayer repository, injected.</param>
        /// <param name="platformRepo">Platform repository, injected.</param>
        /// <param name="fifaTeamRepo">FifaTeam repository, injected.</param>
        /// <param name="highlightRepo">Highlight repository, injected.</param>
        public FifaGamePlayedBusiness(IPlayerRepository playerRepo, IFifaGamePlayedRepository gamePlayedRepo, ITeamPlayerRepository teamPlayerRepo, IPlatformRepository platformRepo, IFifaTeamRepository fifaTeamRepo, IHighlightRepository highlightRepo)
        {
            this.playerRepo = playerRepo;
            this.gamePlayedRepo = gamePlayedRepo;
            this.teamPlayerRepo = teamPlayerRepo;
            this.platformRepo = platformRepo;
            this.fifaTeamRepo = fifaTeamRepo;
            this.highlightRepo = highlightRepo;
        }

        /// <inheritdoc />
        public async Task<FifaGamePlayed?> Create(CreateGameDto createGameDto)
        {
            // First, getting that platform
            var platformInDb = await this.platformRepo.GetById(createGameDto.PlatformId);

            if (platformInDb is null)
            {
                return null;
            }

            // Then, getting creator profile
            var creatorInDb = await this.playerRepo.GetPlayerByKeycloakId(createGameDto.KeycloakId);
            if (creatorInDb is null)
            {
                return null;
            }

            // Now, getting all player accounts
            foreach (var playerId in createGameDto.Team1)
            {
                var playerInDb = await this.playerRepo.GetPlayerById(int.Parse(playerId));
                if (playerInDb is null)
                {
                    return null;
                }
            }

            foreach (var playerId in createGameDto.Team2)
            {
                var playerInDb = await this.playerRepo.GetPlayerById(int.Parse(playerId));
                if (playerInDb is null)
                {
                    return null;
                }
            }

            var newGame = new FifaGamePlayed
            {
                PlatformId = platformInDb.Id,
                PlayedOn = createGameDto.CreatedOn,
                TeamCode1 = createGameDto.TeamCode1,
                TeamCode2 = createGameDto.TeamCode2,
                TeamScore1 = createGameDto.TeamScore1,
                TeamScore2 = createGameDto.TeamScore2,
                CreatedById = creatorInDb.Id,
            };

            if (newGame.TeamScore1 < 0 || newGame.TeamScore2 < 0)
            {
                return null;
            }

            // Checking if fifa teams are in Database.
            if (createGameDto.FifaTeam1 is not null && createGameDto.FifaTeam1 != 0)
            {
                var fifaTeam1 = await this.fifaTeamRepo.GetById((int)createGameDto.FifaTeam1);
                if (fifaTeam1 is not null)
                {
                    newGame.Team1Id = fifaTeam1.Id;
                }
            }

            if (createGameDto.FifaTeam2 is not null && createGameDto.FifaTeam2 != 0)
            {
                var fifaTeam2 = await this.fifaTeamRepo.GetById((int)createGameDto.FifaTeam2);
                if (fifaTeam2 is not null)
                {
                    newGame.Team2Id = fifaTeam2.Id;
                }
            }

            // Now that every player as been found, creating elements in db
            var gameInDb = await this.gamePlayedRepo.Create(newGame);

            if (gameInDb is null)
            {
                return null;
            }
            else
            {
                // Now creating team players
                foreach (var playerId in createGameDto.Team1)
                {
                    var teamPlayerInDb = await this.teamPlayerRepo.CreateTeamPlayer(new FifaTeamPlayer
                    {
                        FifaGameId = gameInDb.Id,
                        PlayerId = int.Parse(playerId),
                        Team = 0,
                    });

                    if (teamPlayerInDb is null)
                    {
                        return null;
                    }
                }

                foreach (var playerId in createGameDto.Team2)
                {
                    var teamPlayerInDb = await this.teamPlayerRepo.CreateTeamPlayer(new FifaTeamPlayer
                    {
                        FifaGameId = gameInDb.Id,
                        PlayerId = int.Parse(playerId),
                        Team = 1,
                    });

                    if (teamPlayerInDb is null)
                    {
                        return null;
                    }
                }
            }

            return gameInDb;
        }

        /// <inheritdoc />
        public async Task<FifaGamePlayed?> Update(UpdateGameDto fifaGame)
        {
            // First, getting game from database
            var gameInDb = await this.gamePlayedRepo.GetById(fifaGame.Id);

            if (gameInDb is null)
            {
                return null;
            }

            // Updating game data
            gameInDb.TeamCode1 = fifaGame.TeamCode1;
            gameInDb.TeamCode2 = fifaGame.TeamCode2;
            gameInDb.TeamScore1 = fifaGame.TeamScore1;
            gameInDb.TeamScore2 = fifaGame.TeamScore2;

            if (gameInDb.TeamScore1 < 0 || gameInDb.TeamScore2 < 0)
            {
                return null;
            }

            // Getting wanted platform
            var platformInDb = await this.platformRepo.GetById(fifaGame.PlatformId);
            if (platformInDb is null)
            {
                return null;
            }

            gameInDb.PlatformId = platformInDb.Id;

            if (fifaGame.FifaTeam1 is not null)
            {
                // Getting Fifa Team 1
                var fifaTeam1InDb = await this.fifaTeamRepo.GetById((int)fifaGame.FifaTeam1);

                if (fifaTeam1InDb is null)
                {
                    return null;
                }

                gameInDb.Team1Id = fifaTeam1InDb.Id;
            }

            if (fifaGame.FifaTeam2 is not null)
            {
                // Getting Fifa Team 1
                var fifaTeam2InDb = await this.fifaTeamRepo.GetById((int)fifaGame.FifaTeam2);

                if (fifaTeam2InDb is null)
                {
                    return null;
                }

                gameInDb.Team2Id = fifaTeam2InDb.Id;
            }

            // Updating game in database
            var updatedGame = await this.gamePlayedRepo.Update(gameInDb);

            return updatedGame;
        }

        /// <inheritdoc/>
        public async Task<GamePlayedDto?> GetById(int gameId)
        {
            // First, getting game
            var gameInDb = await this.gamePlayedRepo.GetById(gameId);

            if (gameInDb is null)
            {
                return null;
            }

            var gamePlayedDto = new GamePlayedDto();
            gamePlayedDto.CreatedBy = gameInDb.CreatedBy;
            gamePlayedDto.Id = gameInDb.Id;
            gamePlayedDto.PlayedOn = gameInDb.PlayedOn;
            gamePlayedDto.Team1 = new TeamDto
            {
                Id = 0,
                FifaTeamId = gameInDb.Team1Id,
                Code = gameInDb.TeamCode1 ?? "Unknown",
                Players = new List<Player>(),
                Score = gameInDb.TeamScore1,
            };
            gamePlayedDto.Team2 = new TeamDto
            {
                Id = 1,
                FifaTeamId = gameInDb.Team2Id,
                Code = gameInDb.TeamCode2 ?? "Unknown",
                Players = new List<Player>(),
                Score = gameInDb.TeamScore2,
            };

            if (gameInDb.Platform is not null)
            {
                gamePlayedDto.Platform = gameInDb.Platform.Name;
            }

            // Getting team players
            foreach (var teamPlayer in gameInDb.TeamPlayers)
            {
                if (teamPlayer.Team == 0)
                {
                    var player = await this.playerRepo.GetPlayerById(teamPlayer.PlayerId);
                    if (player is not null)
                    {
                        gamePlayedDto.Team1.Players.Add(player);
                    }
                }
                else
                {
                    var player = await this.playerRepo.GetPlayerById(teamPlayer.PlayerId);
                    if (player is not null)
                    {
                        gamePlayedDto.Team2.Players.Add(player);
                    }
                }
            }

            gamePlayedDto.Highlights = gameInDb.Highlights;

            return gamePlayedDto;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GamePlayedDto>> GetLastGamesPlayed(int number)
        {
            var gamesPlayedDto = new List<GamePlayedDto>();

            // First, getting last games (from GamePlayed table)
            var gamesPlayed = await this.gamePlayedRepo.Search(x => true, number);

            // For each game played, getting player information
            foreach (var game in gamesPlayed)
            {
                var gamePlayedDto = new GamePlayedDto();
                gamePlayedDto.CreatedBy = game.CreatedBy;
                gamePlayedDto.Id = game.Id;
                gamePlayedDto.PlayedOn = game.PlayedOn;
                gamePlayedDto.Team1 = new TeamDto
                {
                    Id = 0,
                    FifaTeamId = game.Team1Id,
                    Code = game.TeamCode1 ?? "Unknown",
                    Players = new List<Player>(),
                    Score = game.TeamScore1,
                };
                gamePlayedDto.Team2 = new TeamDto
                {
                    Id = 1,
                    FifaTeamId = game.Team2Id,
                    Code = game.TeamCode2 ?? "Unknown",
                    Players = new List<Player>(),
                    Score = game.TeamScore2,
                };

                if (game.Platform is not null)
                {
                    gamePlayedDto.Platform = game.Platform.Name;
                }

                // Getting team players
                var teamPlayers = await this.teamPlayerRepo.GetTeamPlayersByGameId(game.Id);
                foreach (var teamPlayer in teamPlayers)
                {
                    if (teamPlayer.Team == 0)
                    {
                        var player = await this.playerRepo.GetPlayerById(teamPlayer.PlayerId);
                        if (player is not null)
                        {
                            gamePlayedDto.Team1.Players.Add(player);
                        }
                    }
                    else
                    {
                        var player = await this.playerRepo.GetPlayerById(teamPlayer.PlayerId);
                        if (player is not null)
                        {
                            gamePlayedDto.Team2.Players.Add(player);
                        }
                    }
                }

                gamePlayedDto.Highlights = game.Highlights;

                gamesPlayedDto.Add(gamePlayedDto);
            }

            return gamesPlayedDto;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<GamePlayedDto>> GetLastGamesPlayedByPlayerId(int playerId, int limit)
        {
            var gamesPlayedDto = new List<GamePlayedDto>();

            // First, getting last games (from GamePlayed table)
            var gamesPlayed = await this.gamePlayedRepo.Search(x => x.TeamPlayers.FirstOrDefault(x => x.PlayerId == playerId) != null, limit);

            // For each game played, getting player information
            foreach (var game in gamesPlayed)
            {
                var gamePlayedDto = new GamePlayedDto();
                gamePlayedDto.CreatedBy = game.CreatedBy;
                gamePlayedDto.Id = game.Id;
                gamePlayedDto.PlayedOn = game.PlayedOn;
                gamePlayedDto.Team1 = new TeamDto
                {
                    Id = 0,
                    FifaTeamId = game.Team1Id,
                    Code = game.TeamCode1 ?? "Unknown",
                    Players = new List<Player>(),
                    Score = game.TeamScore1,
                };
                gamePlayedDto.Team2 = new TeamDto
                {
                    Id = 1,
                    FifaTeamId = game.Team2Id,
                    Code = game.TeamCode2 ?? "Unknown",
                    Players = new List<Player>(),
                    Score = game.TeamScore2,
                };

                if (game.Platform is not null)
                {
                    gamePlayedDto.Platform = game.Platform.Name;
                }

                // Getting team players
                var teamPlayers = await this.teamPlayerRepo.GetTeamPlayersByGameId(game.Id);
                foreach (var teamPlayer in teamPlayers)
                {
                    if (teamPlayer.Team == 0)
                    {
                        var player = await this.playerRepo.GetPlayerById(teamPlayer.PlayerId);
                        if (player is not null)
                        {
                            gamePlayedDto.Team1.Players.Add(player);
                        }
                    }
                    else
                    {
                        var player = await this.playerRepo.GetPlayerById(teamPlayer.PlayerId);
                        if (player is not null)
                        {
                            gamePlayedDto.Team2.Players.Add(player);
                        }
                    }
                }

                gamePlayedDto.Highlights = game.Highlights;

                gamesPlayedDto.Add(gamePlayedDto);
            }

            return gamesPlayedDto;
        }

        /// <inheritdoc />
        public async Task<bool> Delete(int fifaGameId)
        {
            var gameInDb = await this.gamePlayedRepo.GetById(fifaGameId);

            if (gameInDb is null)
            {
                return false;
            }

            await this.highlightRepo.DeleteAllFifaGame(gameInDb.Id);
            await this.teamPlayerRepo.DeleteAllFifaGame(gameInDb.Id);
            await this.gamePlayedRepo.Delete(gameInDb.Id);

            return true;
        }
    }
}