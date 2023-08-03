// <copyright file="PlayerBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.Business
{
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

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerBusiness" /> class.
        /// </summary>
        /// <param name="playerRepo">Player repository, injected.</param>
        /// <param name="gamePlayedRepo">Game played repository, injected.</param>
        /// <param name="teamPlayerRepo">Team player repository, injected.</param>
        public PlayerBusiness(IPlayerRepository playerRepo, IGamePlayedRepository gamePlayedRepo, ITeamPlayerRepository teamPlayerRepo)
        {
            this.playerRepo = playerRepo;
            this.gamePlayedRepo = gamePlayedRepo;
            this.teamPlayerRepo = teamPlayerRepo;
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
    }
}