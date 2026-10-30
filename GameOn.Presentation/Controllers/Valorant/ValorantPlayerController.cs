// <copyright file="ValorantPlayerController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers.Valorant
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using GameOn.Application.Common.Players.Commands.SetCrewMembership;
    using GameOn.Application.Common.Players.Commands.UnlinkSmurfAccount;
    using GameOn.Application.Common.Players.Queries.GetConnectedPlayer;
    using GameOn.Application.Common.Players.Queries.GetPlayerById;
    using GameOn.Application.Valorant.Players.Commands.LinkValorantSmurfAccount;
    using GameOn.Application.Valorant.Players.Commands.UpdateAllValorantPlayerRanks;
    using GameOn.Application.Valorant.Players.Commands.UpdateValorantPlayer;
    using GameOn.Application.Valorant.Players.Commands.UpdateValorantPlayerAdmin;
    using GameOn.Application.Valorant.Players.Queries.GetAllValorantPlayers;
    using GameOn.Application.Valorant.Players.Queries.GetValorantPlayerById;
    using GameOn.Application.Valorant.Players.Queries.GetValorantPlayerRankHistory;
    using GameOn.Common.DTOs;
    using GameOn.Common.DTOs.Common;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Domain;
    using GameOn.Presentation.Classes;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Valorant Player Controller.
    /// </summary>
    [ApiController]
    [Route("val/player")]
    public class ValorantPlayerController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValorantPlayerController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public ValorantPlayerController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get all Valorant players in database.
        /// </summary>
        /// <param name="archived">If true, get archived players.</param>
        /// <param name="includeSmurfs">If false, only primary accounts are returned. Defaults to true.</param>
        /// <param name="includeOutOfCrew">If true, accounts outside the crew are listed too. Defaults to false.</param>
        /// <returns>200 OK with PlayerDto list.</returns>
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all Valorant players in database.")]
        [SwaggerResponse(200, "Valorant players in database.", typeof(List<PlayerDto>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetAll(bool? archived, bool? includeSmurfs, bool? includeOutOfCrew)
        {
            return this.Ok(await this.mediator.Send(new GetAllValorantPlayersQuery
            {
                Archived = archived ?? false,
                IncludeSmurfs = includeSmurfs ?? true,
                IncludeOutOfCrew = includeOutOfCrew ?? false,
            }));
        }

        /// <summary>
        /// Get a Valorant player by its ID.
        /// </summary>
        /// <param name="id">Player ID.</param>
        /// <param name="period">Rolling time window of the performance stats: AllTime, Week, Month, ThreeMonths or SixMonths.</param>
        /// <param name="queues">Comma-separated queue IDs the performance stats are restricted to (e.g. "competitive,unrated"). Every counted queue when omitted; customs and deathmatch modes never count.</param>
        /// <param name="agentName">Agent the performance stats are restricted to (e.g. "Jett").</param>
        /// <returns>200 OK with PlayerDto if found, 404 if not found.</returns>
        [HttpGet]
        [Route("{id:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get a Valorant player by its ID.", Description = "Get a Valorant player by its ID, and retrieve its account information, current rank, peak rank, player card URLs and performance stats.")]
        [SwaggerResponse(200, "Valorant player is found.", typeof(PlayerDto))]
        [SwaggerResponse(404, "Player not found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetById(int id, LoLStatsPeriod period = LoLStatsPeriod.AllTime, string? queues = null, string? agentName = null)
        {
            var playerInDb = await this.mediator.Send(new GetValorantPlayerByIdQuery
            {
                PlayerId = id,
                Period = period,
                Queues = string.IsNullOrWhiteSpace(queues)
                    ? null
                    : queues.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList(),
                AgentName = agentName,
            });

            if (playerInDb is not null)
            {
                return this.Ok(playerInDb);
            }

            return this.NotFound();
        }

        /// <summary>
        /// Get a Valorant player's rank and MMR history.
        /// </summary>
        /// <param name="id">Player ID.</param>
        /// <param name="limit">Maximum number of entries to return when <paramref name="granularity"/> is omitted. Defaults to 50.</param>
        /// <param name="granularity">When set, only the last rank snapshot of each period (Day/Week/Month) is returned, with carry-forward across idle periods.</param>
        /// <param name="days">How many days back to look when <paramref name="granularity"/> is set.</param>
        /// <returns>200 OK with rank history entries, oldest first.</returns>
        [HttpGet]
        [Route("{id:int}/rank")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get a Valorant player's rank history.")]
        [SwaggerResponse(200, "List of Valorant rank history entries.", typeof(List<ValorantRankHistory>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetRankHistory(int id, int? limit, LoLRankHistoryGranularity? granularity, int? days)
        {
            return this.Ok(await this.mediator.Send(new GetValorantPlayerRankHistoryQuery
            {
                PlayerId = id,
                Limit = limit,
                Granularity = granularity,
                Days = days,
            }));
        }

        /// <summary>
        /// Refresh Valorant player profile and rank by ID.
        /// </summary>
        /// <param name="id">Player ID.</param>
        /// <returns>200 OK with updated PlayerDto, 404 if not found.</returns>
        [HttpPatch]
        [Route("{id:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Refresh user Valorant profile and rank by ID.")]
        [SwaggerResponse(200, "Updated Valorant profile.", typeof(PlayerDto))]
        [SwaggerResponse(404, "Player not found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> RefreshById(int id)
        {
            var playerInDb = await this.mediator.Send(new GetPlayerByIdQuery { PlayerId = id });

            if (playerInDb is null)
            {
                return this.NotFound();
            }

            return this.Ok(await this.mediator.Send(new UpdateValorantPlayerCommand { Player = playerInDb }));
        }

        /// <summary>
        /// Refresh Valorant profile and rank of connected player.
        /// </summary>
        /// <returns>200 OK with updated PlayerDto.</returns>
        [HttpPatch]
        [Authorize]
        [Route("me")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Refresh current user Valorant profile and rank.")]
        [SwaggerResponse(200, "Updated Valorant profile.", typeof(PlayerDto))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(404, "Player not found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> RefreshConnected()
        {
            var playerInDb = await this.mediator.Send(new GetConnectedPlayerQuery { ConnectedPlayer = this.User.GetConnectedPlayer() });

            if (playerInDb is null)
            {
                return this.NotFound();
            }

            return this.Ok(await this.mediator.Send(new UpdateValorantPlayerCommand { Player = playerInDb }));
        }

        /// <summary>
        /// Update Riot ID and refresh Valorant profile of given player (Admin only).
        /// </summary>
        /// <param name="playerId">Player ID.</param>
        /// <param name="riotGamesNickname">Riot Games Nickname.</param>
        /// <param name="riotGamesTagLine">Riot Games Tag Line (ex: EUW).</param>
        /// <returns>200 OK with updated PlayerDto, 404 if not found.</returns>
        [HttpPatch]
        [Authorize(Roles = "gameon_admin")]
        [Route("{playerId:int}/admin")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update user Riot ID and refresh Valorant profile (Admin only).")]
        [SwaggerResponse(200, "Updated Valorant profile.", typeof(PlayerDto))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(403, "Not enough roles.")]
        [SwaggerResponse(404, "Player not found.")]
        [SwaggerResponse(409, "Riot ID already used by another player, nothing was saved.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> UpdateValorantPlayer(int playerId, string riotGamesNickname, string riotGamesTagLine)
        {
            var playerInDb = await this.mediator.Send(new GetPlayerByIdQuery { PlayerId = playerId });

            if (playerInDb is null)
            {
                return this.NotFound();
            }

            playerInDb.RiotGamesNickname = riotGamesNickname;
            playerInDb.RiotGamesTagLine = riotGamesTagLine;

            var result = await this.mediator.Send(new UpdateValorantPlayerAdminCommand { Player = playerInDb });

            return result.Status switch
            {
                RiotIdUpdateStatus.Updated => this.Ok(result.Player),
                _ => this.Conflict("That Riot ID is already used by another player."),
            };
        }

        /// <summary>
        /// Move an account in or out of the crew (Admin only). The crew is shared by every game.
        /// </summary>
        /// <param name="id">Account ID.</param>
        /// <param name="inCrew">True to put the account in the crew, false to take it out.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPatch]
        [Authorize(Roles = "gameon_admin")]
        [Route("{id:int}/crew")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Move an account in or out of the crew.", Description = "Same membership as PATCH /lol/Summoner/{id}/crew: the crew is shared by League of Legends and Valorant. Crew accounts are refreshed automatically and are the only ones counted by the lists and the stats. Taking a player out also takes their smurf accounts out; putting them back does not put the smurfs back.")]
        [SwaggerResponse(200, "Membership updated.", typeof(SetCrewMembershipResultDto))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(403, "Not enough roles.")]
        [SwaggerResponse(404, "Account not found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> SetCrewMembership(int id, bool inCrew)
        {
            var result = await this.mediator.Send(new SetCrewMembershipCommand { PlayerId = id, InCrew = inCrew });

            if (result is null)
            {
                return this.NotFound();
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Attach a Riot account to a player as one of their smurfs (Admin only).
        /// </summary>
        /// <param name="playerId">ID of the player the account belongs to.</param>
        /// <param name="riotGamesNickname">Riot Games Nickname (game name) of the smurf account.</param>
        /// <param name="riotGamesTagLine">Riot Games Tag Line of the smurf account (ex: EUW).</param>
        /// <returns>IActionResult object.</returns>
        [HttpPost]
        [Authorize(Roles = "gameon_admin")]
        [Route("{playerId:int}/smurfs")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Link a smurf account to a player.", Description = "Resolves the Riot ID through HenrikDev, creates the account if it isn't known yet (outside the crew), re-attaches the Valorant games already stored for it, then pulls its rank and recent games. The link is shared with League of Legends.")]
        [SwaggerResponse(200, "Account linked.", typeof(LinkSmurfAccountResultDto))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(403, "Not enough roles.")]
        [SwaggerResponse(404, "Player or Riot account not found.")]
        [SwaggerResponse(409, "Account cannot be linked to that player.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> LinkSmurfAccount(int playerId, string riotGamesNickname, string riotGamesTagLine)
        {
            var result = await this.mediator.Send(new LinkValorantSmurfAccountCommand
            {
                PrimaryPlayerId = playerId,
                RiotGamesNickname = riotGamesNickname,
                RiotGamesTagLine = riotGamesTagLine,
            });

            return result.Status switch
            {
                LinkSmurfAccountStatus.Linked => this.Ok(result),
                LinkSmurfAccountStatus.PrimaryNotFound => this.NotFound("Player not found."),
                LinkSmurfAccountStatus.RiotAccountNotFound => this.NotFound("Riot Games account not found."),
                LinkSmurfAccountStatus.PrimaryIsSmurf => this.Conflict("That player is already a smurf account: link the smurf to its primary account instead."),
                _ => this.Conflict("That Riot account is already used by another player: unlink it first."),
            };
        }

        /// <summary>
        /// Detach a smurf account from the player it belongs to (Admin only). The account, its rank
        /// history and its games are kept; it simply stops being counted as part of that player.
        /// </summary>
        /// <param name="smurfId">ID of the smurf account to detach.</param>
        /// <returns>IActionResult object.</returns>
        [HttpDelete]
        [Authorize(Roles = "gameon_admin")]
        [Route("smurfs/{smurfId:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Unlink a smurf account from its player.", Description = "Same link as DELETE /lol/Summoner/smurfs/{smurfId}: it is shared with League of Legends.")]
        [SwaggerResponse(200, "Account detached.", typeof(Player))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(403, "Not enough roles.")]
        [SwaggerResponse(404, "Smurf account not found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> UnlinkSmurfAccount(int smurfId)
        {
            var smurfAccount = await this.mediator.Send(new UnlinkSmurfAccountCommand { SmurfPlayerId = smurfId });

            if (smurfAccount is null)
            {
                return this.NotFound();
            }

            return this.Ok(smurfAccount);
        }

        /// <summary>
        /// Refresh Valorant profile and rank of every crew member.
        /// </summary>
        /// <returns>204 No Content.</returns>
        [HttpPatch]
        [Route("ranks")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Refresh every crew member's Valorant profile and rank.")]
        [SwaggerResponse(204, "Updated Valorant profiles and ranks.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> UpdateAllRanks()
        {
            await this.mediator.Send(new UpdateAllValorantPlayerRanksCommand());
            return this.NoContent();
        }
    }
}