using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Behoof.Core.JwtTokenSetting;
using Microsoft.IdentityModel.Tokens;

namespace Behoof.Middleware.Jwt
{
    public class JwtSecurity
    {
        private RequestDelegate _next;

        public JwtSecurity(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Cookies["JwtToken"];

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    ClaimsPrincipal claimsPrincipal = await ValidateToken(token);
                    context.User = claimsPrincipal;
                    context.Request?.Headers?.Add("Authorization", $"Bearer {token}");
                }
                catch (SecurityTokenExpiredException ex)
                {
                    context.Response.Cookies.Delete("JwtToken");
                }
            }

            await _next.Invoke(context);
        }


        public async Task<ClaimsPrincipal> ValidateToken(string token)
        {
            return new ClaimsPrincipal(new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = AuthOption.Issuer,
                ValidAudience = AuthOption.Audience,
                IssuerSigningKey = AuthOption.GetSymmetricSecurityKey()
            }, out SecurityToken securityToken));
        }
    }
}
