using DataAccess;
using DataAccess.Models;
using Google.Cloud.SecretManager.V1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var isTestingEnvironment = builder.Environment.EnvironmentName == "Test";
var isDevelopment = builder.Environment.IsDevelopment();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

string GetSecret(string secretName)
{
    try
    {
        var client = SecretManagerServiceClient.Create();
        var secretVersion = new SecretVersionName("exam2024-444413", secretName, "latest");
        var response = client.AccessSecretVersion(secretVersion);
        return response.Payload.Data.ToStringUtf8();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error retrieving secret: {ex.Message}");
        throw;
    }
}


// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Add JWT Authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\n\nExample: \"Bearer eyJhbGciOi...\"",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// dbcontext
string connectionString;
if (isDevelopment)
{
    connectionString = builder.Configuration.GetConnectionString("DbConnectionString");
}
else
{
    connectionString = GetSecret("AppDb");
}

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

// Identity
builder.Services.AddIdentity<Player, IdentityRole<Guid>>(options => { })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
});

// repository and service
builder.Services.AddScoped<IAppRepository, AppRepository>();
builder.Services.AddScoped<IAppService, AppService>();
builder.Services.AddHttpContextAccessor();

// enable CORS for react 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

app.UseForwardedHeaders(
    new ForwardedHeadersOptions{
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });

//Method to format the dates for the database
DateTime? ToDatabaseKind(DateTime? input)
{
    return input.HasValue ? DateTime.SpecifyKind(input.Value, DateTimeKind.Unspecified) : (DateTime?)null;
}

// Apply migrations on startup and define roles
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    dbContext.Database.Migrate();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Player>>();

    string[] roles = new[] { "Admin", "Player" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid> { Name = role });
        }
    }

    string adminEmail;
    string adminPassword;

    if (isDevelopment)
    {
        // Hardcoded admin credentials for local development
        adminEmail = "admin@local.com";
        adminPassword = "admin123";
    }
    else
    {
        // Fetch admin credentials from Google Secret Manager in production
        adminEmail = GetSecret("adminEmail");
        adminPassword = GetSecret("adminPassword");
    }

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new Player
        {
            UserName = "admin",
            Email = adminEmail,
            EmailConfirmed = true,
            Isadmin = true,
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

    var existingGames = dbContext.Games
        .Where(g => g.Year >= 2024 && g.Year < 2024 + 5) 
        .ToList();
    if (!existingGames.Any()) // Only create games if no games exist in the database
    {
        int startingYear = 2024;
        int startingWeek = 51;
        int numberOfYears = 5;
        
        var gamesToCreate = new List<Game>();
        
        for (int year = startingYear; year < startingYear + numberOfYears; year++)
        {
            for (int week = startingWeek; week <= 52; week++)  
            {
                var game = new Game
                {
                    Gameid = Guid.NewGuid(),
                    Year = year,
                    Weeknumber = week,
                    Iscomplete = false,
                    Winningnumbers = null, 
                    Prizesum = 0, 
                    Createdat = ToDatabaseKind(DateTime.UtcNow),
                    Updatedat = ToDatabaseKind(DateTime.UtcNow)
                };
                gamesToCreate.Add(game);
            }
            startingWeek = 1; 
        }
        
        dbContext.Games.AddRange(gamesToCreate);
        await dbContext.SaveChangesAsync();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseRouting();

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run($"http://0.0.0.0:{port}");

public partial class Program { }
