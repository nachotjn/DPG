using DataAccess;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

public class AppRepositoryTests{
    //Tests setup
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
    [Fact]
    public void CreateBoard_ShouldAddBoardToContext(){
        var player = new Player {
            Playerid = Guid.NewGuid(),
        };

        var game = new Game {Gameid = Guid.NewGuid()};
        
        var board = new Board{
            Playerid = player.Playerid,
            Gameid = game.Gameid,
            Numbers = [1,2,3,4,5],
            Isautoplay = false
        };
        var mockSet = new Mock<DbSet<Board>>();
        mockContext.Setup(m => m.Boards).Returns(mockSet.Object);
       
        repository.CreateBoard(board);
    
        mockSet.Verify(m => m.Add(It.IsAny<Board>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Fact]
    public void GetBoardsForPlayer_ShouldReturnBoardsForPlayer(){
        var player = new Player {
            Playerid = Guid.NewGuid(),
        }; 

        var game = new Game {Gameid = Guid.NewGuid()};

        var board1 = new Board{
            Boardid = Guid.NewGuid(),
            Playerid = player.Playerid,
            Gameid = game.Gameid,
            Numbers = [1,2,3,4,5],
            Isautoplay = false
        };

        var board2 = new Board{
            Boardid = Guid.NewGuid(),
            Playerid = player.Playerid,
            Gameid = game.Gameid,
            Numbers = [1,2,3,4,5],
            Isautoplay = false
        };

        //This Board is with a different player
        var board3 = new Board{
            Boardid = Guid.NewGuid(),
            Playerid = Guid.NewGuid(),
            Gameid = game.Gameid,
            Numbers = [6, 7, 8, 9, 10],
            Isautoplay = true
        };

        var boards = new List<Board> { board1, board2, board3 }.AsQueryable();

        var mockSet = new Mock<DbSet<Board>>();
        mockSet.As<IQueryable<Board>>().Setup(m => m.Provider).Returns(boards.Provider);
        mockSet.As<IQueryable<Board>>().Setup(m => m.Expression).Returns(boards.Expression);
        mockSet.As<IQueryable<Board>>().Setup(m => m.ElementType).Returns(boards.ElementType);
        mockSet.As<IQueryable<Board>>().Setup(m => m.GetEnumerator()).Returns(boards.GetEnumerator());
        mockContext.Setup(m => m.Boards).Returns(mockSet.Object);

        var result = repository.GetBoardsForPlayer(player.Playerid);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, b => b.Boardid == board1.Boardid);
        Assert.Contains(result, b => b.Boardid == board2.Boardid);
        Assert.DoesNotContain(result, b => b.Boardid == board3.Boardid); 
    }

    [Fact]
    public void GetBoardsForGame_ShouldReturnBoardsForGame(){
        var player = new Player {
            Playerid = Guid.NewGuid(),
        }; 

        var game = new Game {Gameid = Guid.NewGuid()};

        var board1 = new Board{
            Boardid = Guid.NewGuid(),
            Playerid = player.Playerid,
            Gameid = game.Gameid,
            Numbers = [1,2,3,4,5],
            Isautoplay = false
        };

        var board2 = new Board{
            Boardid = Guid.NewGuid(),
            Playerid = player.Playerid,
            Gameid = game.Gameid,
            Numbers = [1,2,3,4,5],
            Isautoplay = false
        };
        //This one has a different Game
        var board3 = new Board{
            Boardid = Guid.NewGuid(),
            Playerid = player.Playerid,
            Gameid = Guid.NewGuid(),
            Numbers = [6, 7, 8, 9, 10],
            Isautoplay = true
        };

        var boards = new List<Board> { board1, board2, board3 }.AsQueryable();

        var mockSet = new Mock<DbSet<Board>>();
        mockSet.As<IQueryable<Board>>().Setup(m => m.Provider).Returns(boards.Provider);
        mockSet.As<IQueryable<Board>>().Setup(m => m.Expression).Returns(boards.Expression);
        mockSet.As<IQueryable<Board>>().Setup(m => m.ElementType).Returns(boards.ElementType);
        mockSet.As<IQueryable<Board>>().Setup(m => m.GetEnumerator()).Returns(boards.GetEnumerator());
        mockContext.Setup(m => m.Boards).Returns(mockSet.Object);

        var result = repository.GetBoardsForGame(game.Gameid);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, b => b.Boardid == board1.Boardid);
        Assert.Contains(result, b => b.Boardid == board2.Boardid);
        Assert.DoesNotContain(result, b => b.Boardid == board3.Boardid); 
    }



    // GAME TESTING
    [Fact]
    public void CreateGame_ShouldAddGameToContext(){
        var game = new Game {
            Weeknumber = 20,
            Year = 2025,
            Winningnumbers = [1,2,3],
            Iscomplete = false,
            Prizesum = 2500,
        };

        var mockSet = new Mock<DbSet<Game>>();
        mockContext.Setup(m => m.Games).Returns(mockSet.Object);
        

        repository.CreateGame(game);
    
        mockSet.Verify(m => m.Add(It.IsAny<Game>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Fact]
    public void GetAllGames_ShouldReturnGames(){
        var games = new List<Game>{
            new Game { Gameid = Guid.NewGuid(), Weeknumber = 48 },
            new Game { Gameid = Guid.NewGuid(), Weeknumber = 30 }
        }.AsQueryable();

        var mockSet = new Mock<DbSet<Game>>();
        mockSet.As<IQueryable<Game>>().Setup(m => m.Provider).Returns(games.Provider);
        mockSet.As<IQueryable<Game>>().Setup(m => m.Expression).Returns(games.Expression);
        mockSet.As<IQueryable<Game>>().Setup(m => m.ElementType).Returns(games.ElementType);
        mockSet.As<IQueryable<Game>>().Setup(m => m.GetEnumerator()).Returns(games.GetEnumerator());
        mockContext.Setup(m => m.Games).Returns(mockSet.Object);

        var result = repository.GetAllGames();

        Assert.Equal(2, result.Count);
        Assert.Equal(48, result[0].Weeknumber);
    }

    [Fact]
    public void UpdateGame_ShouldUpdateExistingGame(){
        var gameId = Guid.NewGuid();
        var game = new Game { Gameid = gameId, Iscomplete = false};
        mockContext.Setup(m => m.Games.Find(gameId)).Returns(game);
        var updatedGame = new Game { Gameid = gameId, Iscomplete = true };

        repository.UpdateGame(updatedGame);

        Assert.True(game.Iscomplete);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Fact]
    public void GetGameById_ShouldReturnGame_WhenGameExists(){ 
        var gameId = Guid.NewGuid();
        var games = new List<Game>{
            new Game { Gameid = gameId, Weeknumber = 48 },
            new Game { Gameid = Guid.NewGuid(), Weeknumber = 30 }
        }.AsQueryable();

        var mockSet = new Mock<DbSet<Game>>();
        mockSet.As<IQueryable<Game>>().Setup(m => m.Provider).Returns(games.Provider);
        mockSet.As<IQueryable<Game>>().Setup(m => m.Expression).Returns(games.Expression);
        mockSet.As<IQueryable<Game>>().Setup(m => m.ElementType).Returns(games.ElementType);
        mockSet.As<IQueryable<Game>>().Setup(m => m.GetEnumerator()).Returns(games.GetEnumerator());
        mockContext.Setup(m => m.Games).Returns(mockSet.Object);

        var result = repository.GetGameById(gameId);

        Assert.NotNull(result);
        Assert.Equal(48, result?.Weeknumber);
    }



    // WINNER TESTING
    [Fact]
    public void CreateWinner_ShouldAddWinnerToContext(){
        var player = new Player {Playerid = Guid.NewGuid(),};
        var game = new Game {Gameid = Guid.NewGuid()};
        var board = new Board{Boardid = Guid.NewGuid()};
        
        var winner = new Winner{
            Playerid = player.Playerid,
            Gameid = game.Gameid,
            Boardid = board.Boardid,
            Winningamount = 3000
        };

        var mockSet = new Mock<DbSet<Winner>>();
        mockContext.Setup(m => m.Winners).Returns(mockSet.Object);
       
        repository.CreateWinner(winner);
    
        mockSet.Verify(m => m.Add(It.IsAny<Winner>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Fact]
    public void GetWinnersForGame_ShouldReturnWinnersForGame(){
        var player = new Player { Playerid = Guid.NewGuid(),}; 
        var player2 = new Player { Playerid = Guid.NewGuid(),}; 
        var game = new Game {Gameid = Guid.NewGuid()};
        var board = new Board{Boardid = Guid.NewGuid(),};
        var board2 = new Board{Boardid = Guid.NewGuid(),};

        var winner1 = new Winner{
            Winnerid = Guid.NewGuid(),
            Boardid = board2.Boardid,
            Playerid = player.Playerid,
            Gameid = game.Gameid,
            Winningamount = 2500
        };
        var winner2 = new Winner{
            Winnerid = Guid.NewGuid(),
            Boardid = board.Boardid,
            Playerid = player2.Playerid,
            Gameid = game.Gameid,
            Winningamount = 4600
        };
        //Different game
        var winner3 = new Winner{
            Winnerid = Guid.NewGuid(),
            Boardid = board.Boardid,
            Playerid = player.Playerid,
            Gameid = Guid.NewGuid(),
            Winningamount = 4600
        };

        var winners = new List<Winner> { winner1, winner2, winner3 }.AsQueryable();

        var mockSet = new Mock<DbSet<Winner>>();
        mockSet.As<IQueryable<Winner>>().Setup(m => m.Provider).Returns(winners.Provider);
        mockSet.As<IQueryable<Winner>>().Setup(m => m.Expression).Returns(winners.Expression);
        mockSet.As<IQueryable<Winner>>().Setup(m => m.ElementType).Returns(winners.ElementType);
        mockSet.As<IQueryable<Winner>>().Setup(m => m.GetEnumerator()).Returns(winners.GetEnumerator());
        mockContext.Setup(m => m.Winners).Returns(mockSet.Object);

        var result = repository.GetWinnersForGame(game.Gameid);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, w => w.Winnerid == winner1.Winnerid);
        Assert.Contains(result, w => w.Winnerid == winner2.Winnerid);
        Assert.DoesNotContain(result, w => w.Winnerid == winner3.Winnerid); 
    }



    // TRANSACTION TESTING
    [Fact]
    public void CreateTransaction_ShouldAddTransactionToContext(){
        var player = new Player {Playerid = Guid.NewGuid(),};
        
        var transaction = new Transaction{
            Playerid = player.Playerid,
            Transactiontype = "Screenshot",
            Amount = 2500,
            Balanceaftertransaction = 3500,
            Description = "adadawd312e2",
            Isconfirmed = false
        };

        var mockSet = new Mock<DbSet<Transaction>>();
        mockContext.Setup(m => m.Transactions).Returns(mockSet.Object);
       
        repository.CreateTransaction(transaction);
    
        mockSet.Verify(m => m.Add(It.IsAny<Transaction>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Fact]
    public void GetTransactionsForPlayers_ShouldReturnTransactionsForPlayer(){
        var player = new Player { Playerid = Guid.NewGuid(),}; 

        var transaction1 = new Transaction{
            Transactionid = Guid.NewGuid(),
            Playerid = player.Playerid
        };

        var transaction2 = new Transaction{
            Transactionid = Guid.NewGuid(),
            Playerid = player.Playerid
        };

        //Different player for this one
        var transaction3 = new Transaction{
            Transactionid = Guid.NewGuid(),
            Playerid = Guid.NewGuid()
        };

        var transactions = new List<Transaction> {transaction1, transaction2 ,transaction3}.AsQueryable();

        var mockSet = new Mock<DbSet<Transaction>>();
        mockSet.As<IQueryable<Transaction>>().Setup(m => m.Provider).Returns(transactions.Provider);
        mockSet.As<IQueryable<Transaction>>().Setup(m => m.Expression).Returns(transactions.Expression);
        mockSet.As<IQueryable<Transaction>>().Setup(m => m.ElementType).Returns(transactions.ElementType);
        mockSet.As<IQueryable<Transaction>>().Setup(m => m.GetEnumerator()).Returns(transactions.GetEnumerator());
        mockContext.Setup(m => m.Transactions).Returns(mockSet.Object);

        var result = repository.GetTransactionsForPlayer(player.Playerid);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, t => t.Transactionid == transaction1.Transactionid);
        Assert.Contains(result, t => t.Transactionid == transaction2.Transactionid);
        Assert.DoesNotContain(result, t => t.Transactionid == transaction3.Transactionid); 
    }

    [Fact]
    public void UpdateTransaction_ShouldUpdateExistingTransaction(){
        var transactionId = Guid.NewGuid();
        var transaction = new Transaction { Transactionid = transactionId, Isconfirmed = false};
        mockContext.Setup(m => m.Transactions.Find(transactionId)).Returns(transaction);
        var updatedTransaction = new Transaction { Transactionid = transactionId, Isconfirmed = true };

        repository.UpdateTransaction(updatedTransaction);

        Assert.True(transaction.Isconfirmed);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }
}