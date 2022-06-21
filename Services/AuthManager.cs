using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using nyschub.Contracts;
using nyschub.DTO;
using nyschub.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace nyschub.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<Corper> _userManager;
        private readonly IConfiguration _configuration;
        private Corper _user;

        public AuthManager(UserManager<Corper> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<string> CreateToken(Corper corper)
        {
            var signingCredential = GetSigningCredential();
            var claims = await GetClaims(corper);
            var tokenOptions = GenerateTokenOptions(signingCredential, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private async Task<List<Claim>> GetClaims(Corper corper)
        {
            var claims = new List<Claim>
            {
                new Claim("id", corper.Id),
                new Claim(ClaimTypes.Name, corper.UserName),
                new Claim(ClaimTypes.Email, corper.Email),
            };

            var roles = await _userManager.GetRolesAsync(corper);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            };

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredential, List<Claim> claims)
        {
            
            var expiration = DateTime.Now.AddHours(Convert.ToDouble(Environment.GetEnvironmentVariable("JwtLifetime")));
            
             
            var token = new JwtSecurityToken(
                issuer: Environment.GetEnvironmentVariable("JwtIssuer"),
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredential
                );

            return token;
        }

        private SigningCredentials GetSigningCredential()
        {
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JwtKey"));
            Console.WriteLine(key);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public async Task<bool> ValidateUser(LoginDto userDto)
        {
            _user = await _userManager.FindByNameAsync(userDto.Email);
            return (_user != null && await _userManager.CheckPasswordAsync(_user, userDto.Password));
        }
    }
}
