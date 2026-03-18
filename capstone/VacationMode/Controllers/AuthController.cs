using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using VacationMode.Data;
using VacationMode.DTOs;
using VacationMode.Models;

namespace VacationMode.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterDto dto)
    {
        // Check for duplicate email (case-insensitive)
        var exists = _context.Users.Any(u => u.Email.ToLower() == dto.Email.ToLower());
        if (exists)
            return BadRequest("An account with this email already exists.");

        var passwordHash = Convert.ToBase64String(
            SHA256.HashData(Encoding.UTF8.GetBytes(dto.Password))
        );

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email.ToLower().Trim(), // normalize email
            PhoneNumber = dto.PhoneNumber,
            Role = dto.Role,
            PasswordHash = passwordHash
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok("User registered successfully");
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        // Case-insensitive email lookup
        var emailLower = dto.Email.ToLower().Trim();
        var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == emailLower);

        if (user == null)
            return Unauthorized("Invalid email or password.");

        var passwordHash = Convert.ToBase64String(
            SHA256.HashData(Encoding.UTF8.GetBytes(dto.Password))
        );

        if (user.PasswordHash != passwordHash)
            return Unauthorized("Invalid email or password.");

        var token = CreateToken(user);

        // Return just "token" and let the global camelCase policy handle casing
        return Ok(new { token });
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role ?? "User")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
