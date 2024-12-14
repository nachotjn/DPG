using System.ComponentModel.DataAnnotations;
using DataAccess.Models;
using Moq;
using Xunit;

public class AppServiceTests{
    private readonly AppServiceTestSetup setup;

    public AppServiceTests(){
        setup = new AppServiceTestSetup();
    }


    // PLAYER TESTING
    [Fact]
    public async void CreatePlayer_ShouldCreateAndReturnPlayer(){
        var result =  await setup.AppService.CreatePlayer(setup.SampleCreatePlayerDto);
        setup.MockRepository.Setup(repo => repo.CreatePlayer(It.IsAny<Player>(), setup.SamplePassword))
         .ReturnsAsync(setup.SamplePlayer);


        Assert.NotNull(result);
        Assert.Equal(setup.SamplePlayer.UserName, result.Name);
        Assert.Equal(setup.SamplePlayer.Email, result.Email);
        
    }

    [Fact]
    public async void CreatePlayer_ShouldThrowExeptionIfDataInvalid(){
        var invalidPlayerDto = new CreatePlayerDto{
            Name = "Player",
            Email = "invalid-email", 
            Phone = "1234abc890",    
            Password = "Abc@123456"
        };

        var exception = Assert.ThrowsAsync<ValidationException>(() =>
        setup.AppService.CreatePlayer(invalidPlayerDto));

    }

    [Fact]
    public async void UpdatePlayer_ShouldUpdatePlayer(){
        var result = await setup.AppService.CreatePlayer(setup.SampleCreatePlayerDto);
        setup.MockRepository.Setup(repo => repo.UpdatePlayer(It.IsAny<Player>()));
        setup.MockRepository.Setup(repo => repo.GetPlayerById(setup.SamplePlayerId))
            .Returns(setup.SamplePlayer);

        result.Email = "juan@gmail.com";
        result.Phone = "3223666896";
        setup.AppService.UpdatePlayer(result);

        Assert.Equal(setup.SamplePlayer.Email, result.Email);
    }

    [Fact]
    public async void UpdatePlayer_ShouldThrowExceptionIfInvalidData(){
       var result =  await setup.AppService.CreatePlayer(setup.SampleCreatePlayerDto);

       result.Email = "hola123";

        var exception = Assert.Throws<ValidationException>(() =>
            setup.AppService.UpdatePlayer(result));
    }



    //BOARD TESTING
    [Fact]
    public void CreateBoard_ShouldCreateBoard(){
        var result =setup.AppService.CreateBoard(setup.SampleCreateBoardDto);
        Assert.NotNull(result);
        Assert.Equal(setup.SampleBoard.Numbers, result.Numbers);
 
        setup.MockRepository.Verify(repo => repo.CreateBoard(It.IsAny<Board>()), Times.Once);
    }

    [Fact]
    public void CreateBoard_ShouldThrowExeptionIfSequenceInvalid(){
        var invalidBoardDto = new CreateBoardDto{
            Playerid = setup.SamplePlayerId,
            Gameid = setup.SampleGameId,
            Numbers = new List<int> { 1, 2, 3},
            Isautoplay = false,
            Autoplayweeks = null
        };

        var exception = Assert.Throws<ValidationException>(() =>
        setup.AppService.CreateBoard(invalidBoardDto));
    }

    [Fact]
    public void CreateBoard_ShouldThrowExeptionIfPlayerDoesntHaveEnoughBalance(){
        var brokePlayer = new Player{
            Id = Guid.NewGuid(),
            UserName = "Mario",
            Email = "mario@email.com",
            PhoneNumber = "3223666896",
            Isactive = true,
            Balance = 19
        };

        var newBoardDto = new CreateBoardDto{
            Playerid = brokePlayer.Id,
            Gameid = setup.SampleGameId,
            Numbers = new List<int> { 1, 2, 3, 4, 5},
            Isautoplay = false,
            Autoplayweeks = null
        };

        setup.MockRepository.Setup(repo => repo.GetPlayerById(brokePlayer.Id)).Returns(brokePlayer);

        var exception = Assert.Throws<InvalidOperationException>(() =>
        setup.AppService.CreateBoard(newBoardDto));
    }

    [Fact]
    public void CreateBoardWithAutoplay_ShouldCreateNewGameForNextWeeks(){
        setup.MockRepository.Setup(repo => repo.GetGameByWeekAndYear(It.IsAny<int>(), It.IsAny<int>()))
        .Returns((Game)null);

        var newBoardDto = new CreateBoardDto{
            Playerid = setup.SamplePlayerId,
            Gameid = setup.SampleGameId,
            Numbers = new List<int> { 1, 2, 3, 4, 5},
            Isautoplay = true,
            Autoplayweeks = 2
        };

        setup.AppService.CreateBoard(newBoardDto);

        //Check if the games were created       
        setup.MockRepository.Verify(repo => repo.CreateGame(It.Is<Game>(g =>
        g.Weeknumber == 2 && g.Year == setup.SampleGame.Year)), Times.Once);

        setup.MockRepository.Verify(repo => repo.CreateGame(It.Is<Game>(g =>
            g.Weeknumber == 3 && g.Year == setup.SampleGame.Year)), Times.Once);

        //Check if the boards are created for the games        
        setup.MockRepository.Verify(repo => repo.CreateBoard(It.Is<Board>(b =>
            b.Gameid != setup.SampleGameId && b.Numbers.SequenceEqual(newBoardDto.Numbers))), Times.Exactly(2));

        //Check if the prizesum was updated for the created games
        setup.MockRepository.Verify(repo => repo.UpdateGame(It.Is<Game>(g =>
            (g.Weeknumber == 2 || g.Weeknumber == 3) && g.Prizesum > 0)), Times.Exactly(2));
    }


    [Fact]
    public void CreateBoardWithAutoplay_ShouldUpdateNextWeeksPrizesum(){
         var existingGame = new Game{
            Gameid = Guid.NewGuid(),
            Weeknumber = 2,
            Year = 2024,
            Prizesum = 50,  
            Iscomplete = false
        };

        setup.MockRepository.Setup(repo => repo.GetGameByWeekAndYear(2, 2024))
            .Returns(existingGame); 

        var newBoardDto = new CreateBoardDto{
            Playerid = setup.SamplePlayerId,
            Gameid = setup.SampleGameId,
            Numbers = new List<int> { 1, 2, 3, 4, 5},
            Isautoplay = true,
            Autoplayweeks = 1
        };

      
        setup.AppService.CreateBoard(newBoardDto);
        
        //Check that there are no new games being created for that week since its already exists
        setup.MockRepository.Verify(repo => repo.CreateGame(It.IsAny<Game>()), Times.Never);

        setup.MockRepository.Verify(repo => repo.CreateBoard(It.Is<Board>(b =>
            b.Gameid != setup.SampleGameId && b.Numbers.SequenceEqual(newBoardDto.Numbers))), Times.Exactly(1));

        
        setup.MockRepository.Verify(repo => repo.UpdateGame(It.Is<Game>(g =>
            (g.Weeknumber == 2) && g.Prizesum > 50)), Times.Exactly(1));
    }



    // GAME TESTING

    [Fact]
    public  void CreateGame_ShouldCreateAndReturnPGame(){
        var result = setup.AppService.CreateGame(setup.SampleCreateGameDto);
       

        Assert.NotNull(result);
        Assert.Equal(setup.SampleGame.Iscomplete, result.Iscomplete);
        Assert.Equal(setup.SampleGame.Prizesum, result.Prizesum);
        
    }
    [Fact]
    public void CreateGame_ShouldThrowExceptionIfGameExistsForWeekAndYear(){
        setup.MockRepository.Setup(repo => repo.GetGameByWeekAndYear(1, 2024)).Returns(setup.SampleGame);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            setup.AppService.CreateGame(setup.SampleCreateGameDto)
        );
        Assert.Equal("A game already exists for week 1 of 2024.", exception.Message);
        setup.MockRepository.Verify(repo => repo.GetGameByWeekAndYear(1, 2024), Times.Once);
    }

    [Fact]
    public void UpdateGame_ShouldUpdateGame(){
        var result = setup.AppService.CreateGame(setup.SampleCreateGameDto);

        result.Winningnumbers = new List<int> { 1, 2, 3};
        
        setup.AppService.UpdateGame(result);

        Assert.Equal(setup.SampleGame.Winningnumbers, result.Winningnumbers);
    }

    [Fact]
    public void UpdateGame_ShouldThrowExceptionIfWinningSequenceInvalid(){
       var result = setup.AppService.CreateGame(setup.SampleCreateGameDto);

       result.Winningnumbers = new List<int> { 1, 2, 3, 4};

        var exception = Assert.Throws<ValidationException>(() =>
            setup.AppService.UpdateGame(result));
    }



    // WINNER TESTING
   [Fact]
    public void DetermineWinnersForGame_ShouldThrowException_WhenGameNotFound(){
        var gameId = Guid.NewGuid();
        setup.MockRepository.Setup(repo => repo.GetGameById(gameId)).Returns((Game)null);

        
        var exception = Assert.Throws<Exception>(() => 
            setup.AppService.DetermineWinnersForGame(gameId));


        Assert.Equal($"Game with ID {gameId} not found.", exception.Message);
    }

    [Fact]
    public void DetermineWinnersForGame_ShouldThrowException_WhenGameNotComplete(){
        var gameId = Guid.NewGuid();
        var game = new Game { Gameid = gameId, Iscomplete = false };
        setup.MockRepository.Setup(repo => repo.GetGameById(gameId)).Returns(game);

        var exception = Assert.Throws<InvalidOperationException>(() => 
            setup.AppService.DetermineWinnersForGame(gameId));


        Assert.Equal($"Game with ID {gameId} is not complete", exception.Message);
    }

    [Fact]
    public void DetermineWinnersForGame_ShouldThrowException_WhenGameHasNoWinningSequence(){
        var gameId = Guid.NewGuid();
        var game = new Game { Gameid = gameId, Iscomplete = true, Winningnumbers = null };
        setup.MockRepository.Setup(repo => repo.GetGameById(gameId)).Returns(game);

        var exception = Assert.Throws<Exception>(() => 
            setup.AppService.DetermineWinnersForGame(gameId));

        Assert.Equal($"Game with ID {gameId} doenst have a winning sequence", exception.Message);
    }

   [Fact]
    public void DetermineWinnersForGame_ShouldCreateWinners_WhenThereAreWinningBoards(){
        var gameId = Guid.NewGuid();
        var game = new Game { Gameid = gameId, Iscomplete = true, Winningnumbers = new List<int> { 1, 2, 3}, Prizesum = 10000 };

        var player = new Player { Id = Guid.NewGuid(), UserName = "Player1", Email = "player1@email.com" };
        var board = new Board { Boardid = Guid.NewGuid(), Gameid = gameId, Numbers = new List<int> { 1, 2, 3, 4, 5 }, Playerid = player.Id, Player = player };
        var boards = new List<Board> { board };

        setup.MockRepository.Setup(repo => repo.GetGameById(gameId)).Returns(game);
        setup.MockRepository.Setup(repo => repo.GetBoardsForGame(gameId)).Returns(boards);
        setup.MockRepository.Setup(repo => repo.CreateWinner(It.IsAny<Winner>()));

        setup.AppService.DetermineWinnersForGame(gameId);

        setup.MockRepository.Verify(repo => repo.CreateWinner(It.Is<Winner>(w =>
            w.Playerid == player.Id && w.Winningamount == 5000)), Times.Once);
    }

    
    [Fact]
    public void DetermineWinnersForGame_ShouldDistributePrizeBetweenMultipleWinners(){
        var gameId = Guid.NewGuid();
        var game = new Game { Gameid = gameId, Iscomplete = true, Winningnumbers = new List<int> { 1, 2, 3, 4, 5 }, Prizesum = 10000m };

        var player1 = new Player { Id = Guid.NewGuid(), UserName = "Player1", Email = "player1@email.com" };
        var player2 = new Player { Id = Guid.NewGuid(), UserName = "Player2", Email = "player2@email.com" };

        var board1 = new Board { Boardid = Guid.NewGuid(), Gameid = gameId, Numbers = new List<int> { 1, 2, 3, 4, 5 }, Playerid = player1.Id, Player = player1 };
        var board2 = new Board { Boardid = Guid.NewGuid(), Gameid = gameId, Numbers = new List<int> { 1, 2, 3, 4, 5 }, Playerid = player2.Id, Player = player2 };
        var boards = new List<Board> { board1, board2 };

        setup.MockRepository.Setup(repo => repo.GetGameById(gameId)).Returns(game);
        setup.MockRepository.Setup(repo => repo.GetBoardsForGame(gameId)).Returns(boards);
        setup.MockRepository.Setup(repo => repo.CreateWinner(It.IsAny<Winner>()));

        setup.AppService.DetermineWinnersForGame(gameId);

        setup.MockRepository.Verify(repo => repo.CreateWinner(It.Is<Winner>(w =>
            w.Playerid == player1.Id && w.Winningamount == 5000m)), Times.Once);
        setup.MockRepository.Verify(repo => repo.CreateWinner(It.Is<Winner>(w =>
            w.Playerid == player2.Id && w.Winningamount == 5000m)), Times.Once);
    }



 
    // TRANSACTION TESTING
    [Fact]
    public void CreateTransaction_ShouldCreateTransactionAndReturnDto(){
        var createTransactionDto = new CreateTransactionDto {
            Playerid = setup.SamplePlayerId,
            Amount = 100,
        };

        setup.MockRepository.Setup(repo => repo.GetPlayerById(setup.SamplePlayerId))
            .Returns(setup.SamplePlayer);

        setup.MockRepository.Setup(repo => repo.CreateTransaction(It.IsAny<Transaction>()))
            .Returns(setup.SampleTransaction);

        var result = setup.AppService.CreateTransaction(createTransactionDto);

        Assert.NotNull(result);
        Assert.Equal(createTransactionDto.Amount, result.Amount);
        Assert.Equal(setup.SamplePlayer.UserName, result.Player.UserName);
        
        setup.MockRepository.Verify(repo => repo.CreateTransaction(It.IsAny<Transaction>()), Times.Once);
    }

    [Fact]
    public void CreateTransaction_ShouldThrowExceptionIfPlayerNotFound(){
        var createTransactionDto = new CreateTransactionDto {
            Playerid = Guid.NewGuid(), 
            Amount = 100
        };

        setup.MockRepository.Setup(repo => repo.GetPlayerById(createTransactionDto.Playerid))
            .Returns((Player)null);

        var exception = Assert.Throws<ArgumentException>(() =>
            setup.AppService.CreateTransaction(createTransactionDto)
        );

        Assert.Equal($"Player with ID {createTransactionDto.Playerid} does not exist.", exception.Message);
    }

    [Fact]
    public void GetTransactionsForPlayer_ShouldReturnTransactionDtos(){
        var transactions = new List<Transaction> {
            new Transaction { Transactionid = Guid.NewGuid(), Amount = 100, Playerid = setup.SamplePlayerId },
            new Transaction { Transactionid = Guid.NewGuid(), Amount = 200, Playerid = setup.SamplePlayerId }
        };

        setup.MockRepository.Setup(repo => repo.GetTransactionsForPlayer(setup.SamplePlayerId))
            .Returns(transactions);

        var result = setup.AppService.GetTransactionsForPlayer(setup.SamplePlayerId);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(transactions[0].Transactionid, result[0].Transactionid);
        Assert.Equal(transactions[1].Transactionid, result[1].Transactionid);
    }

    [Fact]
    public void UpdateTransaction_ShouldUpdateTransactionIfFoundAndConfirm(){
        var existingTransaction = new Transaction { 
            Transactionid = Guid.NewGuid(), 
            Playerid = setup.SamplePlayerId, 
            Amount = 100, 
            Isconfirmed = false 
        };

        setup.MockRepository.Setup(repo => repo.GetTransactionById(existingTransaction.Transactionid))
            .Returns(existingTransaction);

        setup.MockRepository.Setup(repo => repo.GetPlayerById(setup.SamplePlayerId))
            .Returns(setup.SamplePlayer);

        setup.MockRepository.Setup(repo => repo.UpdateTransaction(It.IsAny<Transaction>()));
        setup.MockRepository.Setup(repo => repo.UpdatePlayer(It.IsAny<Player>()));

        var transactionDto = new TransactionDto { 
            Transactionid = existingTransaction.Transactionid, 
        };

        setup.AppService.UpdateTransaction(transactionDto, true);

        Assert.True(existingTransaction.Isconfirmed);
        Assert.Equal(200, setup.SamplePlayer.Balance);

        setup.MockRepository.Verify(repo => repo.UpdateTransaction(It.IsAny<Transaction>()), Times.Once);
        setup.MockRepository.Verify(repo => repo.UpdatePlayer(It.IsAny<Player>()), Times.Once);
    }

}

