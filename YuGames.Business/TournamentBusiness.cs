// <copyright file="TournamentBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Business
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using YuGames.Business.Contracts;
    using YuGames.Common.Collections;
    using YuGames.DTOs;
    using YuGames.Entities;
    using YuGames.Repository.Contracts;

    /// <summary>
    /// Tournament business.
    /// </summary>
    public class TournamentBusiness : ITournamentBusiness
    {
        private ITournamentRepository tournamentRepository;
        private IFifaTeamBusiness fifaTeamBusi;
        private IFifaGamePlayedRepository fifaGameRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentBusiness" /> class.
        /// </summary>
        /// <param name="tournamentRepo">Tournament repository, injected.</param>
        /// <param name="fifaTeamBusi">Fifa Team business, injected.</param>
        /// <param name="fifaGameRepo">Fifa Game repo, injected.</param>
        public TournamentBusiness(ITournamentRepository tournamentRepo, IFifaTeamBusiness fifaTeamBusi, IFifaGamePlayedRepository fifaGameRepo)
        {
            this.tournamentRepository = tournamentRepo;
            this.fifaTeamBusi = fifaTeamBusi;
            this.fifaGameRepo = fifaGameRepo;
        }

        /// <inheritdoc />
        public async Task<bool> CheckPlayerSubscription(int tournamentId, int playerId)
        {
            return await this.tournamentRepository.CheckPlayerSubscription(tournamentId, playerId);
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
        public async Task<bool> Delete(int tournamentId)
        {
            return await this.tournamentRepository.Delete(tournamentId);
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
        public async Task<bool> GoToPhase1(int tournamentId)
        {
            // Getting tournament
            var tournamentInDb = await this.tournamentRepository.GetById(tournamentId);

            if (tournamentInDb is null)
            {
                throw new NotImplementedException();
            }

            // Getting players that are registered
            var tournamentPlayers = await this.tournamentRepository.GetPlayers(tournamentId);

            foreach (var player in tournamentPlayers)
            {
                // Looping for every opponent
                foreach (var player2 in tournamentPlayers)
                {
                    if (player.Player.Id != player2.Player.Id)
                    {
                        var newGame = new FifaGamePlayed
                        {
                            CreatedById = player.Player.Id,
                            IsPlayed = false,
                            PlatformId = int.Parse(Environment.GetEnvironmentVariable("DEFAULT_PLATFORM") ?? "1"),
                            PlayedOn = DateTime.UtcNow,
                            SeasonId = int.Parse(Environment.GetEnvironmentVariable("CURRENT_SEASON") ?? "1"),
                            Team1Id = player.Team.Id,
                            Team2Id = player2.Team.Id,
                            TeamCode1 = "???",
                            TeamCode2 = "???",
                            TeamPlayers = new List<FifaTeamPlayer>
                                {
                                    new FifaTeamPlayer
                                    {
                                        PlayerId = player.Player.Id,
                                        Team = 0,
                                    },
                                    new FifaTeamPlayer
                                    {
                                        PlayerId = player2.Player.Id,
                                        Team = 1,
                                    },
                                },
                        };

                        await this.fifaGameRepo.Create(newGame);
                    }
                }
            }

            // Updating tournament to Phase 1
            tournamentInDb.State = TournamentStates.Phase1;
            await this.tournamentRepository.UpdateTournament(tournamentInDb);

            return true;
        }

        /// <inheritdoc />
        public async Task<TournamentPlayer> Subscribe(int tournamentId, int playerId, int fifaTeamId)
        {
            var isSubscribed = await this.CheckPlayerSubscription(tournamentId, playerId);

            if (isSubscribed == true)
            {
                throw new NotImplementedException();
            }

            var fifaTeamInDb = await this.fifaTeamBusi.GetById(fifaTeamId);

            if (fifaTeamInDb is null)
            {
                throw new NotImplementedException();
            }

            return await this.tournamentRepository.Subscribe(tournamentId, playerId, fifaTeamId);
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