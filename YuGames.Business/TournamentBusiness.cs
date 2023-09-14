// <copyright file="TournamentBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Business
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using YuGames.Business.Contracts;
    using YuGames.DTOs;
    using YuGames.Entities;
    using YuGames.Repository.Contracts;

    /// <summary>
    /// Tournament business.
    /// </summary>
    public class TournamentBusiness : ITournamentBusiness
    {
        private ITournamentRepository tournamentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentBusiness" /> class.
        /// </summary>
        /// <param name="tournamentRepo">Tournament repository, injected.</param>
        public TournamentBusiness(ITournamentRepository tournamentRepo)
        {
            this.tournamentRepository = tournamentRepo;
        }

        /// <inheritdoc />
        public async Task<Tournament> Create(TournamentDto tournament)
        {
            return await this.tournamentRepository.Create(new Tournament
            {
                Description = tournament.Description,
                Name = tournament.Name,
                State = tournament.State,
                LogoUrl = tournament.LogoUrl,
                PlannedFrom = tournament.PlannedFrom,
                PlannedTo = tournament.PlannedTo,
            });
        }

        /// <inheritdoc />
        public async Task<List<Tournament>> GetAll()
        {
            return await this.tournamentRepository.GetAll();
        }

        /// <inheritdoc />
        public async Task<TournamentDto?> GetById(int id)
        {
            var tournamentInDb = await this.tournamentRepository.GetById(id);

            if (tournamentInDb is null)
            {
                return null;
            }
            else
            {
                var tournament = new TournamentDto(tournamentInDb);

                tournament.Players = await this.tournamentRepository.GetPlayers(id);

                return tournament;
            }
        }

        /// <inheritdoc />
        public async Task<Tournament> UpdateTournament(int id, TournamentDto tournament)
        {
            var tournamentInDb = await this.tournamentRepository.GetById(id);

            if (tournamentInDb is null)
            {
                throw new NotImplementedException();
            }

            tournamentInDb.Name = tournament.Name;
            tournamentInDb.Description = tournament.Description;
            tournamentInDb.PlannedFrom = tournament.PlannedFrom;
            tournamentInDb.PlannedTo = tournament.PlannedTo;
            tournamentInDb.LogoUrl = tournament.LogoUrl;
            tournamentInDb.State = tournament.State;

            return await this.tournamentRepository.UpdateTournament(tournamentInDb);
        }
    }
}