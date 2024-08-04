// <copyright file="Program.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

#pragma warning disable SA1200 // Using directives should be placed correctly
using System.Text;
using GameOn.Application;
using GameOn.Common.Exceptions;
using GameOn.External;
using GameOn.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
#pragma warning restore SA1200 // Using directives should be placed correctly

var builder = WebApplication.CreateBuilder(args);

// Adding all layers
builder.Services.AddPersistence();
builder.Services.AddExternal();
builder.Services.AddApplication();

// Setting up ASP.NET
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "3.0",
        Title = "LeadOn's Corp - GameOn! API",
        Description = "This API goal is to monitor players performance across multiple games.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Email = "virot.valentin@gmail.com",
            Name = "Valentin Virot",
        },
    });
});

// Setting CORS policy
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
{
    policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
}));

// Adding authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new MissingEnvironmentVariableException("JWT_ISSUER"),
        ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new MissingEnvironmentVariableException("JWT_AUDIENCE"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRETKEY") ?? throw new MissingEnvironmentVariableException("JWT_SECRETKEY"))),
    };
});

var app = builder.Build();

// Configuring SwaggerGen
app.UseSwagger();
app.UseSwaggerUI();

// Using CORS
app.UseCors();

// Configuring authentication
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
