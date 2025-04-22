// <copyright file="UpdateChangelogCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace GameOn.Application.Common.Changelogs.Commands.UpdateChangelog
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// UpdateChangelogCommandHandler class.
    /// </summary>
    public class UpdateChangelogCommandHandler : IRequestHandler<UpdateChangelogCommand, Changelog>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateChangelogCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public UpdateChangelogCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Changelog> Handle(UpdateChangelogCommand request, CancellationToken cancellationToken)
        {
            var changelogInDb = await this.context.Changelogs.FirstOrDefaultAsync(x => x.Id == request.Changelog.Id, cancellationToken);

            if (changelogInDb == null)
            {
                throw new NotImplementedException();
            }

            changelogInDb.Published = request.Changelog.Published;
            changelogInDb.Name = request.Changelog.Name;
            changelogInDb.Context = request.Changelog.Context;
            changelogInDb.Version = request.Changelog.Version;
            changelogInDb.Type = request.Changelog.Type;
            changelogInDb.NewFeatures = request.Changelog.NewFeatures;
            changelogInDb.RemovedFeatures = request.Changelog.RemovedFeatures;
            changelogInDb.UpdatedFeatures = request.Changelog.UpdatedFeatures;

            this.context.Changelogs.Update(changelogInDb);
            await this.context.SaveChangesAsync(cancellationToken);
            return request.Changelog;
        }
    }
}
