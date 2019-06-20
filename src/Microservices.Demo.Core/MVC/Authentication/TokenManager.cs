using Microservices.Demo.Core.Cryptography;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Microservices.Demo.Core.MVC.Authentication
{
    public static class TokenManager
    {
        public static object GenerateToken(IConnectionMultiplexer connectionMultiplexer, string cacheKey, IEnumerable<Claim> claims, string key, string issuer = null, string audience = null, double? expires = null)
        {
            RSAParameters privateKeyParameters = DigitalSignatureManager.GetKey(key);
            RsaSecurityKey privateKey = new RsaSecurityKey(privateKeyParameters);

            SigningCredentials credentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);

            JwtSecurityToken securityToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires.HasValue ? (DateTime?)DateTime.UtcNow.AddMinutes(expires.Value) : null,
                signingCredentials: credentials);

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            string refreshToken = SaltGenerator.Generate();

            IDatabase database = connectionMultiplexer.GetDatabase(0);

            database.StringSet(cacheKey, refreshToken);

            return new
            {
                access_token = token,
                expiration = securityToken.ValidTo,
                refresh_token = refreshToken
            };
        }

        public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string publicKey, string issuer = null, string audience = null)
        {
            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters(publicKey, issuer, audience);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.RsaSha256, StringComparison.OrdinalIgnoreCase))
            {
                throw new SecurityTokenException();
            }

            return principal;
        }

        public static TokenValidationParameters GetTokenValidationParameters(string publicKey, string issuer = null, string audience = null)
        {
            RSAParameters publicKeyParameters = DigitalSignatureManager.GetKey(publicKey);
            RsaSecurityKey securityKey = new RsaSecurityKey(publicKeyParameters);

            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,

                ValidateIssuer = !string.IsNullOrWhiteSpace(issuer),
                ValidIssuer = issuer,

                ValidateAudience = !string.IsNullOrWhiteSpace(audience),
                ValidAudience = audience,

                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };
        }
        public static object RefreshToken(IConnectionMultiplexer connectionMultiplexer, string token, string refreshToken, string privateKey, string publicKey, string issuer = null, string audience = null, double? expires = null)
        {
            ClaimsPrincipal principal = GetPrincipalFromExpiredToken(token, publicKey, issuer, audience);

            string ip = principal.Claims.SingleOrDefault(x => x.Type == "ip").Value;
            string customerCode = principal.Claims.SingleOrDefault(x => x.Type == "customerCode").Value;
            string userName = principal.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            string cacheKey = $"{customerCode}-{userName}-{ip}";

            IDatabase database = connectionMultiplexer.GetDatabase(0);

            var savedRefreshToken = database.StringGet(cacheKey);
            if (savedRefreshToken != refreshToken)
            {
                throw new SecurityTokenException();
            }

            RSAParameters privateKeyParameters = DigitalSignatureManager.GetKey(privateKey);
            RsaSecurityKey securityKey = new RsaSecurityKey(privateKeyParameters);

            //var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);

            var newSecurityToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: principal.Claims,
                expires: expires.HasValue ? (DateTime?)DateTime.UtcNow.AddMinutes(expires.Value) : null,
                signingCredentials: credentials);

            string newToken = new JwtSecurityTokenHandler().WriteToken(newSecurityToken);
            string newRefreshToken = SaltGenerator.Generate();

            database.StringSet(cacheKey, newRefreshToken);

            return new
            {
                access_token = newToken,
                expiration = newSecurityToken.ValidTo,
                refresh_token = newRefreshToken
            };
        }

    }
}
