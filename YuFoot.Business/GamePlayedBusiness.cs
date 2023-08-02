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

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePlayedBusiness" /> class.
        /// </summary>
        /// <param name="playerRepo">Player repository, injected.</param>
        public GamePlayedBusiness(IPlayerRepository playerRepo, IGamePlayedRepository gamePlayedRepo)
        {
            this.playerRepo = playerRepo;
            this.gamePlayedRepo = gamePlayedRepo;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GamePlayedDto>> GetLastFiveGames()
        {
            var gamePlayed = await this.gamePlayedRepo.Search(x => true, 5);

            var gamePlayedDtos = new List<GamePlayedDto>();

            foreach (var played in gamePlayed)
            {
                var gamePlayedDto = new GamePlayedDto();
                gamePlayedDto.GameDetails = played;
                gamePlayedDto.Players = played.TeamPlayers;

                foreach (var player in gamePlayedDto.Players)
                {
                    player.Player = await this.playerRepo.GetPlayerById(player.PlayerId) ?? null!;
                }

                gamePlayedDtos.Add(gamePlayedDto);
            }

            return gamePlayedDtos;
        }
    }
}