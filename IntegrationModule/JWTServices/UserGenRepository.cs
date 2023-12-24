using IntegrationModule.Models;
using IntegrationModule.REQModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IntegrationModule.Services
{
    public class UserGenRepository : IUserGenRepository
    {
        private readonly IConfiguration _configuration;

        public UserGenRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public User Add(UserRegisterReq request)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            string b64Salt = Convert.ToBase64String(salt);

            byte[] hash =
                KeyDerivation.Pbkdf2(
                    password: request.Password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8);
            string b64Hash = Convert.ToBase64String(hash);

            byte[] securityToken = RandomNumberGenerator.GetBytes(256 / 8);
            string b64SecToken = Convert.ToBase64String(securityToken);

            var newRegUser = new User
            {
                CreatedAt = DateTime.Now,
                Username = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                SecurityToken = b64SecToken,
                PwdSalt = b64Salt,
                PwdHash = b64Hash,
                Phone = request.Phone,
                IsConfirmed = false,
                CountryOfResidenceId = request.CountryOfResidenceId
            };
            return newRegUser;
        }

        public Tokens JwtTokens(JwtTokensReq request)
        {
            var jwtKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, request.Username),
                    new Claim(JwtRegisteredClaimNames.Sub, request.Username)
                }),
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Expires = DateTime.UtcNow.AddDays(120),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(jwtKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var serializedToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            return new Tokens
            {
                AccessToken = serializedToken
            };
        }
    }
}
