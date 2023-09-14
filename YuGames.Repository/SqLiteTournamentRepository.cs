// <copyright file="SqLiteTournamentRepository.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Repository
{
    using Microsoft.EntityFrameworkCore;
    using YuGames.DTOs;
    using YuGames.Entities;
    using YuGames.EntitiesContext;
    using YuGames.Repository.Contracts;

    /// <summary>
    /// Tournament repository SQLite implementation.
    /// </summary>
    public class SqLiteTournamentRepository : ITournamentRepository
    {
        private YuGamesContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqLiteTournamentRepository"/> class.
        /// </summary>
        /// <param name="context">Database context, injected.</param>
        public SqLiteTournamentRepository(YuGamesContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Tournament> Create(Tournament tournament)
        {
            this.context.Tournaments.Add(tournament);
            await this.context.SaveChangesAsync();
            return tournament;
        }

        /// <inheritdoc />
        public async Task<List<Tournament>> GetAll()
        {
            return await this.context.Tournaments.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<int> Count()
        {
            return await this.context.Tournaments.CountAsync();
        }

        /// <inheritdoc />
        public async Task<Tournament?> GetById(int id)
        {
            return await this.context.Tournaments.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <inheritdoc />
        public async Task<List<TournamentPlayerDto>> GetPlayers(int tournamentId)
        {
            return await this.context.TournamentPlayers.Include(x => x.FifaTeam).Include(x => x.Player).Where(x => x.TournamentId == tournamentId).Select(x => new TournamentPlayerDto
            {
                JoinedAt = x.JoinedAt,
                Player = x.Player,
                Team = x.FifaTeam,
            }).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Tournament> UpdateTournament(Tournament tournament)
        {
            this.context.Tournaments.Update(tournament);
            await this.context.SaveChangesAsync();
            return tournament;
        }

        /// <inheritdoc />
        public async Task<bool> CheckPlayerSubscription(int tournamentId, int playerId)
        {
            var playerSubscription = await this.context.TournamentPlayers.FirstOrDefaultAsync(x => x.TournamentId == tournamentId && x.PlayerId == playerId);

            if (playerSubscription is not null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc />
        public async Task<TournamentPlayer> Subscribe(int tournamentId, int playerId, int fifaTeamId)
        {
            var tournamentPlayer = new TournamentPlayer
            {
                TournamentId = tournamentId,
                PlayerId = playerId,
                FifaTeamId = fifaTeamId,
            };

            this.context.TournamentPlayers.Add(tournamentPlayer);

            await this.context.SaveChangesAsync();

            return tournamentPlayer;
        }

        /// <inheritdoc />
        public async Task<bool> Delete(int tournamentId)
        {
            var tournament = await this.context.Tournaments.FirstOrDefaultAsync(x => x.Id == tournamentId);

            if (tournament is null)
            {
                return true;
            }
            else
            {
                this.context.Tournaments.Remove(tournament);
                await this.context.SaveChangesAsync();
                return true;
            }
        }
    }
}
