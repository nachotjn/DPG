using DataAccess.Models;

public interface IAppService{
    public PlayerDto CreatePlayer(CreatePlayerDto createPlayerDto);
    public List<Player> GetAllPlayers();
    public void UpdatePlayer(PlayerDto playerDto);
    public List<PlayerDto> GetPlayersForGame(Guid gameId);


    public BoardDto CreateBoard(CreateBoardDto createBoardDto);
    public List<Board> GetAllBoards();
    public List <BoardDto> GetBoardsForPlayer(Guid playerId);
    public List <BoardDto> GetBoardsForGame(Guid gameId);

    
    public GameDto CreateGame(CreateGameDto createGameDto);
    public List<Game> GetAllGames();
    public void UpdateGame(GameDto gameDto);


    public WinnerDto CreateWinner(CreateWinnerDto createWinnerDto);
    public List<Winner> GetAllWinners();
    public List <WinnerDto> GetWinnersForGame(Guid gameId);
    public void UpdateWinner(WinnerDto winnerDto, decimal winningAmount);
    public void DetermineWinnersForGame(Guid gameId);


    public TransactionDto CreateTransaction(CreateTransactionDto createTransactionDto);
    public List<TransactionDto> GetTransactionsForPlayer(Guid playerId);
    public void UpdateTransaction(TransactionDto transactionDto, bool isconfirmed);
}