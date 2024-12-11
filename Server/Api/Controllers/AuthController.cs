using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataAccess.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<Player> userManager;
    private readonly IConfiguration configuration;

    public AuthController(UserManager<Player> userManager, IConfiguration configuration){
        this.userManager = userManager;
        this.configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto){
        if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password)){
            return BadRequest("Email and password must be provided.");
        }

    
        var user = await userManager.FindByEmailAsync(loginDto.Email);
        if (user == null || !await userManager.CheckPasswordAsync(user, loginDto.Password)){
            return Unauthorized("Invalid credentials.");
        }

        
        var roles = await userManager.GetRolesAsync(user);
        var token = GenerateJwtToken(user, roles);

        return Ok(new { Token = token });
    }

    private string GenerateJwtToken(Player user, IList<string> roles)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"])),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}


