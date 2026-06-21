using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config) => _config = config;

        public string CreateToken(User user)
        {
            // 1) 설정값 읽기
            var key = _config["Jwt:Key"]!;
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var expiryMinutes = int.Parse(_config["Jwt:ExpiryMinutes"]!);

            // 2) Payload에 담을 claims (★ UserId가 여기!)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("displayName", user.DisplayName),
            };

            // 3) 비밀키로 서명 자격 생성
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 4) 토큰 조립
            var token = new JwtSecurityToken(
                issuer : issuer,
                audience : audience,
                claims : claims,
                expires : DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials : credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
