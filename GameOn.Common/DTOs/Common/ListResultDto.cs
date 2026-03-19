// <copyright file="ListResultDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Common;

/// <summary>
/// Generic list of results coming from database.
/// </summary>
/// <typeparam name="T">Generic object.</typeparam>
public class ListResultDto<T>
{
    /// <summary>
    /// Gets or sets current page.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets number of results per page.
    /// </summary>
    public int ResultsPerPage { get; set; }

    /// <summary>
    /// Gets or sets total results count.
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// Gets or sets results.
    /// </summary>
    public List<T> Results { get; set; } = new List<T>();
}