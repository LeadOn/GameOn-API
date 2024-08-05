// <copyright file="AuthController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using GameOn.Application.Players.Commands.RegisterPlayer;
    using GameOn.Application.Players.Queries.GetPlayerByEmail;
    using GameOn.Common.DTOs;
    using GameOn.Common.Exceptions;
    using GameOn.Domain;
    using GameOn.Presentation.Classes;
    using MediatR;
    using Microsoft.AspNetCore.Identity.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    /// AuthController class.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Configuration interface.
        private readonly IConfiguration configuration;

        // Mediator interface.
        private readonly ISender mediator;

        // dictionary to store refresh tokens.
#pragma warning disable SA1204 // Static elements should appear before instance elements
        private static Dictionary<string, string> refreshTokens = new Dictionary<string, string>();
#pragma warning restore SA1204 // Static elements should appear before instance elements

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="config">Configuration interface, injected.</param>
        /// <param name="mediator">Mediator interface, injected.</param>
        public AuthController(IConfiguration config, ISender mediator)
        {
            this.configuration = config;
            this.mediator = mediator;
        }

        /// <summary>
        /// Register new user in database.
        /// </summary>
        /// <param name="newPlayer">New player.</param>
        /// <returns>New player in database.</returns>
        /// <exception cref="MissingEnvironmentVariableException">When some env variables are missing.</exception>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto newPlayer)
        {
            var player = new Player
            {
                Email = newPlayer.Email,
                Nickname = newPlayer.Nickname,
                PasswordSalt = AuthService.GenerateSalt(),
            };

            player.PasswordHash = AuthService.ComputeHash(newPlayer.Password, player.PasswordSalt, Environment.GetEnvironmentVariable("PASSWORD_PEPPER") ?? throw new MissingEnvironmentVariableException("PASSWORD_PEPPER"), int.Parse(Environment.GetEnvironmentVariable("PASSWORD_ITERATIONS") ?? throw new MissingEnvironmentVariableException("PASSWORD_ITERATIONS")));

            // Checking if user already exists
            var playerInDb = await this.mediator.Send(new GetPlayerByEmailQuery { Email = newPlayer.Email });

            if (playerInDb is not null)
            {
                return this.Conflict();
            }

            return this.Ok(await this.mediator.Send(new RegisterPlayerCommand { Player = player }));
        }

        /// <summary>
        /// Login route.
        /// </summary>
        /// <param name="model"><see cref="UserLoginDto"/>.</param>
        /// <returns><see cref="IActionResult" />.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto model)
        {
            // Getting user from email
            var userInDb = await this.mediator.Send(new GetPlayerByEmailQuery { Email = model.Email });

            if (userInDb is null)
            {
                return this.Unauthorized();
            }

            // Calculating password hash
            var passwordHash = AuthService.ComputeHash(model.Password, userInDb.PasswordSalt, Environment.GetEnvironmentVariable("PASSWORD_PEPPER") ?? throw new MissingEnvironmentVariableException("PASSWORD_PEPPER"), int.Parse(Environment.GetEnvironmentVariable("PASSWORD_ITERATIONS") ?? throw new MissingEnvironmentVariableException("PASSWORD_ITERATIONS")));

            if (userInDb.PasswordHash != passwordHash)
            {
                return this.Unauthorized();
            }
            else
            {
                var token = this.GenerateAccessToken(model.Email);

                // return access token and refresh token
                return this.Ok(new
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                });
            }
        }

        /// <summary>
        /// Generates an Access token for given user.
        /// </summary>
        /// <param name="email">User email.</param>
        /// <returns><see cref="JwtSecurityToken"/>.</returns>
        /// <exception cref="MissingEnvironmentVariableException"><see cref="MissingEnvironmentVariableException"/>.</exception>
        private JwtSecurityToken GenerateAccessToken(string email)
        {
            // Create user claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
            };

            // Create a JWT
            var token = new JwtSecurityToken(
                issuer: Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new MissingEnvironmentVariableException("JWT_ISSUER"),
                audience: Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new MissingEnvironmentVariableException("JWT_AUDIENCE"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60), // Token expiration time
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRETKEY") ?? throw new MissingEnvironmentVariableException("JWT_SECRETKEY"))),
                    SecurityAlgorithms.HmacSha256));

            return token;
        }
    }
}
