// <copyright file="DependencyInjection.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application
{
    using System.Reflection;
    using GameOn.Application.LeagueOfLegends.Coach.Services;
    using GameOn.Application.Services.Jobs;
    using GameOn.Common.Exceptions;
    using GameOn.Common.Interfaces;
    using GameOn.External;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Dependency Injection class.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds all Application dependency injections.
        /// </summary>
        /// <param name="services">IServiceCollection object.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.LicenseKey = Environment.GetEnvironmentVariable("MEDIATR_LICENSE_KEY") ?? throw new MissingEnvironmentVariableException("MEDIATR_LICENSE_KEY");
            });
            services.AddExternal();

            // The coach queue is the state, so there can only be one of it, and exactly one consumer works
            // through it - that single consumer is what keeps calls to the model inside the provider's
            // allowance.
            services.AddSingleton<ILoLCoachQueue, LoLCoachQueue>();
            services.AddHostedService<RefreshLeagueSummonerRanksJob>();
            services.AddHostedService<RefreshLeagueQueuesJob>();
            services.AddHostedService<ProcessLoLCoachQueueJob>();
            return services;
        }
    }
}
