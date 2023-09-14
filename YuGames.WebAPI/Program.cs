// <copyright file="Program.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Versioning;
using YuGames.Business;
using YuGames.Business.Contracts;
using YuGames.Common.Exceptions;
using YuGames.EntitiesContext;
using YuGames.Repository;
using YuGames.Repository.Contracts;
#pragma warning restore SA1200 // Using directives should be placed correctly

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
{
    policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
}));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.Authority = Environment.GetEnvironmentVariable("JWT_AUTHORITY") ?? throw new MissingEnvironmentVariableException("JWT_AUTHORITY");
    o.Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new MissingEnvironmentVariableException("JWT_AUDIENCE");
    o.RequireHttpsMetadata = false;
    o.Events = new JwtBearerEvents()
    {
        OnAuthenticationFailed = c =>
        {
            c.NoResult();

            c.Response.StatusCode = 401;
            c.Response.ContentType = "application/json";
            return c.Response.WriteAsJsonAsync(new { Error = "Unable to authenticate. Maybe your token is expired?" });
        },
    };
});

builder.Services.AddControllers();

// Adding API Versioning
builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    o.ReportApiVersions = true;
    o.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver"));
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Used for Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "1.3",
        Title = "LeadOn's Corp - YuGames API",
        Description = "This API goal is to monitor players performance across multiple games.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Email = "virot.valentin@gmail.com",
            Name = "Valentin Virot",
        },
    });
});

// Adding database context
builder.Services.AddDbContext<YuGamesContext>();

// Dependency injection
builder.Services.AddScoped<IPlayerBusiness, PlayerBusiness>();
builder.Services.AddScoped<IFifaGamePlayedBusiness, FifaGamePlayedBusiness>();
builder.Services.AddScoped<IPlayerRepository, SqLitePlayerRepository>();
builder.Services.AddScoped<IFifaGamePlayedRepository, SqLiteFifaGamePlayedRepository>();
builder.Services.AddScoped<ITeamPlayerRepository, SqLiteTeamPlayerRepository>();
builder.Services.AddScoped<IPlatformBusiness, PlatformBusiness>();
builder.Services.AddScoped<IPlatformRepository, SqLitePlatformRepository>();
builder.Services.AddScoped<IFifaTeamRepository, SqLiteFifaTeamRepository>();
builder.Services.AddScoped<IFifaTeamBusiness, FifaTeamBusiness>();
builder.Services.AddScoped<IHighlightRepository, SqLiteHightlightRepository>();
builder.Services.AddScoped<IAdminBusiness, AdminBusiness>();
builder.Services.AddScoped<IHighlightBusiness, HighlightBusiness>();
builder.Services.AddScoped<ISeasonBusiness, SeasonBusiness>();
builder.Services.AddScoped<ISeasonRepository, SqLiteSeasonRepository>();
builder.Services.AddScoped<ITournamentRepository, SqLiteTournamentRepository>();
builder.Services.AddScoped<ITournamentBusiness, TournamentBusiness>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
