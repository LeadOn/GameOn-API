// <copyright file="LoLLiveGameDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    /// <summary>
    /// LoLLiveGameDto class. A tracked account currently in a game, as reported by Riot's spectator-v5. One
    /// entry per account: several crew members in the same game produce several entries sharing the same
    /// <see cref="GameId"/>. Served from a server-side cache, so it can lag reality by up to a minute (see
    /// <see cref="RetrievedOn"/>).
    /// </summary>
    public class LoLLiveGameDto
    {
        /// <summary>
        /// Gets or sets the account in game. Identity fields only: ranks, recent form and performance stats
        /// are left empty here.
        /// </summary>
        public PlayerDto Player { get; set; } = null!;

        /// <summary>
        /// Gets or sets Riot's game ID, identical for every account in the same game. Group on it to show
        /// crew members playing together (or against each other, see <see cref="TeamId"/>). Numeric only:
        /// match-v5 IDs are this number prefixed with the platform (ex: <c>EUW1_</c>), and the game only
        /// exists in match-v5 once it is over.
        /// </summary>
        public long GameId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the champion the account is playing.
        /// </summary>
        public int ChampionId { get; set; }

        /// <summary>
        /// Gets or sets the name of the champion the account is playing, as Riot's internal name, the same
        /// convention as <c>LoLGameParticipant.ChampionName</c> (ex: "MonkeyKing" for Wukong). Null when the
        /// champion is too recent to be known by the champion referential yet: fall back on
        /// <see cref="ChampionId"/>.
        /// </summary>
        public string? ChampionName { get; set; }

        /// <summary>
        /// Gets or sets the queue ID (see <c>GET lol/Queue</c>), same identifier as <c>LoLGame.QueueId</c>.
        /// Null when Riot doesn't give one, which is typically the case of a custom game.
        /// </summary>
        public int? QueueId { get; set; }

        /// <summary>
        /// Gets or sets the side the account plays on: 100 for blue, 200 for red. Two crew members in the same
        /// game with different values are playing against each other.
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// Gets or sets when the game started, in UTC. Null while the players are still on the loading
        /// screen: Riot only sets the start time once the game itself is running.
        /// </summary>
        public DateTime? GameStart { get; set; }

        /// <summary>
        /// Gets or sets the time elapsed in the game, in seconds, as Riot reported it at
        /// <see cref="RetrievedOn"/>: add the time elapsed since then for a live counter, or derive it from
        /// <see cref="GameStart"/>. 0 while the players are still on the loading screen.
        /// </summary>
        public long GameLengthSeconds { get; set; }

        /// <summary>
        /// Gets or sets when Riot was asked about this account, in UTC. Normally at most a minute old, which is
        /// how long the server keeps an answer before asking again. Can be older while Riot is rate limiting
        /// the API: the last known answer is served until Riot accepts calls again.
        /// </summary>
        public DateTime RetrievedOn { get; set; }
    }
}
