using System;
using System.Collections.Generic;
using Moq;
using Service;
using DataAccess.Models;

public class AppServiceTestSetup {
    public Mock<IAppRepository> MockRepository { get; }
    public AppService AppService { get; }

    public Guid SamplePlayerId { get; } = Guid.NewGuid();
    public Guid SampleGameId { get; } = Guid.NewGuid();
    public Guid SampleBoardId { get; } = Guid.NewGuid();
    public Guid SampleWinnerId {get;} = Guid.NewGuid();
    public Guid SampleTransactionId {get;} = Guid.NewGuid();
    public Player SamplePlayer { get; }
    public Game SampleGame { get; }
    public Board SampleBoard { get; }
    public Winner SampleWinner {get;}
    public Transaction SampleTransaction {get;}
    public CreatePlayerDto SampleCreatePlayerDto { get; }
    public CreateGameDto SampleCreateGameDto { get; }
    public CreateBoardDto SampleCreateBoardDto { get; }
    public CreateWinnerDto SampleCreateWinnerDto{get;}
    public CreateTransactionDto SampleCreateTransactionDto{get;}

    public AppServiceTestSetup(){
        MockRepository = new Mock<IAppRepository>();

        
        AppService = new AppService(MockRepository.Object);

        
        SamplePlayer = new Player{
            Playerid = SamplePlayerId,
            Name = "Test Player",
            Email = "test@example.com",
            Phone = "1234567890",
            Balance = 100,
            Isadmin = false,
            Isactive = true
        };

        
        SampleGame = new Game{
            Gameid = SampleGameId,
            Weeknumber = 1,
            Year = 2024,
            Prizesum = 0,
            Iscomplete = false,
            Winningnumbers = new List<int> { 1, 2, 3, 4, 5 }
        };

        
        SampleBoard = new Board{
            Boardid = SampleBoardId,
            Playerid = SamplePlayerId,
            Gameid = SampleGameId,
            Numbers = new List<int> { 1, 2, 3, 4, 5 },
            Isautoplay = false,
            Autoplayweeks = null
        };

        SampleWinner = new Winner{
            Winnerid = SampleWinnerId,
            Playerid = SamplePlayerId,
            Gameid = SampleGameId,
            Winningamount = 1200
        };

        SampleTransaction = new Transaction{
            Transactionid = SampleTransactionId,
            Playerid = SamplePlayerId,
            Transactiontype = "Screenshot",
            Amount = 500,
            Balanceaftertransaction = 600,
            Description = null,
            Isconfirmed = false
        };

        
        SampleCreatePlayerDto = new CreatePlayerDto{
            Name = "Test Player",
            Email = "test@example.com",
            Phone = "1234567890",
            Password = "123456"
        };

        SampleCreateGameDto = new CreateGameDto{
            Weeknumber = 1,
            Year = 2024
        };

        SampleCreateBoardDto = new CreateBoardDto{
            Playerid = SamplePlayerId,
            Gameid = SampleGameId,
            Numbers = new List<int> { 1, 2, 3, 4, 5 },
            Isautoplay = false,
            Autoplayweeks = null
        };

       SampleCreateTransactionDto = new CreateTransactionDto{
            Playerid = SamplePlayerId,
            Transactiontype = "Screenshot",
            Amount = 500,
            Balanceaftertransaction = 600,
            Description = null,
            Isconfirmed = false
       };

        
        ConfigureMockDefaults();
    }

    private void ConfigureMockDefaults(){
        MockRepository.Setup(repo => repo.GetPlayerById(SamplePlayerId)).Returns(SamplePlayer);
        MockRepository.Setup(repo => repo.CreatePlayer(It.IsAny<Player>())).Returns(SamplePlayer);

        
        MockRepository.Setup(repo => repo.GetGameById(SampleGameId)).Returns(SampleGame);
        MockRepository.Setup(repo => repo.GetGameByWeekAndYear(1, 2024)).Returns((Game)null); 
        MockRepository.Setup(repo => repo.CreateGame(It.IsAny<Game>())).Returns(SampleGame);

        
        MockRepository.Setup(repo => repo.GetBoardsForGame(SampleGameId)).Returns(new List<Board> { SampleBoard });
        MockRepository.Setup(repo => repo.CreateBoard(It.IsAny<Board>())).Returns(SampleBoard);
        

        MockRepository.Setup(repo => repo.GetTransactionById(SampleTransactionId)).Returns(SampleTransaction);
        MockRepository.Setup(repo => repo.CreateTransaction(It.IsAny<Transaction>())).Returns(SampleTransaction);


    }
}
