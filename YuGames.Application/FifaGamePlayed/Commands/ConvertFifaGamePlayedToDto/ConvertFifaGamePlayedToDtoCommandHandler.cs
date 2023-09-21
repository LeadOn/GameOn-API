// <copyright file="ConvertFifaGamePlayedToDtoCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.FifaGamePlayed.Commands.ConvertFifaGamePlayedToDto
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using YuGames.Application.Common.Interfaces;
    using YuGames.Common.DTOs;
    using YuGames.Domain;

    /// <summary>
    /// ConvertFifaGamePlayedToDtoCommandHandler class.
    /// </summary>
    public class ConvertFifaGamePlayedToDtoCommandHandler : IRequestHandler<ConvertFifaGamePlayedToDtoCommand, FifaGamePlayedDto>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertFifaGamePlayedToDtoCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public ConvertFifaGamePlayedToDtoCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<FifaGamePlayedDto> Handle(ConvertFifaGamePlayedToDtoCommand request, CancellationToken cancellationToken)
        {
            var gamePlayedDto = new FifaGamePlayedDto();
            gamePlayedDto.IsPlayed = request.FifaGamePlayed.IsPlayed;
            gamePlayedDto.TournamentId = request.FifaGamePlayed.TournamentId;
            gamePlayedDto.CreatedBy = request.FifaGamePlayed.CreatedBy;
            gamePlayedDto.Id = request.FifaGamePlayed.Id;
            gamePlayedDto.PlayedOn = request.FifaGamePlayed.PlayedOn;
            gamePlayedDto.Team1 = new FifaTeamDto
            {
                Id = 0,
                FifaTeamId = request.FifaGamePlayed.Team1Id,
                Code = request.FifaGamePlayed.TeamCode1 ?? "Unknown",
                Players = new List<Player>(),
                Score = request.FifaGamePlayed.TeamScore1,
            };
            gamePlayedDto.Team2 = new FifaTeamDto
            {
                Id = 1,
                FifaTeamId = request.FifaGamePlayed.Team2Id,
                Code = request.FifaGamePlayed.TeamCode2 ?? "Unknown",
                Players = new List<Player>(),
                Score = request.FifaGamePlayed.TeamScore2,
            };

            if (request.FifaGamePlayed.Platform is not null)
            {
                gamePlayedDto.Platform = game.Platform.Name;
                gamePlayedDto.PlatformId = game.PlatformId;
            }

            gamePlayedDto.Season = request.FifaGamePlayed.Season;

            // Getting team players
            var teamPlayers = await this.context.FifaTeamPlayers.Where(x => x.FifaGameId == gamePlayedDto.Id).ToListAsync(cancellationToken);
            foreach (var teamPlayer in teamPlayers)
            {
                if (teamPlayer.Team == 0)
                {
                    var player = await this.context.Platforms.FirstOrDefaultAsync(x => x.Id == teamPlayer.PlayerId, cancellationToken);
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
