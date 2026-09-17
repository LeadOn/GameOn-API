// <copyright file="DependencyInjection.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External
{
    using GameOn.External.CommunityDragon.Implementations;
    using GameOn.External.CommunityDragon.Interfaces;
    using GameOn.External.Llm.Implementations;
    using GameOn.External.Llm.Interfaces;
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
            services.AddScoped<IQueueService, QueueService>();
            services.AddScoped<ICommunityDragonQueueService, CommunityDragonQueueService>();
            services.AddScoped<ICommunityDragonChampionService, CommunityDragonChampionService>();

            // The provider is picked at startup rather than at compile time: switching to the self-hosted box,
            // or falling back off it, is an environment variable and a restart rather than a deployment.
            var provider = (Environment.GetEnvironmentVariable("LLM_PROVIDER") ?? "gemini").Trim().ToLowerInvariant();

            // Dedicated client: a generation runs far longer than the 100 second default carried by the ambient
            // HttpClient above, which would abort every request well before the model is done. A model running
            // on CPU needs far more room still, and its first call also pays to load the weights off disk.
            var defaultTimeout = provider == "ollama" ? 900 : 180;
            var llmTimeout = TimeSpan.FromSeconds(
                int.TryParse(Environment.GetEnvironmentVariable("LLM_TIMEOUT_SECONDS"), out var timeout) ? timeout : defaultTimeout);

            switch (provider)
            {
                case "ollama":
                    services.AddHttpClient(OllamaLlmService.HttpClientName, client => client.Timeout = llmTimeout);
                    services.AddScoped<ILlmService, OllamaLlmService>();
                    break;

                case "groq":
                    services.AddHttpClient(GroqLlmService.HttpClientName, client => client.Timeout = llmTimeout);
                    services.AddScoped<ILlmService, GroqLlmService>();
                    break;

                default:
                    services.AddHttpClient(GeminiLlmService.HttpClientName, client => client.Timeout = llmTimeout);
                    services.AddScoped<ILlmService, GeminiLlmService>();
                    break;
            }

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
