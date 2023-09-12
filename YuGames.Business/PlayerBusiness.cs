// <copyright file="PlayerBusiness.cs" company="LeadOn's Corp'">
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
    public class PlayerBusiness : IPlayerBusiness
    {
        private IPlayerRepository playerRepo;
        private IFifaGamePlayedRepository gamePlayedRepo;
        private ITeamPlayerRepository teamPlayerRepo;
        private IPlatformRepository platformRepo;
        private IFifaTeamBusiness fifaTeamBusiness;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerBusiness" /> class.
        /// </summary>
        /// <param name="playerRepo">Player repository, injected.</param>
        /// <param name="gamePlayedRepo">Game played repository, injected.</param>
        /// <param name="teamPlayerRepo">Team player repository, injected.</param>
        /// <param name="platformRepo">Platform repository, injected.</param>
        /// <param name="fifaTeamBusiness">FifaTeamBusiness, injected.</param>
        public PlayerBusiness(IPlayerRepository playerRepo, IFifaGamePlayedRepository gamePlayedRepo, ITeamPlayerRepository teamPlayerRepo, IPlatformRepository platformRepo, IFifaTeamBusiness fifaTeamBusiness)
        {
            this.playerRepo = playerRepo;
            this.gamePlayedRepo = gamePlayedRepo;
            this.teamPlayerRepo = teamPlayerRepo;
            this.platformRepo = platformRepo;
            this.fifaTeamBusiness = fifaTeamBusiness;
        }

        /// <inheritdoc />
        public async Task<Player?> GetPlayerById(int id)
        {
            return await this.playerRepo.GetPlayerById(id);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Player>> GetAll()
        {
            return await this.playerRepo.GetAll();
        }

        /// <inheritdoc />
        public async Task<Player> GetConnectedUser(ConnectedPlayerDto player)
        {
            var userInDb = await this.playerRepo.GetPlayerByKeycloakId(player);
#pragma warning disable CS8603 // Possible null reference return.
            return await this.GetPlayerById(userInDb.Id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <inheritdoc />
        public async Task<Player> UpdatePlayer(string fullName, string nickname, string profilePictureUrl, string keycloakId)
        {
            // Now that we have user, update it
            var updatedUser = await this.playerRepo.UpdateUser(keycloakId, fullName, nickname, profilePictureUrl);

#pragma warning disable CS8603 // Possible null reference return.
            return await this.GetPlayerById(updatedUser.Id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <inheritdoc />
        public async Task<FifaPlayerStatsDto> GetPlayerStats(int playerId)
        {
            // Get player from database.
            var playerInDb = await this.playerRepo.GetPlayerById(playerId);

            if (playerInDb is null)
            {
                return new FifaPlayerStatsDto();
            }

            // Getting stats per platforms
            var platformsStats = new List<PlatformStatsDto>();

            var platformsInDb = await this.platformRepo.GetAll();

            var globalStats = new PlatformStatsDto();
            globalStats.Platform = new Platform { Id = 0, Name = "Global" };
            var avgGoalsTaken = new List<float>();
            var avgGoalsGiven = new List<float>();

            foreach (var platform in platformsInDb)
            {
                var stats = new PlatformStatsDto { AverageGoalGiven = 0, AverageGoalTaken = 0, Draws = 0, GoalDifference = 0, Losses = 0, Platform = platform, Wins = 0, GoalsGiven = 0, GoalsTaken = 0 };

                // Getting games played by platform
                var teamPlayersInDb = await this.teamPlayerRepo.Search(x => x.FifaGamePlayed.PlatformId == platform.Id && x.PlayerId == playerInDb.Id && x.FifaGamePlayed.SeasonId == int.Parse(Environment.GetEnvironmentVariable("CURRENT_SEASON") ?? "1"), 1000000);

                // For each game played, getting that stats
                foreach (var teamPlayer in teamPlayersInDb)
                {
                    // Getting game
                    var gameInDb = await this.gamePlayedRepo.GetById(teamPlayer.FifaGameId);

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

                var gamesPlayed = stats.Wins + stats.Losses + stats.Draws;

                // Calculating win rate
                if (stats.Wins == 0 || gamesPlayed == 0)
                {
                    stats.WinRate = 0;
                }
                else
                {
                    stats.WinRate = (float)Math.Round((double)((stats.Wins * 100) / (float)gamesPlayed), 2);
                }

                // Calculating loose rate
                if (stats.Losses == 0 || gamesPlayed == 0)
                {
                    stats.LooseRate = 0;
                }
                else
                {
                    stats.LooseRate = (float)Math.Round((double)((stats.Losses * 100) / (float)gamesPlayed), 2);
                }

                // Calculating draw rate
                if (stats.Draws == 0 || gamesPlayed == 0)
                {
                    stats.DrawRate = 0;
                }
                else
                {
                    stats.DrawRate = (float)Math.Round((double)((stats.Draws * 100) / (float)gamesPlayed), 2);
                }

                globalStats.Wins += stats.Wins;
                globalStats.Losses += stats.Losses;
                globalStats.Draws += stats.Draws;
                globalStats.GoalDifference += stats.GoalDifference;
                globalStats.GoalsTaken += stats.GoalsTaken;
                globalStats.GoalsGiven += stats.GoalsGiven;
                avgGoalsGiven.Add(stats.AverageGoalGiven);
                avgGoalsTaken.Add(stats.AverageGoalTaken);

                platformsStats.Add(stats);
            }

            globalStats.AverageGoalGiven = (float)Math.Round((double)avgGoalsGiven.Average(), 2);
            globalStats.AverageGoalTaken = (float)Math.Round((double)avgGoalsTaken.Average(), 2);

            var totalGamesPlayed = globalStats.Wins + globalStats.Losses + globalStats.Draws;

            // Calculating win rate
            if (globalStats.Wins == 0 || totalGamesPlayed == 0)
            {
                globalStats.WinRate = 0;
            }
            else
            {
                globalStats.WinRate = (float)Math.Round((double)((globalStats.Wins * 100) / (float)totalGamesPlayed), 2);
            }

            // Calculating loose rate
            if (globalStats.Losses == 0 || totalGamesPlayed == 0)
            {
                globalStats.LooseRate = 0;
            }
            else
            {
                globalStats.LooseRate = (float)Math.Round((double)((globalStats.Losses * 100) / (float)totalGamesPlayed), 2);
            }

            // Calculating draw rate
            if (globalStats.Draws == 0 || totalGamesPlayed == 0)
            {
                globalStats.DrawRate = 0;
            }
            else
            {
                globalStats.DrawRate = (float)Math.Round((double)((globalStats.Draws * 100) / (float)totalGamesPlayed), 2);
            }

            platformsStats.Add(globalStats);

            return new FifaPlayerStatsDto
            {
                StatsPerPlatform = platformsStats.OrderBy(x => x.Platform.Id).ToList(),
                MostWinsTeams = await this.fifaTeamBusiness.GetMostWinsTeams(playerId, 3),
                MostLossesTeams = await this.fifaTeamBusiness.GetMostLossesTeams(playerId, 3),
                MostPlayedTeams = await this.fifaTeamBusiness.GetMostPlayedTeams(playerId, 3),
            };
        }
    }
}