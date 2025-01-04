// <copyright file="CreatePlatformCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Platforms.Commands.CreatePlatform
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// CreatePlatformCommandHandler class.
    /// </summary>
    public class CreatePlatformCommandHandler : IRequestHandler<CreatePlatformCommand, Platform>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreatePlatformCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public CreatePlatformCommandHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<Platform> Handle(CreatePlatformCommand request, CancellationToken cancellationToken)
        {
            var platform = new Platform
            {
                Name = request.Name,
            };

            context.Platforms.Add(platform);
            await context.SaveChangesAsync(cancellationToken);
            return platform;
        }
    }
}
