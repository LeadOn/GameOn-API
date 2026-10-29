// <copyright file="ValGameParticipant.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Participant entry in a Valorant match.
    /// </summary>
    public class ValGameParticipant
    {
        /// <summary>
        /// Competitive tier table UUID on valorant-api.com (Episode 5+ tier table including Ascendant).
        /// </summary>
        private const string CompetitiveTierTableUuid = "03621f52-342b-cf4e-4f86-9350a49c6d04";

        /// <summary>
        /// Gets or sets the primary key ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the parent match UUID.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the linked GameOn! player ID, if this participant is a tracked player.
        /// </summary>
        public int? PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the participant's Valorant PUUID.
        /// </summary>
        public string Puuid { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the participant's Riot ID game name.
        /// </summary>
        public string? RiotIdGameName { get; set; }

        /// <summary>
        /// Gets or sets the participant's Riot ID tag line.
        /// </summary>
        public string? RiotIdTagLine { get; set; }

        /// <summary>
        /// Gets or sets the participant's team identifier ("Red", "Blue", or PUUID in Deathmatch).
        /// </summary>
        public string? TeamId { get; set; }

        /// <summary>
        /// Gets or sets the premade party UUID. Participants sharing the same PartyId queued together.
        /// </summary>
        public string? PartyId { get; set; }

        /// <summary>
        /// Gets or sets the participant's platform ("pc" or "console").
        /// </summary>
        public string? Platform { get; set; }

        /// <summary>
        /// Gets or sets the participant's Valorant account level at the time of the match.
        /// </summary>
        public int AccountLevel { get; set; }

        /// <summary>
        /// Gets or sets the participant's equipped player card UUID.
        /// </summary>
        public string? PlayerCardId { get; set; }

        /// <summary>
        /// Gets or sets the participant's equipped player title UUID.
        /// </summary>
        public string? PlayerTitleId { get; set; }

        /// <summary>
        /// Gets or sets the played agent UUID.
        /// </summary>
        public string? AgentId { get; set; }

        /// <summary>
        /// Gets or sets the played agent display name (e.g. "Jett", "Raze", "Omen").
        /// </summary>
        public string? AgentName { get; set; }

        /// <summary>
        /// Gets or sets the participant's competitive tier ID at the time of the match (0 = Unranked, 3..27 = Iron 1..Radiant).
        /// </summary>
        public int TierId { get; set; }

        /// <summary>
        /// Gets or sets the participant's competitive tier name (e.g. "Platinum 3", "Unrated").
        /// </summary>
        public string? TierName { get; set; }

        /// <summary>
        /// Gets or sets the total combat score across all rounds.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets the total kills.
        /// </summary>
        public int Kills { get; set; }

        /// <summary>
        /// Gets or sets the total deaths.
        /// </summary>
        public int Deaths { get; set; }

        /// <summary>
        /// Gets or sets the total assists.
        /// </summary>
        public int Assists { get; set; }

        /// <summary>
        /// Gets or sets the total headshot hits.
        /// </summary>
        public int Headshots { get; set; }

        /// <summary>
        /// Gets or sets the total bodyshot hits.
        /// </summary>
        public int Bodyshots { get; set; }

        /// <summary>
        /// Gets or sets the total legshot hits.
        /// </summary>
        public int Legshots { get; set; }

        /// <summary>
        /// Gets or sets the total damage dealt across the match.
        /// </summary>
        public int DamageDealt { get; set; }

        /// <summary>
        /// Gets or sets the total damage received across the match.
        /// </summary>
        public int DamageReceived { get; set; }

        /// <summary>
        /// Gets or sets the C ability (grenade slot) cast count.
        /// </summary>
        public int GrenadeCasts { get; set; }

        /// <summary>
        /// Gets or sets the Q ability (ability1 slot) cast count.
        /// </summary>
        public int Ability1Casts { get; set; }

        /// <summary>
        /// Gets or sets the E ability (ability2 / signature slot) cast count.
        /// </summary>
        public int Ability2Casts { get; set; }

        /// <summary>
        /// Gets or sets the X ability (ultimate slot) cast count.
        /// </summary>
        public int UltimateCasts { get; set; }

        /// <summary>
        /// Gets or sets the total credits spent across the match.
        /// </summary>
        public int SpentOverall { get; set; }

        /// <summary>
        /// Gets or sets the average credits spent per round.
        /// </summary>
        public double SpentAverage { get; set; }

        /// <summary>
        /// Gets or sets the total loadout value across the match.
        /// </summary>
        public int LoadoutValueOverall { get; set; }

        /// <summary>
        /// Gets or sets the average loadout value per round.
        /// </summary>
        public double LoadoutValueAverage { get; set; }

        /// <summary>
        /// Gets or sets the number of AFK rounds.
        /// </summary>
        public double AfkRounds { get; set; }

        /// <summary>
        /// Gets or sets the number of rounds spent in spawn.
        /// </summary>
        public double RoundsInSpawn { get; set; }

        /// <summary>
        /// Gets or sets the friendly-fire damage received from teammates.
        /// </summary>
        public double FriendlyFireIncoming { get; set; }

        /// <summary>
        /// Gets or sets the friendly-fire damage dealt to teammates.
        /// </summary>
        public double FriendlyFireOutgoing { get; set; }

        /// <summary>
        /// Gets or sets the session playtime in milliseconds.
        /// </summary>
        public long SessionPlaytimeMs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this participant won the match.
        /// </summary>
        public bool Win { get; set; }

        /// <summary>
        /// Gets or sets the Ranked Rating (RR) delta earned or lost by this participant in this match,
        /// populated from <see cref="ValorantRankHistory"/> when available.
        /// </summary>
        public int? RrChange { get; set; }

        /// <summary>
        /// Gets or sets the post-game competitive tier ID from <see cref="ValorantRankHistory"/>, when available.
        /// </summary>
        public int? PostGameTierId { get; set; }

        /// <summary>
        /// Gets or sets the post-game competitive tier name from <see cref="ValorantRankHistory"/>, when available.
        /// </summary>
        public string? PostGameTierName { get; set; }

        /// <summary>
        /// Gets or sets the post-game Ranked Rating (RR) within the tier from <see cref="ValorantRankHistory"/>, when available.
        /// </summary>
        public int? PostGameRankedRating { get; set; }

        /// <summary>
        /// Gets the agent square icon URL on <c>media.valorant-api.com</c>, or null when no agent UUID is set.
        /// </summary>
        [NotMapped]
        public string? AgentIconUrl => string.IsNullOrWhiteSpace(this.AgentId)
            ? null
            : $"https://media.valorant-api.com/agents/{this.AgentId}/displayicon.png";

        /// <summary>
        /// Gets the competitive rank icon URL on <c>media.valorant-api.com</c> for the participant's tier
        /// (preferring <see cref="PostGameTierId"/> when available), or null if unranked.
        /// </summary>
        [NotMapped]
        public string? TierIconUrl
        {
            get
            {
                var effectiveTier = this.PostGameTierId is >= 3 ? this.PostGameTierId.Value : this.TierId;
                return effectiveTier is >= 3 and <= 27
                    ? $"https://media.valorant-api.com/competitivetiers/{CompetitiveTierTableUuid}/{effectiveTier}/largeicon.png"
                    : null;
            }
        }

        /// <summary>
        /// Gets or sets the linked GameOn! player, if any.
        /// </summary>
        public virtual Player? Player { get; set; }

        /// <summary>
        /// Gets or sets the parent match.
        /// </summary>
        [JsonIgnore]
        public virtual ValGame Game { get; set; } = null!;

        /// <summary>
        /// Gets or sets the derived performance stats for this participant.
        /// </summary>
        public virtual ValGameParticipantStat? Stats { get; set; }
    }
}
