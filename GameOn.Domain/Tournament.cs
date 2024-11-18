// <copyright file="Tournament.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain;

using System.Text.Json.Serialization;

/// <summary>
/// Tournament class.
/// </summary>
public class Tournament
{
    /// <summary>
    /// Gets or sets Id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets Name.
    /// </summary>
    public string Name { get; set; } = "DEFAULT";

    /// <summary>
    /// Gets or sets Description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets State.
    /// </summary>
    public int State { get; set; }

    /// <summary>
    /// Gets or sets Logo URL.
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Gets or sets Phase 2 challonge URL.
    /// </summary>
    public string? Phase2ChallongeUrl { get; set; }

    /// <summary>
    /// Gets or sets Planned From date.
    /// </summary>
    public DateTime PlannedFrom { get; set; }

    /// <summary>
    /// Gets or sets Planned To date.
    /// </summary>
    public DateTime PlannedTo { get; set; }

    /// <summary>
    /// Gets or sets Tournament winner's ID.
    /// </summary>
    public int? WinnerId { get; set; }

    public string? Rules { get; set; }

    public int WinPoints { get; set; }

    public int LoosePoints { get; set; }

    public int DrawPoints { get; set; }

    /// <summary>
    /// Gets or sets Tournament winner.
    /// </summary>
    [JsonIgnore]
    public virtual Player? Winner { get; set; }

    /// <summary>
    /// Gets or sets Tournament Players.
    /// </summary>
    [JsonIgnore]
    public virtual List<TournamentPlayer> Players { get; set; } = null!;

    /// <summary>
    /// Gets or sets tournament games.
    /// </summary>
    [JsonIgnore]
    public virtual List<FifaGamePlayed> Games { get; set; } = null!;
}