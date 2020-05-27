using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Charcutarie.Api.Infra;
using Charcutarie.Core.JWT;
using Charcutarie.Models.ViewModels;
using Charcutarie.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Charcutarie.Api.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly AuthSettings signingConfigurations;
        private readonly JwtIssuerOptions tokenConfigurations;
        public AuthenticationController(IAccountService accountService, IOptionsMonitor<AuthSettings> authOptions, IOptionsMonitor<JwtIssuerOptions> tokenOptions)
        {
            this.accountService = accountService;
            signingConfigurations = authOptions.CurrentValue;
            tokenConfigurations = tokenOptions.CurrentValue;
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> GetToken([FromBody]Login request)
        {
            var userData = await accountService.DoLogin(request.CorpClientId, request.Username, request.Password);
            if (userData == null)
            {
                return Ok(new TokenData
                {
                    Authenticated = false,
                    Message = "Usuário ou senha inválido"
                });
            }
            if (!userData.Active)
            {
                return Ok(new TokenData
                {
                    Authenticated = false,
                    Message = "Usuário Inativo"
                });
            }
            if(signingConfigurations.ClientSecret != request.ClientSecret)
                return Ok(new TokenData
                {
                    Authenticated = false,
                    Message = "Ocorreu um problema na autenticação. Entre em contato com o administrador do sistema."
                });

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(ClaimTypes.Role, Constants.JWTAllowedRoles[0]),
                new Claim(nameof(userData.CorpClientId), userData.CorpClientId?.ToString()),
                new Claim(nameof(userData.DBAName), userData.DBAName),
                new Claim(nameof(userData.CompanyName), userData.CompanyName),
                new Claim(nameof(userData.Username), userData.Username),
                new Claim(nameof(userData.UserId), userData.UserId.ToString())
            };
            var creationDate = DateTime.Now;
            var expirationDate = creationDate.AddMinutes(tokenConfigurations.Minutes);
            var tokenString = GenerateToken(claims, expirationDate);
            var newRefreshToken = GenerateRefreshToken();
            await accountService.SaveRefreshToken(userData.UserId, newRefreshToken, creationDate);
            return Ok(new TokenData
            {
                UserData = userData,
                Authenticated = true,
                Created = creationDate,
                Expiration = expirationDate,
                AccessToken = tokenString,
                RefreshToken = newRefreshToken,
                Message = "OK"
            });

        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost("Token/Refresh")]
        public async Task<IActionResult> Refresh([FromBody]RequestTokenRefresh token)
        {
            var principal = GetPrincipalFromExpiredToken(token.Token);
            var userid = Convert.ToInt64(principal.Claims.First(c => c.Type == "UserId").Value);

            var savedRefreshToken = await accountService.GetRefreshToken(userid);
            //if (savedRefreshToken != token.RefreshToken)
            //    throw new SecurityTokenException("Invalid refresh token");

            var creationDate = DateTime.Now;
            var expirationDate = creationDate.AddMinutes(tokenConfigurations.Minutes);
            var newJwtToken = GenerateToken(principal.Claims, expirationDate);
            var newRefreshToken = GenerateRefreshToken();
            await accountService.SaveRefreshToken(userid, newRefreshToken, creationDate);

            return new ObjectResult(new TokenData
            {
                Authenticated = true,
                Created = creationDate,
                Expiration = expirationDate,
                AccessToken = newJwtToken,
                RefreshToken = newRefreshToken,
                Message = "OK"
            });
        }

        private string GenerateToken(IEnumerable<Claim> claims, DateTime expirationDate)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingConfigurations.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                tokenConfigurations.Issuer,
                tokenConfigurations.Audience,
                expires: expirationDate,
                signingCredentials: creds,
                claims: claims);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingConfigurations.SecretKey)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}