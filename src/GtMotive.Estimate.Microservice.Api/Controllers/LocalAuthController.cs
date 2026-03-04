using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    /// <summary>
    /// Local identity auth emulator: issues JWT tokens for development/Swagger when the external identity URL is unavailable.
    /// Single hardcoded user (in-memory) for Swagger; keep external JwtAuthority config for when it is fixed.
    /// </summary>
    [ApiController]
    [Route("connect")]
    [AllowAnonymous]
    public class LocalAuthController(IConfiguration config) : ControllerBase
    {
        private const string LocalClientId = "client-gtestimate-swagger";
        private const string LocalClientSecret = "gtmotive";
        private const string ClientCredentialsGrantType = "client_credentials";

        private readonly IConfiguration _config = config ?? throw new ArgumentNullException(nameof(config));

        /// <summary>
        /// Local token endpoint (emulates /connect/token). Accepts OAuth2 client credentials grant.
        /// Use client_id "client-gtestimate-swagger" and client_secret "gtmotive" for Swagger Try It.
        /// </summary>
        /// <param name="grantType">OAuth2 grant type (use "client_credentials" for local auth).</param>
        /// <param name="clientId">Client id (use "client-gtestimate-swagger" for Swagger).</param>
        /// <param name="clientSecret">Client secret (use "gtmotive" for Swagger).</param>
        /// <returns>Token response with access_token, or error.</returns>
        [HttpPost("token")]
        [Consumes("application/x-www-form-urlencoded")]
        [ProducesResponseType(typeof(LocalTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult ConnectToken(
            [FromForm(Name = "grant_type")] string grantType,
            [FromForm(Name = "client_id")] string clientId,
            [FromForm(Name = "client_secret")] string clientSecret)
        {
            if (string.IsNullOrEmpty(grantType) || !string.Equals(grantType, ClientCredentialsGrantType, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { error = "unsupported_grant_type", error_description = "Only grant_type=client_credentials is supported for local auth." });
            }

            // Swagger UI sends client credentials in Authorization: Basic header, not in form body
            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            {
                var (headerClientId, headerClientSecret) = TryGetCredentialsFromAuthorizationHeader();
                clientId ??= headerClientId;
                clientSecret ??= headerClientSecret;
            }

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            {
                return BadRequest(new { error = "invalid_request", error_description = "client_id and client_secret are required (form body or Basic Authorization header)." });
            }

            if (!string.Equals(clientId, LocalClientId, StringComparison.Ordinal) ||
                !string.Equals(clientSecret, LocalClientSecret, StringComparison.Ordinal))
            {
                return Unauthorized(new { error = "invalid_grant", error_description = "Invalid client_id or client_secret." });
            }

            var issuer = _config["Jwt:Issuer"];
            var key = _config["Jwt:Key"];
            var testUserId = _config["Jwt:TestUserId"] ?? "user-test-001";
            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(key))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "server_error", error_description = "Jwt:Issuer and Jwt:Key must be configured." });
            }

            var claims = new[]
            {
                new Claim("sub", testUserId),
                new Claim(ClaimTypes.Name, "Swagger Client"),
                new Claim("scope", "estimate-public-scope")
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: "estimate-api",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new LocalTokenResponse
            {
                AccessToken = accessToken,
                TokenType = "Bearer",
                ExpiresIn = 3600
            });
        }

        private (string ClientId, string ClientSecret) TryGetCredentialsFromAuthorizationHeader()
        {
            var authHeader = Request.Headers.Authorization.FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return (null, null);
            }

            try
            {
                var base64 = authHeader["Basic ".Length..].Trim();
                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
                var colonIndex = decoded.IndexOf(':', StringComparison.Ordinal);
                if (colonIndex < 0)
                {
                    return (null, null);
                }

                var id = decoded[..colonIndex];
                var secret = decoded[(colonIndex + 1)..];
                return (id, secret);
            }
            catch (FormatException)
            {
                return (null, null);
            }
        }

        private sealed class LocalTokenResponse
        {
            [System.Text.Json.Serialization.JsonPropertyName("access_token")]
            public string AccessToken { get; set; }

            [System.Text.Json.Serialization.JsonPropertyName("token_type")]
            public string TokenType { get; set; }

            [System.Text.Json.Serialization.JsonPropertyName("expires_in")]
            public int ExpiresIn { get; set; }
        }
    }
}
