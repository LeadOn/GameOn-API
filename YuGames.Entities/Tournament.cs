// <copyright file="Tournament.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Entities;

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
    /// Gets or sets Planned From date.
    /// </summary>
    public DateTime PlannedFrom { get; set; }

    /// <summary>
    /// Gets or sets Planned To date.
    /// </summary>
    public DateTime PlannedTo { get; set; }

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