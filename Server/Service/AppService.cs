using DataAccess.Models;

namespace Service;

public interface IAppService{
    public PlayerDto CreatePlayer(CreatePlayerDto createPlayerDto);
    public List<Player> GetAllPlayers();

}

public class AppService(IAppRepository appRepository) : IAppService{
    //Players
    public PlayerDto CreatePlayer(CreatePlayerDto createPlayerDto)
    {
        var player = createPlayerDto.ToPlayer();
        Player newPlayer = appRepository.CreatePlayer(player);
        return new PlayerDto().FromEntity(newPlayer);
    }

    public List<Player> GetAllPlayers()
    {
        return appRepository.GetAllPlayers().ToList();
    }
}
