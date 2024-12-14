using DataAccess.Models;

public class ApiTestSetup{
    public CreatePlayerDto SampleCreatePlayerDto { get; }
    public CreateGameDto SampleCreateGameDto { get; }
    public CreateBoardDto SampleCreateBoardDto { get; }
    public CreateWinnerDto SampleCreateWinnerDto{get;}
    public CreateTransactionDto SampleCreateTransactionDto{get;}
    public Guid SamplePlayerId { get; } = Guid.NewGuid();
    public string SamplePassword = "Hol@123546";
    public Guid SampleGameId { get; } = Guid.NewGuid();
    public Guid SampleBoardId { get; } = Guid.NewGuid();
    public Guid SampleWinnerId {get;} = Guid.NewGuid();
    public Guid SampleTransactionId {get;} = Guid.NewGuid();
    public Player SamplePlayer { get; }
    public Game SampleGame { get; }
    public Board SampleBoard { get; }
    public Winner SampleWinner {get;}
    public Transaction SampleTransaction {get;}

    public ApiTestSetup(){
         SamplePlayer = new Player{
            Id = SamplePlayerId,
            UserName = "Test Player",
            Email = "test@example.com",
            PhoneNumber = "1234567890",
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
            Winningnumbers = new List<int> { 1, 2, 3}
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
            Name = "TestPlayer",
            Email = "test@example.com",
            Phone = "1234567890",
            Password = "Secure@123"
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
    }
}