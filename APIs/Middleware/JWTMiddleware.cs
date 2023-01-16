using BusinessLayer.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Middleware
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public JWTMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                AttachAccountToContext(context, token);
            }
            await _next(context);
        }

        private void AttachAccountToContext(HttpContext context, string token)
        {
            try
            {

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.ASCII.GetBytes(_config["AppSettings:Key"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
                //Guid accountId = Guid.Parse(jwtToken.Claims.First(x => x.Type == CustomClaim.USER_ID).Value);

                //Check for Token expiration
                long tokenExpiry = long.Parse(jwtToken.Claims.First(x => x.Type == "exp").Value);
                long currentTimeInSeconds = GetCurrentUnixTimestampSeconds(DateTime.Now);
                if (currentTimeInSeconds > tokenExpiry)
                {
                    throw new Exception("Forbidden...Token has expired");
                }

                // attach account to context on successful jwt validation
                //context.Items["UserId"] = accountId;
                context.Items["Token"] = token;
                context.Items["Role"] = jwtToken.Claims.First(x => x.Type == CustomClaim.USER_ROLE).Value;
                var tt = jwtToken.Claims.First(x => x.Type == CustomClaim.USER_ROLE).Value;
                context.Items["TokenExpirationDateInSeconds"] = tokenExpiry;
                context.Items["CurrentTimeInSconds"] = currentTimeInSeconds;

            }
            catch (Exception)
            {
                // do nothing if jwt validation fails
                // account is not attached to context so request won't have access to secure routes
            }
        }

        private static long GetCurrentUnixTimestampSeconds(DateTime localDateTime)
        {
            DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime univDateTime;
            univDateTime = localDateTime.ToUniversalTime();
            return (long)(univDateTime - UnixEpoch).TotalSeconds;
        }
    }
}
