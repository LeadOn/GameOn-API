// <copyright file="GetTournamentLogoQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

using GameOn.Application.FIFA.Tournaments.Queries.GetTournamentById;

namespace GameOn.Application.FIFA.Tournaments.Queries.GetTournamentLogo
{
    using GameOn.Application.Common.Players.Queries.GetPlayerById;
    using GameOn.Common.DTOs;
    using GameOn.Common.Exceptions;
    using GameOn.External.NetworkStorage.Interfaces;
    using MediatR;

    /// <summary>
    /// GetTournamentLogoQueryHandler class.
    /// </summary>
    public class GetTournamentLogoQueryHandler : IRequestHandler<GetTournamentLogoQuery, TournamentLogoDto>
    {
        private readonly IMediator mediator;
        private readonly INetworkStorageService nsService;
        private readonly string bucketName = Environment.GetEnvironmentVariable("S3_BUCKET_NAME") ?? throw new MissingEnvironmentVariableException("S3_BUCKET_NAME");
        private readonly string tpBasePath = Environment.GetEnvironmentVariable("S3_TP_BASE_PATH") ?? throw new MissingEnvironmentVariableException("S3_TP_BASE_PATH");

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTournamentLogoQueryHandler"/> class.
        /// </summary>
        /// <param name="nsService">NetworkStorageService, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public GetTournamentLogoQueryHandler(INetworkStorageService nsService, IMediator mediator)
        {
            this.nsService = nsService;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<TournamentLogoDto> Handle(GetTournamentLogoQuery request, CancellationToken cancellationToken)
        {
            var tpDto = new TournamentLogoDto();

            // First, getting tournament
            var tournamentInDb = await this.mediator.Send(new GetTournamentByIdQuery { TournamentId = request.TournamentId }, cancellationToken);

            if (tournamentInDb is null)
            {
                return tpDto;
            }

            if (tournamentInDb.LogoUrl is null || tournamentInDb.LogoUrl == string.Empty)
            {
                tpDto.FileName = Environment.GetEnvironmentVariable("DEFAULT_PROFILE_PIC") ?? throw new MissingEnvironmentVariableException("DEFAULT_PROFILE_PIC");
            }
            else
            {
                tpDto.FileName = tournamentInDb.LogoUrl;
            }

            // Getting profile picture
            tpDto.FileStream = await this.nsService.GetFile(this.bucketName, this.tpBasePath + "/" + tpDto.FileName);
            return tpDto;
        }
    }
}
