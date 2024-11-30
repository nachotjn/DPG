using DataAccess;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

public class AppRepositoryTests{
    private readonly Mock<AppDbContext> mockContext;
    private readonly AppRepository repository;

    public AppRepositoryTests(){
        mockContext = new Mock<AppDbContext>();
        repository = new AppRepository(mockContext.Object);
    }


    // PLAYER TESTING

    [Fact]
    public void CreatePlayer_ShouldAddPlayerToContext(){
        // Given
        var player = new Player {
            Name = "Alberto",
            Email = "alberto@gmail.com",
            Isadmin = false,
            Isactive = true,
            Phone = "3223666896",
            Password = "123456789"
        };

        var mockSet = new Mock<DbSet<Player>>();
        mockContext.Setup(m => m.Players).Returns(mockSet.Object);
        // When
        repository.CreatePlayer(player);
    
        // Then
        mockSet.Verify(m => m.Add(It.IsAny<Player>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

   
    [Fact]
    public void GetAllPlayers_ShouldReturnPlayers(){
        var players = new List<Player>{
            new Player { Playerid = Guid.NewGuid(), Name = "Player 1" },
            new Player { Playerid = Guid.NewGuid(), Name = "Player 2" }
        }.AsQueryable();

        var mockSet = new Mock<DbSet<Player>>();
        mockSet.As<IQueryable<Player>>().Setup(m => m.Provider).Returns(players.Provider);
        mockSet.As<IQueryable<Player>>().Setup(m => m.Expression).Returns(players.Expression);
        mockSet.As<IQueryable<Player>>().Setup(m => m.ElementType).Returns(players.ElementType);
        mockSet.As<IQueryable<Player>>().Setup(m => m.GetEnumerator()).Returns(players.GetEnumerator());
        mockContext.Setup(m => m.Players).Returns(mockSet.Object);

        var result = repository.GetAllPlayers();

        Assert.Equal(2, result.Count);
        Assert.Equal("Player 1", result[0].Name);
    }

    [Fact]
    public void GetPlayerById_ShouldReturnPlayer_WhenPlayerExists(){ 
        var playerId = Guid.NewGuid();
        var players = new List<Player>
        {
            new Player { Playerid = playerId, Name = "Test Player" }
        }.AsQueryable();

        var mockSet = new Mock<DbSet<Player>>();
        mockSet.As<IQueryable<Player>>().Setup(m => m.Provider).Returns(players.Provider);
        mockSet.As<IQueryable<Player>>().Setup(m => m.Expression).Returns(players.Expression);
        mockSet.As<IQueryable<Player>>().Setup(m => m.ElementType).Returns(players.ElementType);
        mockSet.As<IQueryable<Player>>().Setup(m => m.GetEnumerator()).Returns(players.GetEnumerator());
        mockContext.Setup(m => m.Players).Returns(mockSet.Object);

        var result = repository.GetPlayerById(playerId);

        Assert.NotNull(result);
        Assert.Equal("Test Player", result?.Name);
    }

    [Fact]
    public void UpdatePlayer_ShouldUpdateExistingPlayer(){
        var playerId = Guid.NewGuid();
        var player = new Player { Playerid = playerId, Name = "Old Name" };
        mockContext.Setup(m => m.Players.Find(playerId)).Returns(player);
        var updatedPlayer = new Player { Playerid = playerId, Name = "New Name" };

        repository.UpdatePlayer(updatedPlayer);

        Assert.Equal("New Name", player.Name);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }


    // BOARD TESTING
}