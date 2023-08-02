// <copyright file="Program.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

#pragma warning disable SA1200 // Using directives should be placed correctly
using YuFoot.Business;
using YuFoot.Business.Contracts;
using YuFoot.Repository;
using YuFoot.Repository.Contracts;
#pragma warning restore SA1200 // Using directives should be placed correctly

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
{
    policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
}));

builder.Services.AddControllers();

// Used for Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "0.2",
        Title = "LeadOn's Corp - YuFoot API",
        Description = "This API goal is to monitor Yunit's Soccer team players performance accross real-world and virtual Soccer games.",
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
