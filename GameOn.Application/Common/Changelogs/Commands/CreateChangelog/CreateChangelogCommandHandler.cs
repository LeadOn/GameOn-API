// <copyright file="CreateChangelogCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Changelogs.Commands.CreateChangelog
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// CreateChangelogCommandHandler class.
    /// </summary>
    public class CreateChangelogCommandHandler : IRequestHandler<CreateChangelogCommand, Changelog>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateChangelogCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public CreateChangelogCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Changelog> Handle(CreateChangelogCommand request, CancellationToken cancellationToken)
        {
            var newChangelog = new Changelog
            {
                Version = request.Changelog.Version,
                Type = request.Changelog.Type,
                UpdatedFeatures = request.Changelog.UpdatedFeatures,
                RemovedFeatures = request.Changelog.RemovedFeatures,
                NewFeatures = request.Changelog.NewFeatures,
                Context = request.Changelog.Context,
                PublicationDate = request.Changelog.PublicationDate,
            };

            this.context.Changelogs.Add(newChangelog);
            await this.context.SaveChangesAsync(cancellationToken);
            return newChangelog;
        }
    }
}
