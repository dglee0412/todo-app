using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Dtos;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly PasswordHasher<User> _hasher = new();
        private readonly TokenService _tokenService;

        public AuthController(AppDbContext db, TokenService tokenService)
        {
            _db = db;
            _tokenService = tokenService;
        }

        //POST /api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            // 1) 이메일 중복 체크
            var exists = await _db.Users.AnyAsync(u => u.Email == dto.Email);
            if (exists)
                return Conflict(new { message = "이미 사용 중인 이메일입니다." });

            // 2) User 객체 생성
            var user = new User
            {
                Email = dto.Email,
                DisplayName = dto.DisplayName,
                CreatedAt = DateTime.UtcNow
            };

            // 3) 비밀번호 해싱 → PasswordHash에 저장
            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            // 4) DB 저장
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var token = _tokenService.CreateToken(user); // ← 가입 즉시 토큰 발급

            // 5) 응답 (토큰은 3단계에서 채움 — 지금은 빈 문자열)
            return Ok(new AuthResponseDto(user.Id, user.Email, user.DisplayName, token));
        }

        //Post /api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            // 1) 이메일로 사용자 찾기
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return Unauthorized(new { message = "이메일 또는 비밀번호가 올바르지 않습니다." });

            // 2) 비밀번호 검증
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized(new { message = "이메일 또는 비밀번호가 올바르지 않습니다." });

            // 3) 통과 → 토큰 발급
            var token = _tokenService.CreateToken(user);
            return Ok(new AuthResponseDto(user.Id, user.Email, user.DisplayName, token));
        }
    }
}
