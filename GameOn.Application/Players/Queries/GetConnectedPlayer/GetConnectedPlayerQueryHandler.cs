// <copyright file="GetConnectedPlayerQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Players.Queries.GetConnectedPlayer
{
    using GameOn.Application.Common.Interfaces;
    using GameOn.Application.Players.Queries.GetPlayerById;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetConnectedPlayerQueryHandler class.
    /// </summary>
    public class GetConnectedPlayerQueryHandler : IRequestHandler<GetConnectedPlayerQuery, Player?>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetConnectedPlayerQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">MediatR, injected.</param>
        public GetConnectedPlayerQueryHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<Player?> Handle(GetConnectedPlayerQuery request, CancellationToken cancellationToken)
        {
            var userInDb = await this.context.Players.FirstOrDefaultAsync(x => x.KeycloakId == request.ConnectedPlayer.KeycloakId);

            if (userInDb is null)
            {
                var fullName = request.ConnectedPlayer.Email;

                if (request.ConnectedPlayer.FirstName is not null)
                {
                    fullName = request.ConnectedPlayer.FirstName;
                    if (request.ConnectedPlayer.LastName is not null)
                    {
                        fullName += " " + request.ConnectedPlayer.LastName;
                    }
                }

                var nickname = fullName;

                if (request.ConnectedPlayer.PreferredUsername is not null)
                {
                    nickname = request.ConnectedPlayer.PreferredUsername;
                }

                // Creating user
                var user = new Player
                {
                    KeycloakId = request.ConnectedPlayer.KeycloakId,
                    CreatedOn = DateTime.UtcNow,
                    FullName = fullName,
                    Nickname = nickname,
                };

                this.context.Players.Add(user);
                await this.context.SaveChangesAsync(cancellationToken);
            }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return await this.mediator.Send(new GetPlayerByIdQuery { PlayerId = userInDb.Id }, cancellationToken);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}
