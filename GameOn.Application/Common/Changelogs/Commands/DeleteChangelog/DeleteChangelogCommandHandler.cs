// <copyright file="DeleteChangelogCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Changelogs.Commands.DeleteChangelog
{
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// DeleteChangelogCommandHandler class.
    /// </summary>
    public class DeleteChangelogCommandHandler : IRequestHandler<DeleteChangelogCommand, bool>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteChangelogCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public DeleteChangelogCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(DeleteChangelogCommand request, CancellationToken cancellationToken)
        {
            var changelogInDb = await this.context.Changelogs.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (changelogInDb == null)
            {
                return false;
            }

            this.context.Changelogs.Remove(changelogInDb);
            await this.context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
