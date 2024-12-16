using DataAccess.Models;

public class ApiTestSetup{
    public CreatePlayerDto SampleCreatePlayerDto { get; }
    public CreateGameDto SampleCreateGameDto { get; }
    public CreateBoardDto SampleCreateBoardDto { get; }
    public CreateWinnerDto SampleCreateWinnerDto{get;}
    public CreateTransactionDto SampleCreateTransactionDto{get;}
    public Guid SamplePlayerId { get; } =   Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
    public Guid SamplePlayerId2 { get; } =   Guid.Parse("414a47a2-faf4-4f0f-a1fb-b55f329d838d");

    public string SamplePassword = "Hol@123546";
    public Guid SampleGameId { get; } = Guid.Parse("b6e1bfa6-8e19-48d5-bd72-e1f66f7e406a");
    public Guid SampleBoardId { get; } = Guid.NewGuid();
    public Guid SampleWinnerId {get;} = Guid.NewGuid();
    public Guid SampleTransactionId {get;} = Guid.NewGuid();
    public Player SamplePlayer { get; }
    public Game SampleGame { get; }
    public Board SampleBoard { get; }
    public Winner SampleWinner {get;}
    public Transaction SampleTransaction {get;}
    public GameDto SampleGameDto{get;}

    public ApiTestSetup(){
         SamplePlayer = new Player{
            Id = SamplePlayerId,
            UserName = "TestPlayer",
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

        SampleGameDto = new GameDto{
            GameID = SampleGameId,
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
            Weeknumber = 20,
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
            Transactiontype = "Code",
            Amount = 500,
            Balanceaftertransaction = 600,
            Description = "Abc123",
            Isconfirmed = false
       };
    }
}