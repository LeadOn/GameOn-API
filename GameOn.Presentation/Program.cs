// <copyright file="Program.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

#pragma warning disable SA1200 // Using directives should be placed correctly
using GameOn.Application;
using GameOn.Common.Exceptions;
using GameOn.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
#pragma warning restore SA1200 // Using directives should be placed correctly

var builder = WebApplication.CreateBuilder(args);

// Adding all layers
builder.Services.AddPersistence();
builder.Services.AddApplication();

// Setting up ASP.NET
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "4.2.1",
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

var app = builder.Build();

// Configuring SwaggerGen
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});

// Using CORS
app.UseCors();

// Configuring authentication
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
