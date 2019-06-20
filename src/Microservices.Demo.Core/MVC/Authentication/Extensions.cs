using Microservices.Demo.Core.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Microservices.Demo.Core.MVC.Authentication
{
    public static class Extensions
    {
        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            JwtOptions options = new JwtOptions();
            IConfigurationSection section = configuration.GetSection("jwt");
            section.Bind(options);
            services.Configure<JwtOptions>(configuration.GetSection("jwt"));
            
            RSAParameters publicKeyParameters = DigitalSignatureManager.GetKey(options.PublicKey);
            RsaSecurityKey securityKey = new RsaSecurityKey(publicKeyParameters);


            TokenValidationParameters tokenValidationParameters = TokenManager.GetTokenValidationParameters(options.PublicKey, options.Issuer, options.Audience);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                     .AddJwtBearer(authorizationOptions =>
                     {
                         authorizationOptions.TokenValidationParameters = tokenValidationParameters;
                         authorizationOptions.Events = new JwtBearerEvents
                         {
                             OnAuthenticationFailed = context =>
                             {
                                 if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                                 {
                                     context.Response.Headers.Add("Token-Expired", "true");
                                 }
                                 return Task.CompletedTask;
                             }
                         };
                     });

            services.AddAuthorization(authorizationOptions =>
            {
                authorizationOptions.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
            });
        }
    }
}
