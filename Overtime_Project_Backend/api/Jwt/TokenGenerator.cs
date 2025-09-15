using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


namespace api.JWTToken
{
    public class TokenCreator
    {
        private readonly IConfiguration _config;

        public TokenCreator(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(string username)
        {
            return GenerateJwtToken(username); 
        }

        private string GenerateJwtToken(string username)
        {

            var jwtKey = _config["Jwt:Key"];
            var jwtIssuer = _config["Jwt:Issuer"];
            var jwtAudience = _config["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            switch (username.ToLower())
            {
                case "employee":
                    claims.Add(new Claim(ClaimTypes.Role, "Employee"));
                    break;
                case "manager":
                    claims.Add(new Claim(ClaimTypes.Role, "Manager"));
                    break;
                case "people_ops":
                    claims.Add(new Claim(ClaimTypes.Role, "People_Ops"));
                    break;
                case "payroll]":
                    claims.Add(new Claim(ClaimTypes.Role, "Payroll"));
                    break;
                default:
                    claims.Add(new Claim(ClaimTypes.Role, "Guest"));
                    break;
            }

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}