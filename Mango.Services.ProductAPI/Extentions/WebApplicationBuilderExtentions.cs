using Mango.Services.ProductAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Mango.Services.ProductAPI.Extentions
{
    public static class WebApplicationBuilderExtentions
    {
        public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder)
        {
            var jwtOptions = builder.Configuration.GetSection("ApiSettings:JWTOptions").Get<JWTOptions>();

            if (jwtOptions != null)
            {
                var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);

                builder.Services.AddAuthentication(authOption =>
                {
                    authOption.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authOption.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(jwtBearerOption =>
                {
                    jwtBearerOption.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience
                    };
                });
            }

            return builder;
        }
    }
}
