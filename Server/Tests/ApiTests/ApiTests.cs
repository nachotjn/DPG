using System.Net.Http.Json;
using System.Text.Json;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class ApiTests : IClassFixture<CustomWebApplicationFactory<Program>>{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory; 

    private void SetAuthenticationHeader(string token){
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
    
    public ApiTests(CustomWebApplicationFactory<Program> factory){
        _factory = factory;  
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }



    // PLAYER TESTING
    [Fact]
    public async Task CreatePlayer_AdminUser_CreatesPlayerSuccessfully(){
        //Arrange
        var adminToken = _factory.AdminToken;
        SetAuthenticationHeader(adminToken);

        var createPlayerDto = new CreatePlayerDto{
            Name = "NewPlayer",
            Email = "newplayer@example.com",
            Password = "Secure@123",
            Phone = "3223666896",
            IsAdmin = false,
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Player", createPlayerDto);

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Error: " + content);
        }

        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadFromJsonAsync<PlayerDto>();
        Assert.NotNull(responseContent);
        Assert.Equal("NewPlayer", responseContent.Name);
    }

    [Fact]
    public async Task CreatePlayer_UnauthorizedUser_FailsWith401(){
        var createPlayerDto = new CreatePlayerDto{
            Name = "NewPlayer",
            Email = "newplayer@example.com",
            Password = "Secure@123",
            Phone = "3223666896",
            IsAdmin = false,
        };

        // No auth header

        var response = await _client.PostAsJsonAsync("/api/Player", createPlayerDto);

        
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreatePlayer_PlayerToken_FailsWith403(){

        var playertoken = _factory.PlayerToken;
        SetAuthenticationHeader(playertoken);
        var createPlayerDto = new CreatePlayerDto{
            Name = "NewPlayer2",
            Email = "newplayer2@example.com",
            Password = "Secure@123",
            Phone = "3223666896",
            IsAdmin = false,
        };

        
        var response = await _client.PostAsJsonAsync("/api/Player", createPlayerDto);

        Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
    }



  



}
