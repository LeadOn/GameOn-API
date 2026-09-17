// <copyright file="LoLGameCoachReport.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// LoLGameCoachReport class. AI-generated coaching analysis of one player's performance in one game.
    /// One row per (match, player): the analysis is personal, not a summary of the game.
    /// </summary>
    /// <remarks>
    /// Generation is on demand and synchronous, so a row here always holds a finished analysis - there is no
    /// pending or failed state to model. The table is, in effect, the cache that keeps a given analysis from
    /// ever being paid for twice.
    /// </remarks>
    public class LoLGameCoachReport
    {
        /// <summary>
        /// Gets or sets the report ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the match ID this report analyses.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the PUUID of the analysed participant. Kept alongside <see cref="PlayerId"/> because
        /// it is the only stable way back to the participant row, including on imported customs where the
        /// League client anonymises PUUIDs, and on smurf accounts.
        /// </summary>
        public string Puuid { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the GameOn! player this report is about.
        /// </summary>
        public int? PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the version of the prompt that produced this report. Bump it whenever the prompt or the
        /// context builder changes meaningfully: a report written by an older version is served as-is, but can
        /// be found and regenerated on purpose instead of silently going stale.
        /// </summary>
        public int PromptVersion { get; set; }

        /// <summary>
        /// Gets or sets the model that produced this report.
        /// </summary>
        public string? ModelName { get; set; }

        /// <summary>
        /// Gets or sets the one-paragraph verdict, lifted out of <see cref="ContentJson"/> so it can be listed
        /// and searched without deserialising every report.
        /// </summary>
        public string? Summary { get; set; }

        /// <summary>
        /// Gets or sets the rating out of 10 the model gave the performance. Purely editorial: unlike
        /// <see cref="LoLGameParticipantStat.Rating"/> it is not reproducible and must never be averaged or ranked on.
        /// </summary>
        public double? Rating { get; set; }

        /// <summary>
        /// Gets or sets the full structured analysis, as returned by the model and validated against the response
        /// schema (verdict, strengths, improvement areas).
        /// </summary>
        public string? ContentJson { get; set; }

        /// <summary>
        /// Gets or sets the date this report was generated.
        /// </summary>
        public DateTime GeneratedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the game this report analyses.
        /// </summary>
        [JsonIgnore]
        public virtual LoLGame Game { get; set; } = null!;

        /// <summary>
        /// Gets or sets the player this report is about.
        /// </summary>
        [JsonIgnore]
        public virtual Player? Player { get; set; }
    }
}
