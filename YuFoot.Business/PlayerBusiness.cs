// <copyright file="PlayerBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.Business
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using YuFoot.Business.Contracts;
    using YuFoot.DTOs;
    using YuFoot.Entities;
    using YuFoot.Repository.Contracts;

    /// <summary>
    /// Player business.
    /// </summary>
    public class PlayerBusiness : IPlayerBusiness
    {
        private IPlayerRepository playerRepo;
        private IGamePlayedRepository gamePlayedRepo;
        private ITeamPlayerRepository teamPlayerRepo;
        private IPlatformRepository platformRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerBusiness" /> class.
        /// </summary>
        /// <param name="playerRepo">Player repository, injected.</param>
        /// <param name="gamePlayedRepo">Game played repository, injected.</param>
        /// <param name="teamPlayerRepo">Team player repository, injected.</param>
        /// <param name="platformRepo">Platform repository, injected.</param>
        public PlayerBusiness(IPlayerRepository playerRepo, IGamePlayedRepository gamePlayedRepo, ITeamPlayerRepository teamPlayerRepo, IPlatformRepository platformRepo)
        {
            this.playerRepo = playerRepo;
            this.gamePlayedRepo = gamePlayedRepo;
            this.teamPlayerRepo = teamPlayerRepo;
            this.platformRepo = platformRepo;
        }

        /// <inheritdoc />
        public async Task<PlayerDto?> GetPlayerById(int id)
        {
            var player = await this.playerRepo.GetPlayerById(id);

            if (player == null)
            {
                return null;
            }
            else
            {
                var playerDto = new PlayerDto
                {
                    CreatedOn = player.CreatedOn,
                    Draws = 0,
                    FullName = player.FullName,
                    Id = player.Id,
                    Losses = 0,
                    MatchPlayed = 0,
                    Nickname = player.Nickname,
                    ProfilePictureUrl = player.ProfilePictureUrl,
                    Wins = 0,
                    TotalGoals = 0,
                };

                // Getting TeamPlayer entries
                var teamPlayers = await this.teamPlayerRepo.GetTeamPlayerByPlayerId(player.Id);
                var goalsTaken = 0;

                foreach (var teamPlayer in teamPlayers)
                {
                    // Getting associated match
                    var gamePlayed = await this.gamePlayedRepo.GetById(teamPlayer.GamePlayedId);

                    if (gamePlayed is not null)
                    {
                        if ((teamPlayer.Team == 0 && gamePlayed.TeamScore1 < gamePlayed.TeamScore2) || (teamPlayer.Team == 1 && gamePlayed.TeamScore1 > gamePlayed.TeamScore2))
                        {
                            playerDto.Losses++;
                        }
                        else if ((teamPlayer.Team == 0 && gamePlayed.TeamScore1 > gamePlayed.TeamScore2) || (teamPlayer.Team == 1 && gamePlayed.TeamScore1 < gamePlayed.TeamScore2))
                        {
                            playerDto.Wins++;
                        }
                        else
                        {
                            playerDto.Draws++;
                        }

                        if (teamPlayer.Team == 0 && gamePlayed.PlatformId != 3)
                        {
                            playerDto.TotalGoals += gamePlayed.TeamScore1;
                            goalsTaken += gamePlayed.TeamScore2;
                        }
                        else if (teamPlayer.Team == 1 && gamePlayed.PlatformId != 3)
                        {
                            playerDto.TotalGoals += gamePlayed.TeamScore2;
                            goalsTaken += gamePlayed.TeamScore1;
                        }
                    }
                }

                playerDto.MatchPlayed = playerDto.Wins + playerDto.Draws + playerDto.Losses;
                playerDto.TotalGoalDifference = playerDto.TotalGoals - goalsTaken;

                return playerDto;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Player>> GetAll()
        {
            return await this.playerRepo.GetAll();
        }

        /// <inheritdoc />
        public async Task<PlayerDto> GetConnectedUser(ConnectedPlayerDto player)
        {
            var userInDb = await this.playerRepo.GetPlayerByKeycloakId(player);
#pragma warning disable CS8603 // Possible null reference return.
            return await this.GetPlayerById(userInDb.Id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <inheritdoc />
        public async Task<PlayerDto> UpdatePlayer(string fullName, string nickname, string profilePictureUrl, string keycloakId)
        {
            // Now that we have user, update it
            var updatedUser = await this.playerRepo.UpdateUser(keycloakId, fullName, nickname, profilePictureUrl);

#pragma warning disable CS8603 // Possible null reference return.
            return await this.GetPlayerById(updatedUser.Id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <inheritdoc />
        public async Task<List<PlatformStatsDto>?> GetPlayerStats(int playerId)
        {
            // Get player from database.
            var playerInDb = await this.playerRepo.GetPlayerById(playerId);

            if (playerInDb is null)
            {
                return null;
            }

            var platformsStats = new List<PlatformStatsDto>();

            var platformsInDb = await this.platformRepo.GetAll();

            foreach (var platform in platformsInDb)
            {
                var stats = new PlatformStatsDto { AverageGoalGiven = 0, AverageGoalTaken = 0, Draws = 0, GoalDifference = 0, Losses = 0, Platform = platform, Wins = 0, GoalsGiven = 0, GoalsTaken = 0 };

                // Getting games played by platform
                var teamPlayersInDb = await this.teamPlayerRepo.Search(x => x.GamePlayed.PlatformId == platform.Id && x.PlayerId == playerInDb.Id, 1000000);

                // For each game played, getting that stats
                foreach (var teamPlayer in teamPlayersInDb)
                {
                    // Getting game
                    var gameInDb = await this.gamePlayedRepo.GetById(teamPlayer.GamePlayedId);

                    if (gameInDb is null)
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        if (gameInDb.TeamScore1 == gameInDb.TeamScore2)
                        {
                            stats.Draws++;
                        }
                        else if ((teamPlayer.Team == 0 && gameInDb.TeamScore1 > gameInDb.TeamScore2) || (teamPlayer.Team == 1 && gameInDb.TeamScore1 < gameInDb.TeamScore2))
                        {
                            stats.Wins++;
                        }
                        else if ((teamPlayer.Team == 0 && gameInDb.TeamScore1 < gameInDb.TeamScore2) || (teamPlayer.Team == 1 && gameInDb.TeamScore1 > gameInDb.TeamScore2))
                        {
                            stats.Losses++;
                        }

                        if (teamPlayer.Team == 0)
                        {
                            stats.GoalsGiven += gameInDb.TeamScore1;
                            stats.GoalsTaken += gameInDb.TeamScore2;
                        }
                        else if (teamPlayer.Team == 1)
                        {
                            stats.GoalsGiven += gameInDb.TeamScore2;
                            stats.GoalsTaken += gameInDb.TeamScore1;
                        }
                    }
                }

                stats.GoalDifference = stats.GoalsGiven - stats.GoalsTaken;
                if (stats.GoalsGiven == 0 || (stats.Wins + stats.Losses + stats.Draws) == 0)
                {
                    stats.AverageGoalGiven = 0;
                }
                else
                {
                    stats.AverageGoalGiven = (float)Math.Round((double)(stats.GoalsGiven / (float)(stats.Wins + stats.Draws + stats.Losses)), 2);
                }

                if (stats.GoalsTaken == 0 || (stats.Wins + stats.Losses + stats.Draws) == 0)
                {
                    stats.AverageGoalTaken = 0;
                }
                else
                {
                    stats.AverageGoalTaken = (float)Math.Round((double)(stats.GoalsTaken / (float)(stats.Wins + stats.Draws + stats.Losses)), 2);
                }

                platformsStats.Add(stats);
            }

            return platformsStats;
        }
    }
}