using ECommerceAPI.Application.DTO;
using ECommerceAPI.Application.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Token
{
    public class TokenHandler : ITokenHandler
    {
        readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Application.DTO.Token CreateAccessToken(int expirationMinute)
        {
            Application.DTO.Token token = new Application.DTO.Token();

            //security key elde edildi
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SignInKey"]));
            //şifrelenmiş kimliği oluştur
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);
            //Token ayarları
            token.Expiration = DateTime.UtcNow.AddMinutes(expirationMinute);
            JwtSecurityToken tokenSecurity = new JwtSecurityToken(
                audience : _configuration["Token:Audience"],
                issuer : _configuration["Token:Issuer"],
                expires : token.Expiration,
                //token ne zaman devreye girecek
                notBefore : DateTime.UtcNow,
                signingCredentials : credentials
                );
            
            //token oluşturucu
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            token.AccessToken = tokenHandler.WriteToken(tokenSecurity);
            return token;
        }
    }
}
