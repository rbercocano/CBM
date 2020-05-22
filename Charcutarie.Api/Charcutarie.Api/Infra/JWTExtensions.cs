using Charcutarie.Core.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Text;

namespace Charcutarie.Api.Infra
{
    public static class JWTExtensions
    {
        public static void ConfigureJWT(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            var authSettings = configuration.GetSection(nameof(AuthSettings));
            services.Configure<AuthSettings>(authSettings);

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSettings[nameof(AuthSettings.SecretKey)]));
            var jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                options.Minutes = Convert.ToInt32(jwtAppSettingOptions[nameof(JwtIssuerOptions.Minutes)]);
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(bearerOptions =>
              {
                  bearerOptions.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],
                      ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],
                      IssuerSigningKey = signingKey
                  };
              });
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("ApiAccess", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireClaim(ClaimTypes.Role, Constants.JWTAllowedRoles)
                    .RequireAuthenticatedUser().Build());
            });

        }
    }
}
