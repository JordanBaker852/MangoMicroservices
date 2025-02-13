﻿using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mango.Services.AuthAPI.Service
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JWTOptions _jwtOptions;

        public JwtTokenGenerator(IOptions<JWTOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> userRoles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

            var claims = new List<Claim>()
            { 
                new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id),
                new Claim(JwtRegisteredClaimNames.Name, applicationUser.UserName)
            };

            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}