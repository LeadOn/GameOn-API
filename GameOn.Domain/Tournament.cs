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
    public string Description { get; set; } = "Aucune description renseignée.";

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

    /// <summary>
    /// Gets or sets Tournament rules.
    /// </summary>
    public string? Rules { get; set; }

    /// <summary>
    /// Gets or sets Tournament Win Points.
    /// </summary>
    public int WinPoints { get; set; }

    /// <summary>
    /// Gets or sets Tournament Loose Points.
    /// </summary>
    public int LoosePoints { get; set; }

    /// <summary>
    /// Gets or sets Tournament Draw Points.
    /// </summary>
    public int DrawPoints { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether tournament is featured or not.
    /// </summary>
    public bool Featured { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether one or two games will be generated in phase 1.
    /// </summary>
    public bool PhaseOneDoubleRound { get; set; }

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