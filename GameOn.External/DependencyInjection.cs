// <copyright file="DependencyInjection.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External
{
    using GameOn.External.NetworkStorage.Implementations;
    using GameOn.External.NetworkStorage.Interfaces;
    using GameOn.External.RiotGames.Implementations;
    using GameOn.External.RiotGames.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Minio;

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
            services.AddScoped<HttpClient>();
            services.AddScoped<IAccountService, AccountV1Service>();
            services.AddScoped<ISummonerService, SummonerV4Service>();
            services.AddScoped<ILeagueService, LeagueV4Service>();
            services.AddScoped<IMatchService, MatchV5Service>();

            // Adding connection to MinIO
            services.AddMinio(client =>
                client.WithEndpoint(Environment.GetEnvironmentVariable("S3_ENDPOINT") ?? throw new NotImplementedException())
                    .WithCredentials(
                        Environment.GetEnvironmentVariable("S3_ACCESS_KEY") ?? throw new NotImplementedException(),
                        Environment.GetEnvironmentVariable("S3_SECRET_KEY") ?? throw new NotImplementedException())
                    .WithSSL(false)
                    .Build());
            services.AddScoped<INetworkStorageService, MinIOService>();

            return services;
        }
    }
}
