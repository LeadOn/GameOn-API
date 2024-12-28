// <copyright file="UpdatePlatformCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Platforms.Commands.UpdatePlatform
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// UpdatePlatformCommandHandler class.
    /// </summary>
    public class UpdatePlatformCommandHandler : IRequestHandler<UpdatePlatformCommand, Platform>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatePlatformCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public UpdatePlatformCommandHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<Platform> Handle(UpdatePlatformCommand request, CancellationToken cancellationToken)
        {
            var platformInDb = await this.context.Platforms.FirstOrDefaultAsync(x => x.Id == request.PlatformId, cancellationToken);

            if (platformInDb is null)
            {
                throw new NotImplementedException();
            }

            platformInDb.Name = request.Name;

            this.context.Platforms.Update(platformInDb);
            await this.context.SaveChangesAsync(cancellationToken);
            return platformInDb;
        }
    }
}
