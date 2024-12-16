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
using Microsoft.Extensions.Configuration;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private readonly IConfiguration _configuration;

    public string AdminToken { get; private set; }
    public string PlayerToken { get; private set; }

    // Constructor to inject configuration
    public CustomWebApplicationFactory()
    {
        _configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables() // Add environment variables
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
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

                await SeedRoles(roleManager);
                await SeedUsers(userManager);
                await SeedSampleEntities(dbContext);

                await dbContext.SaveChangesAsync();
            }
        });
    }

    private async Task SeedRoles(RoleManager<IdentityRole<Guid>> roleManager)
    {
        string[] roles = { "Admin", "Player" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid> { Name = role });
            }
        }
    }

    private async Task SeedUsers(UserManager<Player> userManager)
    {
        // Create Admin
        var adminEmail = "admin@example.com";
        var adminUser = new Player
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            Isadmin = true
        };
        await CreateUserIfNotExists(userManager, adminUser, "Admin@123", "Admin");
        AdminToken = GenerateJwtToken(adminUser.Id.ToString(), adminEmail, true, "Admin");

        // Create Player
        var playerEmail = "player@example.com";
        var playerUser = new Player
        {
            UserName = playerEmail,
            Email = playerEmail,
            EmailConfirmed = false,
            Isadmin = false
        };
        await CreateUserIfNotExists(userManager, playerUser, "Player@123", "Player");
        PlayerToken = GenerateJwtToken(playerUser.Id.ToString(), playerEmail, false, "Player");
    }

    private async Task CreateUserIfNotExists(UserManager<Player> userManager, Player user, string password, string role)
    {
        if (await userManager.FindByEmailAsync(user.Email) == null)
        {
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }

    private async Task SeedSampleEntities(AppDbContext dbContext)
    {
        // Use specific GUIDs for seeded data to match ApiTestSetup
        var samplePlayerId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
        var samplePlayerId2 = Guid.Parse("414a47a2-faf4-4f0f-a1fb-b55f329d838d"); 
        var sampleGameId = Guid.Parse("b6e1bfa6-8e19-48d5-bd72-e1f66f7e406a");   

        if (await dbContext.Players.FindAsync(samplePlayerId) == null)
        {
            dbContext.Players.Add(new Player { 
                Id = samplePlayerId,
                UserName = "SamplePlayer1", 
                Email = "sampleplayer@example.com",
                PhoneNumber = "3223666896" ,
                Balance = 100,
                Isactive = true,
                Isadmin = false
            });
            dbContext.Players.Add(new Player { 
                Id = samplePlayerId2,  
                UserName = "SamplePlayer2", 
                Email = "sampleplayer2@example.com",
                PhoneNumber = "3223666896",
                Balance = 10,
                Isactive = true,
                Isadmin = false 
            });
            await dbContext.SaveChangesAsync();
        }

        var player = await dbContext.Players.FindAsync(samplePlayerId);
        if (player == null) {
            throw new Exception($"Player with ID {samplePlayerId} was not seeded correctly.");
        }

        if (await dbContext.Players.FindAsync(sampleGameId) == null)
        {
            dbContext.Games.Add(new Game { 
                Gameid = sampleGameId,
                Weeknumber = 1, 
                Year = 2024,
                Prizesum = 0,
                Iscomplete = false,
                Winningnumbers = new List<int> { 1, 2, 3 }
            });
            await dbContext.SaveChangesAsync();
        }
    }

    public string GenerateJwtToken(string userId, string email, bool isAdmin, string role)
{
    // Fetch the JWT settings from the environment variables (with fallback values)
    var key = _configuration["JwtSettings:Key"] ?? "zTDROdJ32D1LMf5DMQgIkdbznKYwIUH3"; // Add a fallback key for testing
    var issuer = _configuration["JwtSettings:Issuer"] ?? "Exam2024"; // Add a fallback issuer for testing
    var audience = _configuration["JwtSettings:Audience"] ?? "AppUsers"; // Add a fallback audience for testing

    if (string.IsNullOrEmpty(key))
    {
        throw new ArgumentNullException(nameof(key), "JWT secret key is missing.");
    }
    if (string.IsNullOrEmpty(issuer))
    {
        throw new ArgumentNullException(nameof(issuer), "JWT issuer is missing.");
    }
    if (string.IsNullOrEmpty(audience))
    {
        throw new ArgumentNullException(nameof(audience), "JWT audience is missing.");
    }

    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, userId),
        new Claim(JwtRegisteredClaimNames.Email, email),
        new Claim(ClaimTypes.Role, role),
        new Claim("IsAdmin", isAdmin.ToString())
    };

    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer,
        audience,
        claims,
        expires: DateTime.Now.AddHours(1),
        signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}
}
