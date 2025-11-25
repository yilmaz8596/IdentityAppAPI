using API.Utility;
using IdentityAppAPI.Models;
using IdentityAppAPI.Services.IServices;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityAppAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration; 
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration; 
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        }
        public string CreateJWT(AppUser user)
        {
            var userClaims = new List<Claim>
            {
                new Claim(SD.UserId, user.Id.ToString()),
                new Claim(SD.Name,user.Name),
                new Claim(SD.UserName, user.UserName),
                new Claim(SD.Email, user.Email)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.Now.AddDays(int.Parse(_configuration["JWT:ExpiryInDays"])),
                SigningCredentials = creds, 
                Issuer = _configuration["JWT:Issuer"],
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(jwt);
        }
    }
}
