﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AgendaxApi.Entities;
using AgendaxApi.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace AgendaxApi.Servises
{
	public class TokenService : ITokenServise
	{
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Tokenkey"]));
        }

        public string CreateToke(User user)
        {
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.NameId , user.UserName)
            };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

