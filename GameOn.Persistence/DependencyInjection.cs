// <copyright file="DependencyInjection.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Persistence
{
    using GameOn.Common.Interfaces;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Dependency Injection class.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds all persistence dependency injections.
        /// </summary>
        /// <param name="services">IServiceCollection object.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddDbContext<GameOnContext>();
#pragma warning disable CS8603 // Possible null reference return.
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<GameOnContext>());
#pragma warning restore CS8603 // Possible null reference return.
            return services;
        }
    }
}
