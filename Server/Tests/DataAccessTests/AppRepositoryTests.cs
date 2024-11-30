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

    [Fact]
    public void CreatePlayer_ShouldAddPlayerToContext()
    {
        // Given
        var player = new Player {
            Name = "Alberto",
            Email = "alberto@gmail.com",
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

}