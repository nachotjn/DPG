using System.Net.Http.Json;
using System.Text.Json;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class ApiTests : IClassFixture<CustomWebApplicationFactory<Program>>{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory; 
    private readonly ApiTestSetup setup;
    


    
    public ApiTests(CustomWebApplicationFactory<Program> factory){
        _factory = factory;  
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        setup = new ApiTestSetup();
    }

    private void SetAuthenticationHeader(string token){
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
    

    // PLAYER TESTING
    [Fact]
    public async Task CreatePlayer_AdminUser_CreatesPlayerSuccessfully(){
        //Arrange
        var adminToken = _factory.AdminToken;
        SetAuthenticationHeader(adminToken);

        var createPlayerDto = setup.SampleCreatePlayerDto;

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
        Assert.Equal("TestPlayer", responseContent.Name);
    }

    [Fact]
    public async Task CreatePlayer_UnauthorizedUser_FailsWith401(){
        var createPlayerDto = setup.SampleCreatePlayerDto;

        // No auth header

        var response = await _client.PostAsJsonAsync("/api/Player", createPlayerDto);

        
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreatePlayer_PlayerToken_FailsWith403(){
        var playertoken = _factory.PlayerToken;
        SetAuthenticationHeader(playertoken);
        
        var createPlayerDto = setup.SampleCreatePlayerDto;

        
        var response = await _client.PostAsJsonAsync("/api/Player", createPlayerDto);

        Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task UpdatePlayer_InvalidPlayerId_ReturnsBadRequest(){
        var adminToken = _factory.AdminToken;
        SetAuthenticationHeader(adminToken);

        var existingPlayerDto = new PlayerDto{
            PlayerId = Guid.NewGuid(), 
            Name = "UpdatedPlayer",
            Email = "updatedplayer@example.com"
        };

        var response = await _client.PutAsJsonAsync($"/api/Player/{Guid.NewGuid()}", existingPlayerDto); 

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode); 
    }

    [Fact]
    public async Task UpdatePlayer_ShouldUpdate_Returns200Or204(){
        var adminToken = _factory.AdminToken;
        SetAuthenticationHeader(adminToken);

        var existingPlayerDto = new PlayerDto{
            PlayerId = setup.SamplePlayerId, 
            Name = "UpdatedPlayer",
            Email = "updatedplayer@example.com",
            Phone = "3223666896",
            IsAdmin = false,
            IsActive = true,
            Balance = 100
        };

        var response = await _client.PutAsJsonAsync($"/api/Player/{setup.SamplePlayerId}", existingPlayerDto); 

        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode); // 400 BadRequest
    }

    [Fact]
    public async Task GetAllPlayers_AdminUser_ReturnsAllPlayers(){
        var adminToken = _factory.AdminToken;
        SetAuthenticationHeader(adminToken);

        
        var response = await _client.GetAsync("/api/Player");

        response.EnsureSuccessStatusCode();
    
        var playersWrapper = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();

        Assert.NotNull(playersWrapper);
        
        Assert.True(playersWrapper.ContainsKey("$values"), "$values key not found in response.");
        var players = JsonSerializer.Deserialize<List<PlayerDto>>(playersWrapper["$values"].ToString());

        Assert.NotNull(players);
        Assert.True(players.Count > 0, "Expected at least one player in the response.");
    }

    [Fact]
    public async Task GetAllPlayers_NoAuthHeader_ReturnsUnauthorized(){
        var response = await _client.GetAsync("/api/Player");

        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }



    [Fact]
    public async Task GetPlayersForGame_InvalidGameId_ReturnsEmptyList(){
        var adminToken = _factory.AdminToken;
        SetAuthenticationHeader(adminToken);

        var invalidGameId = Guid.NewGuid(); 


        var response = await _client.GetAsync($"/api/Player/games/{invalidGameId}");
        response.EnsureSuccessStatusCode();
        var playersWrapper = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();

        Assert.True(playersWrapper.ContainsKey("$values"), "$values key not found in response.");
        var players = JsonSerializer.Deserialize<List<PlayerDto>>(playersWrapper["$values"].ToString());
        Assert.NotNull(players);
        Assert.Empty(players); 
    }

   

   
    // GAME TESTING
    [Fact]
    public async Task CreateGame_AdminUser_CreatesGameSuccessfully(){
        var adminToken = _factory.AdminToken;
        SetAuthenticationHeader(adminToken);

        
        var createGameDto = setup.SampleCreateGameDto;

        var response = await _client.PostAsJsonAsync("/api/Game", createGameDto);

        
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadFromJsonAsync<GameDto>();
        Assert.NotNull(responseContent);
        Assert.Equal(createGameDto.Weeknumber, responseContent.Weeknumber);
        Assert.Equal(createGameDto.Year, responseContent.Year);
    }

    [Fact]
    public async Task CreateGame_UnauthorizedUser_FailsWith401(){
        var createGameDto = setup.SampleCreateGameDto;

        // No auth header 
        var response = await _client.PostAsJsonAsync("/api/Game", createGameDto);

        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateGame_PlayerToken_FailsWith403(){
        var playerToken = _factory.PlayerToken;
        SetAuthenticationHeader(playerToken);

        var createGameDto = setup.SampleCreateGameDto;

        var response = await _client.PostAsJsonAsync("/api/Game", createGameDto);

        Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
    }




    // BOARD TESTING
    
    [Fact]
    public async Task CreateBoard_ValidData_CreatesBoardSuccessfully(){
      
        var adminToken = _factory.AdminToken;
        SetAuthenticationHeader(adminToken);

        
        var createBoardDto = new CreateBoardDto
        {
            Playerid = setup.SamplePlayerId, 
            Gameid = setup.SampleGameId,    
            Numbers = new List<int>{1, 2, 3, 4, 5},
            Isautoplay = false,
            Autoplayweeks = null
        };

       
        var response = await _client.PostAsJsonAsync("/api/Board", createBoardDto);
        

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        
        
        Assert.NotNull(responseBody);
        Assert.True(responseBody.ContainsKey("boardId"));
        Assert.True(responseBody.ContainsKey("player")); 
    }

    



   


    // TRANSACTIONSTESTING
    [Fact]
    public async Task CreateTransaction_ValidData_CreatesTransactionSuccessfully(){
        var adminToken = _factory.AdminToken;
        SetAuthenticationHeader(adminToken);

        var createTransactionDto = new CreateTransactionDto{
            Playerid = setup.SamplePlayerId,
            Transactiontype = "Code",
            Amount = 500,
            Balanceaftertransaction = 600,
            Description = "Abc123",
            Isconfirmed = false
       };

        var response = await _client.PostAsJsonAsync("/api/transaction", createTransactionDto);

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();


        response.EnsureSuccessStatusCode();
        Assert.NotNull(responseBody);
        Assert.True(responseBody.ContainsKey("transactionid"));
        Assert.True(responseBody.ContainsKey("player")); 
    }

}
