// <copyright file="AuthController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using GameOn.Common.DTOs;
    using GameOn.Common.Exceptions;
    using Microsoft.AspNetCore.Identity.Data;
    using Microsoft.AspNetCore.Mvc;
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

        // dictionary to store refresh tokens.
#pragma warning disable SA1204 // Static elements should appear before instance elements
        private static Dictionary<string, string> refreshTokens = new Dictionary<string, string>();
#pragma warning restore SA1204 // Static elements should appear before instance elements

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="config">Configuration interface, injected.</param>
        public AuthController(IConfiguration config)
        {
            this.configuration = config;
        }

        /// <summary>
        /// Login route.
        /// </summary>
        /// <param name="model"><see cref="UserLoginDto"/>.</param>
        /// <returns><see cref="IActionResult" />.</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto model)
        {
            // Check user credentials (in a real application, you'd authenticate against a database)
            if (model is { Username: "demo", Password: "password" })
            {
                var token = this.GenerateAccessToken(model.Username);

                // Generate refresh token
                var refreshToken = Guid.NewGuid().ToString();

                // Store the refresh token (in-memory for simplicity)
                refreshTokens[refreshToken] = model.Username;

                // return access token and refresh token
                return this.Ok(new
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                });
            }

            // unauthorized user
            return this.Unauthorized("Invalid credentials!");
        }

        /// <summary>
        /// Refreshes the access token.
        /// </summary>
        /// <param name="request"><see cref="RefreshRequestDto" />.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshRequestDto request)
        {
            if (refreshTokens.TryGetValue(request.RefreshToken, out var userId))
            {
                // Generate a new access token
                var token = this.GenerateAccessToken(userId);

                // Return the new access token to the client
                return this.Ok(new { AccessToken = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return this.BadRequest("Invalid refresh token");
        }

        /// <summary>
        /// Generates an Access token for given user.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <returns><see cref="JwtSecurityToken"/>.</returns>
        /// <exception cref="MissingEnvironmentVariableException"><see cref="MissingEnvironmentVariableException"/>.</exception>
        private JwtSecurityToken GenerateAccessToken(string userName)
        {
            // Create user claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
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
