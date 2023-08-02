// <copyright file="GamePlayedBusiness.cs" company="LeadOn's Corp'">
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
    public class GamePlayedBusiness : IGamePlayedBusiness
    {
        private IPlayerRepository playerRepo;
        private IGamePlayedRepository gamePlayedRepo;
        private ITeamPlayerRepository teamPlayerRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePlayedBusiness" /> class.
        /// </summary>
        /// <param name="playerRepo">Player repository, injected.</param>
        /// <param name="gamePlayedRepo">GamePlayed repository, injected.</param>
        /// <param name="teamPlayerRepo">TeamPlayer repository, injected.</param>
        public GamePlayedBusiness(IPlayerRepository playerRepo, IGamePlayedRepository gamePlayedRepo, ITeamPlayerRepository teamPlayerRepo)
        {
            this.playerRepo = playerRepo;
            this.gamePlayedRepo = gamePlayedRepo;
            this.teamPlayerRepo = teamPlayerRepo;
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
                gamePlayedDto.Id = game.Id;
                gamePlayedDto.PlayedOn = game.PlayedOn;
                gamePlayedDto.Team1 = new TeamDto
                {
                    Id = 0,
                    Code = game.TeamCode1 ?? "Unknown",
                    Players = new List<Player>(),
                    Score = game.TeamScore1,
                };
                gamePlayedDto.Team2 = new TeamDto
                {
                    Id = 1,
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

                gamesPlayedDto.Add(gamePlayedDto);
            }

            return gamesPlayedDto;
        }
    }
}