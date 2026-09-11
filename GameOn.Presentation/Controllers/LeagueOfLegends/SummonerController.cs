// <copyright file="SummonerController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers.LeagueOfLegends
{
    using GameOn.Application.Common.Players.Queries.GetConnectedPlayer;
    using GameOn.Application.Common.Players.Queries.GetPlayerById;
    using GameOn.Application.LeagueOfLegends.Summoners.Commands.LinkSmurfAccount;
    using GameOn.Application.LeagueOfLegends.Summoners.Commands.UnlinkSmurfAccount;
    using GameOn.Application.LeagueOfLegends.Summoners.Commands.UpdateAllPlayerRanks;
    using GameOn.Application.LeagueOfLegends.Summoners.Commands.UpdatePlayerSummoner;
    using GameOn.Application.LeagueOfLegends.Summoners.Commands.UpdatePlayerSummonerAdmin;
    using GameOn.Application.LeagueOfLegends.Summoners.Queries.GetAllLeaguePlayers;
    using GameOn.Application.LeagueOfLegends.Summoners.Queries.GetLeaguePlayerById;
    using GameOn.Application.LeagueOfLegends.Summoners.Queries.GetSummonerRankHistory;
    using GameOn.Common.DTOs;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Domain;
    using GameOn.Presentation.Classes;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Summoner Controller.
    /// </summary>
    [ApiController]
    [Route("lol/[controller]")]
    public class SummonerController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SummonerController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public SummonerController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get all league of legends players in database.
        /// </summary>
        /// <param name="archived">If true, get archived players.</param>
        /// <param name="includeSmurfs">If false, only primary accounts are returned. Defaults to true: smurf accounts hold their own rank and are listed alongside their owner, tagged with <see cref="PlayerDto.PrimaryPlayerId"/>.</param>
        /// <returns>200 OK with Player list.</returns>
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all League of Legends players in database.")]
        [SwaggerResponse(200, "Players in database.", typeof(List<Player>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetAll(bool? archived, bool? includeSmurfs)
        {
            return this.Ok(await this.mediator.Send(new GetAllLeaguePlayersQuery { Archived = archived ?? false, IncludeSmurfs = includeSmurfs ?? true }));
        }

        /// <summary>
        /// Get a summoner by its ID.
        /// </summary>
        /// <param name="id">Summoner ID.</param>
        /// <param name="period">Rolling time window for <see cref="PlayerDto.PerformanceStats"/>. Defaults to all-time.</param>
        /// <param name="queues">Restrict <see cref="PlayerDto.PerformanceStats"/> to these queue IDs, comma-separated (Riot queueId, see LoLQueue).</param>
        /// <param name="teamPosition">Restrict <see cref="PlayerDto.PerformanceStats"/> to games played at this position (TOP, JUNGLE, MIDDLE, BOTTOM, UTILITY). Defaults to every role.</param>
        /// <returns>200 OK with Player if found, 404 if not found.</returns>
        [HttpGet]
        [Route("{id:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get a summoner by its ID.", Description = "Get a summoner by its ID, and retrieve its information.")]
        [SwaggerResponse(200, "Summoner is found.", typeof(PlayerDto))]
        [SwaggerResponse(404, "Player not found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetSummonerById(int id, LoLStatsPeriod? period, string? queues = null, string? teamPosition = null)
        {
            var playerInDb = await this.mediator.Send(new GetLeaguePlayerByIdQuery { PlayerId = id, Period = period ?? LoLStatsPeriod.AllTime, QueueIds = ParseQueueIds(queues), TeamPosition = teamPosition });

            if (playerInDb is not null)
            {
                return this.Ok(playerInDb);
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Get summoner rank history.
        /// </summary>
        /// <param name="id">Summoner ID.</param>
        /// <param name="limit">Limit.</param>
        /// <param name="granularity">When set, only the last rank snapshot of each period (day/week/month) is returned, instead of every change.</param>
        /// <param name="days">How many days back to look when <paramref name="granularity"/> is set. Defaults to a sensible window for the chosen granularity.</param>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Route("{id:int}/rank")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get summoner League of Legends history.")]
        [SwaggerResponse(200, "List of rank history.", typeof(List<LeagueOfLegendsRankHistory>))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetRankHistory(int id, int? limit, LoLRankHistoryGranularity? granularity, int? days)
        {
#pragma warning disable CS8601 // Existence possible d'une assignation de référence null.
            return this.Ok(await this.mediator.Send(new GetSummonerRankHistoryQuery { PlayerId = id, Limit = limit, Granularity = granularity, Days = days }));
#pragma warning restore CS8601 // Existence possible d'une assignation de référence null.
        }

        /// <summary>
        /// Refresh summoner by ID.
        /// </summary>
        /// <param name="id">Player ID.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPatch]
        [Route("{id:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update user League of Legends summoner.")]
        [SwaggerResponse(200, "Updated user profile.", typeof(Player))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> RefreshById(int id)
        {
            var playerInDb = await this.mediator.Send(new GetPlayerByIdQuery { PlayerId = id });

#pragma warning disable CS8601 // Existence possible d'une assignation de référence null.
            return this.Ok(await this.mediator.Send(new UpdatePlayerSummonerCommand { Player = playerInDb }));
#pragma warning restore CS8601 // Existence possible d'une assignation de référence null.
        }

        /// <summary>
        /// Update summoner of connected player.
        /// </summary>
        /// <returns>IActionResult object.</returns>
        [HttpPatch]
        [Authorize]
        [Route("me")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update current user League of Legends summoner.")]
        [SwaggerResponse(200, "Updated user profile.", typeof(Player))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> RefreshConnected()
        {
            var playerInDb = await this.mediator.Send(new GetConnectedPlayerQuery { ConnectedPlayer = this.User.GetConnectedPlayer() });

#pragma warning disable CS8601 // Existence possible d'une assignation de référence null.
            return this.Ok(await this.mediator.Send(new UpdatePlayerSummonerCommand { Player = playerInDb }));
#pragma warning restore CS8601 // Existence possible d'une assignation de référence null.
        }

        /// <summary>
        /// Update summoner of given player.
        /// </summary>
        /// <param name="playerId">Player ID.</param>
        /// <param name="riotGamesNickname">Riot Games Nickname.</param>
        /// <param name="riotGamesTagLine">Riot Games Tag Line (ex: EUW).</param>
        /// <returns>IActionResult object.</returns>
        [HttpPatch]
        [Authorize(Roles = "gameon_admin")]
        [Route("{playerId:int}/admin")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update user League of Legends profile.")]
        [SwaggerResponse(200, "Updated user profile.", typeof(Player))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> UpdateSummoner(int playerId, string riotGamesNickname, string riotGamesTagLine)
        {
            var playerInDb = await this.mediator.Send(new GetPlayerByIdQuery { PlayerId = playerId });

            if (playerInDb == null)
            {
                return this.NotFound();
            }

            playerInDb.RiotGamesNickname = riotGamesNickname;
            playerInDb.RiotGamesTagLine = riotGamesTagLine;

#pragma warning disable CS8601 // Existence possible d'une assignation de référence null.
            return this.Ok(await this.mediator.Send(new UpdatePlayerSummonerAdminCommand { Player = playerInDb }));
#pragma warning restore CS8601 // Existence possible d'une assignation de référence null.
        }

        /// <summary>
        /// Update every summoner rank.
        /// </summary>
        /// <returns>IActionResult object.</returns>
        [HttpPatch]
        [Route("ranks")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update every League of Legends ranks.")]
        [SwaggerResponse(204, "Updated user profiles.", typeof(Player))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> UpdateAllRanks()
        {
            await this.mediator.Send(new UpdateAllPlayerRanksCommand());
            return this.NoContent();
        }

        /// <summary>
        /// Attach a Riot account to a player as one of their smurfs.
        /// </summary>
        /// <param name="playerId">ID of the player the account belongs to.</param>
        /// <param name="riotGamesNickname">Riot Games Nickname (game name) of the smurf account.</param>
        /// <param name="riotGamesTagLine">Riot Games Tag Line of the smurf account (ex: EUW).</param>
        /// <returns>IActionResult object.</returns>
        [HttpPost]
        [Authorize(Roles = "gameon_admin")]
        [Route("{playerId:int}/smurfs")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Link a smurf account to a player.", Description = "Creates the account if that Riot ID isn't known yet, pulls its rank and recent games, and re-attaches the games it already played with GameOn members.")]
        [SwaggerResponse(200, "Account linked.", typeof(LinkSmurfAccountResultDto))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(403, "Not enough roles.")]
        [SwaggerResponse(404, "Player or Riot account not found.")]
        [SwaggerResponse(409, "Account cannot be linked to that player.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> LinkSmurfAccount(int playerId, string riotGamesNickname, string riotGamesTagLine)
        {
            var result = await this.mediator.Send(new LinkSmurfAccountCommand
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
        /// Detach a smurf account from the player it belongs to. The account, its rank history and its
        /// games are kept; it simply stops being counted as part of that player.
        /// </summary>
        /// <param name="smurfId">ID of the smurf account to detach.</param>
        /// <returns>IActionResult object.</returns>
        [HttpDelete]
        [Authorize(Roles = "gameon_admin")]
        [Route("smurfs/{smurfId:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Unlink a smurf account from its player.")]
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
        /// Parses a comma-separated list of queue IDs (e.g. "420,440") into a list of ints.
        /// Duplicated from <see cref="MatchController"/>: worth factoring out into a shared
        /// helper if a third caller shows up.
        /// </summary>
        /// <param name="queues">Comma-separated queue IDs.</param>
        /// <returns>Parsed list, or null if the input is empty.</returns>
        private static List<int>? ParseQueueIds(string? queues)
        {
            if (string.IsNullOrWhiteSpace(queues))
            {
                return null;
            }

            return queues
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(int.Parse)
                .ToList();
        }
    }
}
