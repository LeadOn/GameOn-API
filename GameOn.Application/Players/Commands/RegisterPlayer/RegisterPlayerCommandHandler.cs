// <copyright file="RegisterPlayerCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Players.Commands.RegisterPlayer
{
    using GameOn.Application.Common.Interfaces;
    using GameOn.Application.Players.Queries.GetPlayerByEmail;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// RegisterPlayerCommandHandler class.
    /// </summary>
    public class RegisterPlayerCommandHandler : IRequestHandler<RegisterPlayerCommand, Player>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterPlayerCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public RegisterPlayerCommandHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<Player> Handle(RegisterPlayerCommand request, CancellationToken cancellationToken)
        {
            var playerInDb = await this.mediator.Send(new GetPlayerByEmailQuery { Email = request.Player.Email });

            if (playerInDb is not null)
            {
                throw new NotImplementedException();
            }

            var newPlayer = new Player
            {
                Email = request.Player.Email,
                Nickname = request.Player.Nickname,
                PasswordSalt = request.Player.PasswordSalt,
                PasswordHash = request.Player.PasswordHash,
            };

            await this.context.Players.AddAsync(newPlayer, cancellationToken);
            await this.context.SaveChangesAsync(cancellationToken);
            return newPlayer;
        }
    }
}
