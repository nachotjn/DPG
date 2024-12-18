using System;
using System.Collections.Generic;
using Moq;
using Service;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

public class AppServiceTestSetup {
    public Mock<IAppRepository> MockRepository { get; }
    private Mock<UserManager<Player>> MockUserManager;
    private Mock<IHttpContextAccessor> MockhttpContextAccessor;
    public AppService AppService { get; }

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
    public CreatePlayerDto SampleCreatePlayerDto { get; }
    public CreateGameDto SampleCreateGameDto { get; }
    public CreateBoardDto SampleCreateBoardDto { get; }
    public CreateWinnerDto SampleCreateWinnerDto{get;}
    public CreateTransactionDto SampleCreateTransactionDto{get;}

    public AppServiceTestSetup(){

        var mockUserStore = Mock.Of<IUserStore<Player>>();
        var mockPasswordHasher = Mock.Of<IPasswordHasher<Player>>();
        var mockIdentityOptions = Mock.Of<IOptions<IdentityOptions>>();
        var mockLogger = Mock.Of<ILogger<UserManager<Player>>>();
        var mockUserValidators = new List<IUserValidator<Player>>();
        var mockPasswordValidators = new List<IPasswordValidator<Player>>();
        var mockLookupNormalizer = Mock.Of<ILookupNormalizer>();
        var mockIdentityErrorDescriber = Mock.Of<IdentityErrorDescriber>();
        var mockServiceProvider = Mock.Of<IServiceProvider>();

        MockRepository = new Mock<IAppRepository>();
         MockUserManager = new Mock<UserManager<Player>>(mockUserStore, mockIdentityOptions, mockPasswordHasher,
                                                        mockUserValidators, mockPasswordValidators, mockLookupNormalizer,
                                                        mockIdentityErrorDescriber, mockServiceProvider, mockLogger);
        MockhttpContextAccessor = new Mock<IHttpContextAccessor>();

        
        AppService = new AppService(MockRepository.Object, MockUserManager.Object, MockhttpContextAccessor.Object);

        
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
        MockRepository.Setup(repo => repo.CreatePlayer(It.IsAny<Player>(), It.IsAny<string>())).ReturnsAsync(SamplePlayer);

        MockRepository.Setup(repo => repo.GetGameById(SampleGameId)).Returns(SampleGame);
        MockRepository.Setup(repo => repo.GetGameByWeekAndYear(1, 2024)).Returns((Game)null); 
        MockRepository.Setup(repo => repo.CreateGame(It.IsAny<Game>())).Returns(SampleGame);

        MockRepository.Setup(repo => repo.GetBoardsForGame(SampleGameId)).Returns(new List<Board> { SampleBoard });
        MockRepository.Setup(repo => repo.CreateBoard(It.IsAny<Board>())).Returns(SampleBoard);

        MockRepository.Setup(repo => repo.GetTransactionById(SampleTransactionId)).Returns(SampleTransaction);
        MockRepository.Setup(repo => repo.CreateTransaction(It.IsAny<Transaction>())).Returns(SampleTransaction);

        MockRepository.Setup(repo => repo.CreateWinner(It.IsAny<Winner>())).Returns(SampleWinner);

        MockUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(SamplePlayer);
        MockUserManager.Setup(um => um.CheckPasswordAsync(It.IsAny<Player>(), It.IsAny<string>())).ReturnsAsync(true);
        MockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<Player>())).ReturnsAsync(new List<string> { "Admin" });
        MockUserManager.Setup(um => um.IsInRoleAsync(It.IsAny<Player>(), "Admin")).ReturnsAsync(true);

        
        MockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(SamplePlayer);
        MockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<Player>(), "Player")).ReturnsAsync(IdentityResult.Success);


        var mockClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim(ClaimTypes.Name, SamplePlayer.UserName),
            new Claim(ClaimTypes.Email, SamplePlayer.Email),
            new Claim(ClaimTypes.NameIdentifier, SamplePlayer.Id.ToString()),
            new Claim(ClaimTypes.Role, "Admin") 
        }, "mock"));

        MockhttpContextAccessor.Setup(h => h.HttpContext.User).Returns(mockClaimsPrincipal);
    }

}
