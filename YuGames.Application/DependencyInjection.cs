// <copyright file="DependencyInjection.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application
{
    using System.Reflection;
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
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}
