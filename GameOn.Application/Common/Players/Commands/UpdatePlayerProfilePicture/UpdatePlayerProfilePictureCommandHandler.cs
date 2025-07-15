// <copyright file="UpdatePlayerProfilePictureCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Players.Commands.UpdatePlayerProfilePicture
{
    using GameOn.Application.Common.Players.Queries.GetPlayerById;
    using GameOn.Common.Exceptions;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.NetworkStorage.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// UpdatePlayerProfilePictureCommandHandler class.
    /// </summary>
    public class UpdatePlayerProfilePictureCommandHandler : IRequestHandler<UpdatePlayerProfilePictureCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IApplicationDbContext context;
        private readonly INetworkStorageService nsService;
        private readonly string bucketName = Environment.GetEnvironmentVariable("S3_BUCKET_NAME") ?? throw new MissingEnvironmentVariableException("S3_BUCKET_NAME");
        private readonly string ppBasePath = Environment.GetEnvironmentVariable("S3_PP_BASE_PATH") ?? throw new MissingEnvironmentVariableException("S3_PP_BASE_PATH");

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatePlayerProfilePictureCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="nsService">NetworkStorageService, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public UpdatePlayerProfilePictureCommandHandler(IApplicationDbContext context, INetworkStorageService nsService, IMediator mediator)
        {
            this.context = context;
            this.nsService = nsService;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(UpdatePlayerProfilePictureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await this.nsService.UploadFile(this.bucketName, this.ppBasePath + "/" + request.PlayerId + Path.GetExtension(request.File.FileName), request.File);

                // now that file is uploaded, updating user
                var playerInDb = await this.mediator.Send(new GetPlayerByIdQuery { PlayerId = request.PlayerId }, cancellationToken);

                if (playerInDb is not null)
                {
                    playerInDb.ProfilePictureUrl = request.PlayerId + Path.GetExtension(request.File.FileName);
                    this.context.Players.Update(playerInDb);
                    await this.context.SaveChangesAsync(cancellationToken);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
