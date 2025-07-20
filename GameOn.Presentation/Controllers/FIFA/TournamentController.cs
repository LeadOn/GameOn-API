﻿// <copyright file="TournamentController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers.FIFA
{
    using GameOn.Application.Common.Players.Queries.GetConnectedPlayer;
    using GameOn.Application.FIFA.TournamentPlayers.Commands.RemoveTournamentSubscription;
    using GameOn.Application.FIFA.TournamentPlayers.Commands.UpdateTournamentSubscription;
    using GameOn.Application.FIFA.Tournaments.Commands.CreateTournament;
    using GameOn.Application.FIFA.Tournaments.Commands.DeleteTournament;
    using GameOn.Application.FIFA.Tournaments.Commands.GoToPhase1;
    using GameOn.Application.FIFA.Tournaments.Commands.SavePhase1Score;
    using GameOn.Application.FIFA.Tournaments.Commands.SubscribeTournament;
    using GameOn.Application.FIFA.Tournaments.Commands.UpdateTournament;
    using GameOn.Application.FIFA.Tournaments.Commands.UpdateTournamentPicture;
    using GameOn.Application.FIFA.Tournaments.Queries.CheckTournamentSubscription;
    using GameOn.Application.FIFA.Tournaments.Queries.GetAllTournaments;
    using GameOn.Application.FIFA.Tournaments.Queries.GetFeaturedTournaments;
    using GameOn.Application.FIFA.Tournaments.Queries.GetTournamentById;
    using GameOn.Application.FIFA.Tournaments.Queries.GetTournamentLogo;
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using GameOn.External.NetworkStorage.Interfaces;
    using GameOn.Presentation.Classes;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Tournament Controller.
    /// </summary>
    [ApiController]
    [Route("fifa/[controller]")]
    public class TournamentController : ControllerBase
    {
        private readonly ISender mediator;
        private readonly INetworkStorageService nsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        /// <param name="nsService">NetworkStorage interface, injected.</param>
        public TournamentController(ISender mediator, INetworkStorageService nsService)
        {
            this.mediator = mediator;
            this.nsService = nsService;
        }

        /// <summary>
        /// Get all tournaments in database.
        /// </summary>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all tournaments in database.")]
        [SwaggerResponse(200, "Tournaments.", typeof(List<Tournament>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetAll()
        {
            return this.Ok(await this.mediator.Send(new GetAllTournamentsQuery()));
        }

        /// <summary>
        /// Get all featured tournaments in database.
        /// </summary>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Route("featured")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all featured tournaments in database.")]
        [SwaggerResponse(200, "Featured Tournaments.", typeof(List<TournamentDto>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetFeatured()
        {
            return this.Ok(await this.mediator.Send(new GetFeaturedTournamentsQuery()));
        }

        /// <summary>
        /// Get tournament by ID in database.
        /// </summary>
        /// <param name="id">Tournament ID.</param>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Route("{id:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get tournament by ID in database.")]
        [SwaggerResponse(200, "Tournament.", typeof(TournamentDto))]
        [SwaggerResponse(404, "Tournament not found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetById(int id)
        {
            return this.Ok(await this.mediator.Send(new GetTournamentByIdQuery { TournamentId = id }));
        }

        /// <summary>
        /// Check player subscription to a tournament in database.
        /// </summary>
        /// <param name="id">Tournament ID.</param>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Authorize]
        [Route("{id:int}/subscription")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Check if player is subscribed to a tournament.")]
        [SwaggerResponse(200, "Player is subscribed.", typeof(TournamentPlayer))]
        [SwaggerResponse(404, "Player not subscribed / tournament not found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> CheckSubscription(int id)
        {
            var playerSubscription = await this.mediator.Send(new CheckTournamentSubscriptionQuery { TournamentId = id, ConnectedPlayer = this.User.GetConnectedPlayer() });

            if (playerSubscription is not null)
            {
                return this.Ok(playerSubscription);
            }

            return this.NotFound();
        }

        /// <summary>
        /// Create tournament in database.
        /// </summary>
        /// <param name="tournament">Tournament DTO.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPost]
        [Authorize(Roles = "gameon_admin")]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Create tournament in database.")]
        [SwaggerResponse(201, "Created tournament.", typeof(Tournament))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(403, "Not enough roles.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> Create(TournamentDto tournament)
        {
            var newTournament = new Tournament
            {
                Description = tournament.Description,
                Name = tournament.Name,
                State = tournament.State,
                LogoUrl = tournament.LogoUrl,
                PlannedFrom = tournament.PlannedFrom,
                PlannedTo = tournament.PlannedTo,
            };

            return this.StatusCode(201, await this.mediator.Send(new CreateTournamentCommand { Tournament = newTournament }));
        }

        /// <summary>
        /// Update tournament in database.
        /// </summary>
        /// <param name="id">Tournament ID.</param>
        /// <param name="tournament">Updated tournament.</param>
        /// <returns>IActionResult Object.</returns>
        [HttpPatch]
        [Authorize(Roles = "gameon_admin")]
        [Route("{id:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Create tournament in database.")]
        [SwaggerResponse(201, "Created tournament.", typeof(Tournament))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(403, "Not enough roles.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> Update(int id, [FromBody] TournamentDto tournament)
        {
            return this.Ok(await this.mediator.Send(new UpdateTournamentCommand { TournamentId = id, TournamentDto = tournament }));
        }

        /// <summary>
        /// Update tournament to phase 1.
        /// </summary>
        /// <param name="id">Tournament ID.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPost]
        [Authorize(Roles = "gameon_admin")]
        [Route("{id:int}/phase1")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Make Tournament to go into phase 1.")]
        [SwaggerResponse(204, "Updated tournament.", typeof(Tournament))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(403, "Not enough roles.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GoToPhase1(int id)
        {
            var result = await this.mediator.Send(new GoToPhase1Command { TournamentId = id });

            if (result)
            {
                return this.NoContent();
            }
            else
            {
                return this.Problem();
            }
        }

        /// <summary>
        /// Remove player from tournament.
        /// </summary>
        /// <param name="tournamentId">Tournament ID.</param>
        /// <param name="playerId">Player ID.</param>
        /// <returns>IActionResult object.</returns>
        [HttpDelete]
        [Authorize(Roles = "gameon_admin")]
        [Route("{tournamentId:int}/player/{playerId:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Remove player from tournament.")]
        [SwaggerResponse(204, "Removed player.")]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(403, "Not enough roles.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> RemovePlayer(int tournamentId, int playerId)
        {
            var connectedPlayer = this.User.GetConnectedPlayer();

            var result = await this.mediator.Send(new RemoveTournamentSubscriptionCommand { TournamentId = tournamentId, Player = connectedPlayer, PlayerId = playerId });

            if (result)
            {
                return this.NoContent();
            }
            else
            {
                return this.Problem();
            }
        }

        /// <summary>
        /// Save phase 1 score.
        /// </summary>
        /// <param name="id">Tournament ID.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPost]
        [Authorize(Roles = "gameon_admin")]
        [Route("{id:int}/phase1/score")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Save phase 1 score.")]
        [SwaggerResponse(204, "Update successfully.")]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(403, "Not enough roles.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> SavePhase1Score(int id)
        {
            var result = await this.mediator.Send(new SavePhase1ScoreCommand() { TournamentId = id });

            if (result)
            {
                return this.NoContent();
            }
            else
            {
                return this.Problem();
            }
        }

        /// <summary>
        /// Subscribe to tournament.
        /// </summary>
        /// <param name="tournamentId">Tournament ID.</param>
        /// <param name="fifaTeamId">FIFA Team ID.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPost]
        [Authorize]
        [Route("{tournamentId:int}/subscription")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Subscribe to tournament.")]
        [SwaggerResponse(204, "Player is subscribed.")]
        [SwaggerResponse(404, "Player not subscribed / tournament not found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> Subscribe(int tournamentId, int fifaTeamId)
        {
            await this.mediator.Send(new GetConnectedPlayerQuery { ConnectedPlayer = this.User.GetConnectedPlayer() });

            return this.Ok(await this.mediator.Send(new SubscribeTournamentCommand { FifaTeamId = fifaTeamId, TournamentId = tournamentId, Player = this.User.GetConnectedPlayer() }));
        }

        /// <summary>
        /// Update tournament subscription.
        /// </summary>
        /// <param name="tournamentId">Tournament ID.</param>
        /// <param name="fifaTeamId">FIFA Team ID.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPatch]
        [Authorize]
        [Route("{tournamentId:int}/subscription")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update tournament subscription.")]
        [SwaggerResponse(200, "Player subscription updated.", typeof(TournamentPlayer))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> UpdateSubscription(int tournamentId, int fifaTeamId)
        {
            await this.mediator.Send(new GetConnectedPlayerQuery { ConnectedPlayer = this.User.GetConnectedPlayer() });

            var result = await this.mediator.Send(new UpdateTournamentSubscriptionCommand
            {
                TournamentId = tournamentId,
                FifaTeamId = fifaTeamId,
                Player = this.User.GetConnectedPlayer(),
            });

            if (result is null)
            {
                return this.Problem();
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Delete tournament.
        /// </summary>
        /// <param name="id">Tournament ID.</param>
        /// <returns>IActionResult object.</returns>
        [HttpDelete]
        [Authorize(Roles = "gameon_admin")]
        [Route("{id:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Delete tournament in database.")]
        [SwaggerResponse(204, "Deleted tournament.")]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(403, "Not enough roles.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await this.mediator.Send(new DeleteTournamentCommand { TournamentId = id });

            if (result == false)
            {
                return this.Problem();
            }
            else
            {
                return this.NoContent();
            }
        }

        /// <summary>
        /// Upload tournament picture.
        /// </summary>
        /// <param name="tournamentId">Tournament ID.</param>
        /// <param name="tournamentPicture">Tournament picture file.</param>
        /// <returns>IActionResult.</returns>
        [HttpPost]
        [Authorize(Roles = "gameon_admin")]
        [Route("{tournamentId}/logo")]
        [SwaggerOperation(Summary = "Updates tournament picture.")]
        [SwaggerResponse(201, "Tournament's picture created on the server.")]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(406, "Wrong file format.")]
        [SwaggerResponse(404, "No profile picture found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> UpdateTournamentPicture(int tournamentId, IFormFile tournamentPicture)
        {
            if (!CheckIfPictureFormatIsAuthorized(tournamentPicture.ContentType))
            {
                return this.StatusCode(406);
            }

            var currentPlayer = await this.mediator.Send(new UpdateTournamentPictureCommand { TournamentId = tournamentId, File = tournamentPicture });

            return currentPlayer ? this.Created() : this.Problem();
        }

        /// <summary>
        /// Gets tournament logo from network attached storage.
        /// </summary>
        /// <param name="tournamentId">Tournament ID.</param>
        /// <returns>File.</returns>
        [HttpGet]
        [Route("{tournamentId}/logo")]
        [SwaggerOperation(Summary = "Get tournament logo from server.")]
        [SwaggerResponse(200, "Tournament logo from server.", typeof(FileStreamResult))]
        [SwaggerResponse(404, "No tournament logo found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetTournamentLogo(int tournamentId)
        {
            var tpDto = await this.mediator.Send(new GetTournamentLogoQuery { TournamentId = tournamentId });

            if (tpDto.FileStream is null)
            {
                return this.NotFound();
            }

            return this.File(tpDto.FileStream, this.nsService.GetContentType(tpDto.FileName));
        }

        /// <summary>
        /// Checks if file type is valid.
        /// </summary>
        /// <param name="contentType">File content type.</param>
        /// <returns>True if valid, false if not.</returns>
        private static bool CheckIfPictureFormatIsAuthorized(string contentType)
        {
            return contentType is "image/jpeg" or "image/png" or "image/gif" or "image/webp";
        }
    }
}
