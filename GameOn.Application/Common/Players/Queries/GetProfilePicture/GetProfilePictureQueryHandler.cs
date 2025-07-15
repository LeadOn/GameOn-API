// <copyright file="GetProfilePictureQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Players.Queries.GetProfilePicture
{
    using GameOn.Application.Common.Players.Queries.GetPlayerById;
    using GameOn.Common.DTOs;
    using GameOn.Common.Exceptions;
    using GameOn.External.NetworkStorage.Interfaces;
    using MediatR;

    /// <summary>
    /// GetProfilePictureQueryHandler class.
    /// </summary>
    public class GetProfilePictureQueryHandler : IRequestHandler<GetProfilePictureQuery, ProfilePictureDto>
    {
        private readonly IMediator mediator;
        private readonly INetworkStorageService nsService;
        private readonly string bucketName = Environment.GetEnvironmentVariable("S3_BUCKET_NAME") ?? throw new MissingEnvironmentVariableException("S3_BUCKET_NAME");
        private readonly string ppBasePath = Environment.GetEnvironmentVariable("S3_PP_BASE_PATH") ?? throw new MissingEnvironmentVariableException("S3_PP_BASE_PATH");

        /// <summary>
        /// Initializes a new instance of the <see cref="GetProfilePictureQueryHandler"/> class.
        /// </summary>
        /// <param name="nsService">NetworkStorageService, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public GetProfilePictureQueryHandler(INetworkStorageService nsService, IMediator mediator)
        {
            this.nsService = nsService;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<ProfilePictureDto> Handle(GetProfilePictureQuery request, CancellationToken cancellationToken)
        {
            var ppDto = new ProfilePictureDto();

            // First, getting player
            var playerInDb = await this.mediator.Send(new GetPlayerByIdQuery { PlayerId = request.PlayerId }, cancellationToken);

            if (playerInDb is null)
            {
                return ppDto;
            }

            if (playerInDb.ProfilePictureUrl is null || playerInDb.ProfilePictureUrl == string.Empty)
            {
                ppDto.FileName = Environment.GetEnvironmentVariable("DEFAULT_PROFILE_PIC") ?? throw new MissingEnvironmentVariableException("DEFAULT_PROFILE_PIC");
            }
            else
            {
                ppDto.FileName = playerInDb.ProfilePictureUrl;
            }

            // Getting profile picture
            ppDto.FileStream = await this.nsService.GetFile(this.bucketName, this.ppBasePath + "/" + ppDto.FileName);
            return ppDto;
        }
    }
}
