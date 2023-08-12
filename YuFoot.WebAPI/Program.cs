// <copyright file="Program.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Versioning;
using YuFoot.Business;
using YuFoot.Business.Contracts;
using YuFoot.Common.Exceptions;
using YuFoot.EntitiesContext;
using YuFoot.Repository;
using YuFoot.Repository.Contracts;
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

// Adding API Versionning
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
        Version = "1.0",
        Title = "LeadOn's Corp - YuFoot API",
        Description = "This API goal is to monitor Soccer players performance across real-world and virtual Soccer games.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Email = "virot.valentin@gmail.com",
            Name = "Valentin Virot",
        },
    });
});

// Adding database context
builder.Services.AddDbContext<YuFootContext>();

// Dependency injection
builder.Services.AddScoped<IPlayerBusiness, PlayerBusiness>();
builder.Services.AddScoped<IGamePlayedBusiness, GamePlayedBusiness>();
builder.Services.AddScoped<IPlayerRepository, SqLitePlayerRepository>();
builder.Services.AddScoped<IGamePlayedRepository, SqLiteGamePlayedRepository>();
builder.Services.AddScoped<ITeamPlayerRepository, SqLiteTeamPlayerRepository>();
builder.Services.AddScoped<IPlatformBusiness, PlatformBusiness>();
builder.Services.AddScoped<IPlatformRepository, SqLitePlatformRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
