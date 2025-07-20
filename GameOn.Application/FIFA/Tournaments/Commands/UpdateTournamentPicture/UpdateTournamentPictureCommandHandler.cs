// <copyright file="UpdateTournamentPictureCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.Tournaments.Commands.UpdateTournamentPicture
{
    using GameOn.Common.Exceptions;
    using GameOn.Common.Interfaces;
    using GameOn.External.NetworkStorage.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// UpdateTournamentPictureCommandHandler class.
    /// </summary>
    public class UpdateTournamentPictureCommandHandler : IRequestHandler<UpdateTournamentPictureCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IApplicationDbContext context;
        private readonly INetworkStorageService nsService;
        private readonly string bucketName = Environment.GetEnvironmentVariable("S3_BUCKET_NAME") ?? throw new MissingEnvironmentVariableException("S3_BUCKET_NAME");
        private readonly string tpBasePath = Environment.GetEnvironmentVariable("S3_TP_BASE_PATH") ?? throw new MissingEnvironmentVariableException("S3_TP_BASE_PATH");

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTournamentPictureCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="nsService">NetworkStorageService, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public UpdateTournamentPictureCommandHandler(IApplicationDbContext context, INetworkStorageService nsService, IMediator mediator)
        {
            this.context = context;
            this.nsService = nsService;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(UpdateTournamentPictureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await this.nsService.UploadFile(this.bucketName, this.tpBasePath + "/" + request.TournamentId + Path.GetExtension(request.File.FileName), request.File);

                // now that file is uploaded, updating user
                var tournamentInDb = await this.context.Tournaments.FirstOrDefaultAsync(x => x.Id == request.TournamentId, cancellationToken);

                if (tournamentInDb is not null)
                {
                    tournamentInDb.LogoUrl = request.TournamentId + Path.GetExtension(request.File.FileName);
                    this.context.Tournaments.Update(tournamentInDb);
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
