using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    public string AdminToken { get; private set; }
    public string PlayerToken { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder){
        builder.UseEnvironment("Test");
        builder.ConfigureServices(async services =>
        {
            // Remove existing database registration
            var npgsqlDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (npgsqlDescriptor != null)
            {
                services.Remove(npgsqlDescriptor);
            }

            // Add an in-memory database 
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryTestDb");
            });

            var serviceProvider = services.BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();  

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Player>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

                // Seed roles and users
                string[] roles = new[] { "Admin", "Player" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole<Guid> { Name = role });
                    }
                }

                var adminEmail = "admin@example.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new Player
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true,
                        Isadmin = true
                    };

                    var result = await userManager.CreateAsync(adminUser, "Admin@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }

                

                var playerEmail = "player@example.com";
                var playerUser = await userManager.FindByEmailAsync(playerEmail);
                 if (playerUser == null){
                    playerUser = new Player{
                        UserName = playerEmail,
                        Email = playerEmail,
                        EmailConfirmed = false,
                        Isadmin = false
                    };

                    var result = await userManager.CreateAsync(playerUser, "Player@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(playerUser, "Player");
                    }
                }

                // Generate Admin  and player tokens after theyre created
                AdminToken = GenerateJwtToken(adminUser.Id.ToString(), adminEmail, true, "Admin");
                PlayerToken = GenerateJwtToken(playerUser.Id.ToString(), playerEmail, false, "Player");



            }
        });
    }

    public string GenerateJwtToken(string userId, string email, bool isAdmin, string role)
    {
        var jwtSettings = new
        {
            Key = "zTDROdJ32D1LMf5DMQgIkdbznKYwIUH3",
            Issuer = "Exam2024",
            Audience = "AppUsers"
        };

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim("IsAdmin", isAdmin.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            jwtSettings.Issuer,
            jwtSettings.Audience,
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}

