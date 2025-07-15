// <copyright file="DependencyInjection.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application
{
    using System.Reflection;
    using GameOn.Common.Exceptions;
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
            return services;
        }
    }
}
