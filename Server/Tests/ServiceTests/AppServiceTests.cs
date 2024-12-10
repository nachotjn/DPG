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
    public void CreatePlayer_ShouldCreateAndReturnPlayer(){
        var result = setup.AppService.CreatePlayer(setup.SampleCreatePlayerDto);

        Assert.NotNull(result);
        Assert.Equal(setup.SamplePlayer.Name, result.Name);
        Assert.Equal(setup.SamplePlayer.Email, result.Email);
        
        setup.MockRepository.Verify(repo => repo.CreatePlayer(It.IsAny<Player>()), Times.Once);
    }

    [Fact]
    public void CreatePlayer_ShouldThrowExeptionIfDataInvalid(){
        var invalidPlayerDto = new CreatePlayerDto{
            Name = "Player",
            Email = "invalid-email", 
            Phone = "1234abc890",    
            Password = "123456"
        };

        var exception = Assert.Throws<ValidationException>(() =>
        setup.AppService.CreatePlayer(invalidPlayerDto));

    }

    [Fact]
    public void UpdatePlayer_ShouldUpdatePlayer(){
        var result = setup.AppService.CreatePlayer(setup.SampleCreatePlayerDto);

        result.Email = "juan@gmail.com";
        result.Phone = "3223666896";
        setup.AppService.UpdatePlayer(result);

        Assert.Equal(setup.SamplePlayer.Email, result.Email);
    }

    [Fact]
    public void UpdatePlayer_ShouldThrowExceptionIfInvalidData(){
       var result = setup.AppService.CreatePlayer(setup.SampleCreatePlayerDto);

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
            Playerid = Guid.NewGuid(),
            Name = "Mario",
            Email = "mario@email.com",
            Phone = "3223666896",
            Password = "123456",
            Balance = 19
        };

        var newBoardDto = new CreateBoardDto{
            Playerid = brokePlayer.Playerid,
            Gameid = setup.SampleGameId,
            Numbers = new List<int> { 1, 2, 3, 4, 5},
            Isautoplay = false,
            Autoplayweeks = null
        };

        setup.MockRepository.Setup(repo => repo.GetPlayerById(brokePlayer.Playerid)).Returns(brokePlayer);

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





    // TRANSACTION TESTING
    [Fact]
    public void CreateTransaction_ShouldCreateAndReturnTransaction(){
        var result = setup.AppService.CreateTransaction(setup.SampleCreateTransactionDto);

        Assert.NotNull(result);
        Assert.Equal(setup.SampleTransaction.Amount, result.Amount);
        
        //Assert.NotEqual(Guid.Empty, result.Transactionid); 
        setup.MockRepository.Verify(repo => repo.CreateTransaction(It.IsAny<Transaction>()), Times.Once);
    }

    
    [Fact]
    public void UpdateTransaction_ShouldConfirmATransactionAndUpdatePlayerBalance(){
        //var result = setup.AppService.CreateTransaction(setup.SampleCreateTransactionDto);
        
       
       // setup.AppService.UpdateTransaction(result, true);
        //var player = result.Player;
        

        //Assert.True(result.Isconfirmed);
        //Assert.Equal(setup.SampleTransaction.Isconfirmed, result.Isconfirmed);
        //Assert.Equal(setup.SamplePlayer.Balance, result.Balanceaftertransaction);
    }




}

