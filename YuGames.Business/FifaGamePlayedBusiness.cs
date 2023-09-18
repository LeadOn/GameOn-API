// <copyright file="FifaGamePlayedBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Business
{
    using System;
    using YuGames.Business.Contracts;
    using YuGames.Common.Exceptions;
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
        public async Task<FifaGamePlayed?> Create(CreateFifaGameDto createGameDto)
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
                IsPlayed = true,
                SeasonId = int.Parse(Environment.GetEnvironmentVariable("CURRENT_SEASON") ?? throw new MissingEnvironmentVariableException("CURRENT_SEASON")),
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
            gameInDb.IsPlayed = fifaGame.IsPlayed;

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
        public async Task<FifaGamePlayedDto?> GetById(int gameId)
        {
            // First, getting game
            var gameInDb = await this.gamePlayedRepo.GetById(gameId);

            if (gameInDb is null)
            {
                return null;
            }

            return await this.Convert(gameInDb);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<FifaGamePlayedDto>> GetLastGamesPlayed(int number)
        {
            var gamesPlayedDto = new List<FifaGamePlayedDto>();

            // First, getting last games (from GamePlayed table)
            var gamesPlayed = await this.gamePlayedRepo.Search(x => true, number);

            // For each game played, getting player information
            foreach (var game in gamesPlayed)
            {
                gamesPlayedDto.Add(await this.Convert(game));
            }

            return gamesPlayedDto;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<FifaGamePlayedDto>> GetLastGamesPlayedByPlayerId(int playerId, int limit)
        {
            var gamesPlayedDto = new List<FifaGamePlayedDto>();

            // First, getting last games (from GamePlayed table)
            var gamesPlayed = await this.gamePlayedRepo.Search(x => x.TeamPlayers.FirstOrDefault(x => x.PlayerId == playerId) != null && x.IsPlayed == true, limit);

            // For each game played, getting player information
            foreach (var game in gamesPlayed)
            {
                gamesPlayedDto.Add(await this.Convert(game));
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

        /// <inheritdoc />
        public async Task<List<FifaGamePlayedDto>> Search(int? limit, int? platformId, DateTime? startDate, DateTime? endDate)
        {
            limit ??= 10;

            if (limit is not null && limit > 50)
            {
                limit = 50;
            }

            if (platformId is not null)
            {
                // checking if platform exists in database
                var platformInDb = await this.platformRepo.GetById((int)platformId);

                if (platformInDb is null)
                {
                    platformId = null;
                }
            }

            if (startDate is null)
            {
                startDate = DateTime.UtcNow.AddYears(-1);
            }

            if (endDate is null)
            {
                endDate = DateTime.UtcNow.AddDays(1);
            }

            IEnumerable<FifaGamePlayed> gamesInDb = new List<FifaGamePlayed>();

            if (platformId is null && limit is not null)
            {
                gamesInDb = await this.gamePlayedRepo.Search(x => x.PlayedOn >= startDate && x.PlayedOn <= endDate, (int)limit);
            }
            else if (platformId is not null && limit is not null)
            {
                gamesInDb = await this.gamePlayedRepo.Search(x => x.PlayedOn >= startDate && x.PlayedOn <= endDate && x.PlatformId == platformId, (int)limit);
            }

            var gamePlayedDtos = new List<FifaGamePlayedDto>();

            // For each game played, getting player information
            foreach (var game in gamesInDb)
            {
                gamePlayedDtos.Add(await this.Convert(game));
            }

            return gamePlayedDtos;
        }

        /// <inheritdoc />
        public async Task<Season?> GetCurrentSeason()
        {
            return await this.gamePlayedRepo.GetCurrentSeason();
        }

        /// <summary>
        /// Converts FifaGamePlayed from Database to FifaGamePlayedDto with embeded entities.
        /// </summary>
        /// <param name="game"><see cref="FifaGamePlayed"/>.</param>
        /// <returns><see cref="FifaGamePlayedDto"/>.</returns>
        private async Task<FifaGamePlayedDto> Convert(FifaGamePlayed game)
        {
            var gamePlayedDto = new FifaGamePlayedDto();
            gamePlayedDto.IsPlayed = game.IsPlayed;
            gamePlayedDto.CreatedBy = game.CreatedBy;
            gamePlayedDto.Id = game.Id;
            gamePlayedDto.PlayedOn = game.PlayedOn;
            gamePlayedDto.Team1 = new FifaTeamDto
            {
                Id = 0,
                FifaTeamId = game.Team1Id,
                Code = game.TeamCode1 ?? "Unknown",
                Players = new List<Player>(),
                Score = game.TeamScore1,
            };
            gamePlayedDto.Team2 = new FifaTeamDto
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
                gamePlayedDto.PlatformId = game.PlatformId;
            }

            gamePlayedDto.Season = game.Season;

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

            return gamePlayedDto;
        }
    }
}