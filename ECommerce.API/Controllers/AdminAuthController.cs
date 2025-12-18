using ECommerce.Application.DTO;
using ECommerce.Application.Interfaces;
using ECommerce.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/admin-auth")]
public class AdminAuthController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly IConfiguration _config;

    public AdminAuthController(IAdminService adminService, IConfiguration config)
    {
        _adminService = adminService;
        _config = config;
    }

    private string GenerateAdminJwt(Admin admin)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                new Claim(ClaimTypes.Name, admin.Username),
                new Claim(ClaimTypes.Role, "Admin") 
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AdminLoginRequestDTO request)
    {
        var admin = await _adminService.GetByUsernameAsync(request.Username);
        if (admin == null)
            return Unauthorized("Admin not found");

        var passwordHash = Convert.ToBase64String(
            Encoding.UTF8.GetBytes(request.Password)
        );

        if (admin.PasswordHash != passwordHash)
            return Unauthorized("Invalid password");

        var token = GenerateAdminJwt(admin);

        return Ok(new
        {
            token,
            admin.Username,
            admin.Email
        });
    }

}
