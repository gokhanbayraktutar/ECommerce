using ECommerce.Application.Interfaces;
using ECommerce.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _config;

    public AuthController(IUserService userService, IConfiguration config)
    {
        _userService = userService;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        var existingUser = await _userService.GetByUsernameAsync(user.Username);
        if (existingUser != null) return BadRequest("Username already exists.");

        // Basit hash örneği (gerçek projede BCrypt veya Argon2 kullan)
        user.PasswordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.PasswordHash));
        await _userService.AddAsync(user);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User login)
    {
        var user = await _userService.GetByUsernameAsync(login.Username);
        if (user == null) return Unauthorized();

        var passwordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(login.PasswordHash));
        if (user.PasswordHash != passwordHash) return Unauthorized();

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Ok(new { Token = tokenHandler.WriteToken(token) });
    }
}
