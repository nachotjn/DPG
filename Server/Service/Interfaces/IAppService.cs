using DataAccess.Models;

public interface IAppService{
    public PlayerDto CreatePlayer(CreatePlayerDto createPlayerDto);
    public List<Player> GetAllPlayers();
    public void UpdatePlayer(PlayerDto playerDto);
    public List<PlayerDto> GetPlayersForGame(Guid gameId);


    public BoardDto CreateBoard(CreateBoardDto createBoardDto);
    public List<Board> GetAllBoards();
    public List <BoardDto> GetBoardsForPlayer(Guid playerId);

    
    public GameDto CreateGame(CreateGameDto createGameDto);
    public List<Game> GetAllGames();
    public void UpdateGame(GameDto gameDto);


    public WinnerDto CreateWinner(CreateWinnerDto createWinnerDto);
    public List<Winner> GetAllWinners();


    public TransactionDto CreateTransaction(CreateTransactionDto createTransactionDto);
}