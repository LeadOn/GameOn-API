// <copyright file="DependencyInjection.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External
{
    using GameOn.External.RiotGames.Implementations;
    using GameOn.External.RiotGames.Interfaces;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Dependency Injection class.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds all external dependency injections.
        /// </summary>
        /// <param name="services">IServiceCollection object.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddExternal(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountV1Service>();
            services.AddScoped<ISummonerService, SummonerV4Service>();
            services.AddScoped<ILeagueService, LeagueV4Service>();
            services.AddScoped<IMatchService, MatchV5Service>();
            return services;
        }
    }
}
