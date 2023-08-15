﻿// <copyright file="GamePlayedBusiness.cs" company="LeadOn's Corp'">
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
    public class GamePlayedBusiness : IGamePlayedBusiness
    {
        private IPlayerRepository playerRepo;
        private IGamePlayedRepository gamePlayedRepo;
        private ITeamPlayerRepository teamPlayerRepo;
        private IPlatformRepository platformRepo;
        private IFifaTeamRepository fifaTeamRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePlayedBusiness" /> class.
        /// </summary>
        /// <param name="playerRepo">Player repository, injected.</param>
        /// <param name="gamePlayedRepo">GamePlayed repository, injected.</param>
        /// <param name="teamPlayerRepo">TeamPlayer repository, injected.</param>
        /// <param name="platformRepo">Platform repository, injected.</param>
        /// <param name="fifaTeamRepo">FifaTeam repository, injected.</param>
        public GamePlayedBusiness(IPlayerRepository playerRepo, IGamePlayedRepository gamePlayedRepo, ITeamPlayerRepository teamPlayerRepo, IPlatformRepository platformRepo, IFifaTeamRepository fifaTeamRepo)
        {
            this.playerRepo = playerRepo;
            this.gamePlayedRepo = gamePlayedRepo;
            this.teamPlayerRepo = teamPlayerRepo;
            this.platformRepo = platformRepo;
            this.fifaTeamRepo = fifaTeamRepo;
        }

        /// <inheritdoc />
        public async Task<GamePlayed?> CreateGame(CreateGameDto createGameDto)
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

            var newGame = new GamePlayed
            {
                PlatformId = platformInDb.Id,
                PlayedOn = createGameDto.CreatedOn,
                TeamCode1 = createGameDto.TeamCode1,
                TeamCode2 = createGameDto.TeamCode2,
                TeamScore1 = createGameDto.TeamScore1,
                TeamScore2 = createGameDto.TeamScore2,
                CreatedById = creatorInDb.Id,
            };

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
            var gameInDb = await this.gamePlayedRepo.CreateGame(newGame);

            if (gameInDb is null)
            {
                return null;
            }
            else
            {
                // Now creating team players
                foreach (var playerId in createGameDto.Team1)
                {
                    var teamPlayerInDb = await this.teamPlayerRepo.CreateTeamPlayer(new TeamPlayer
                    {
                        GamePlayedId = gameInDb.Id,
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
                    var teamPlayerInDb = await this.teamPlayerRepo.CreateTeamPlayer(new TeamPlayer
                    {
                        GamePlayedId = gameInDb.Id,
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
    }
}