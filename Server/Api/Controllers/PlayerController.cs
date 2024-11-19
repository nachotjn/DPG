using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Service;

[ApiController]
[Route("api/[controller]")]
public class PlayerController(IAppService appService) : ControllerBase{
    [HttpPost]
    [Route("")]
    public ActionResult<Player> CreatePlayer(CreatePlayerDto createPlayerDto){
        var player = appService.CreatePlayer(createPlayerDto);
        return Ok(player);
    }

    [HttpGet]
    [Route("")]
    public ActionResult<List<Player>> GetAllPlayers(){
        var players = appService.GetAllPlayers();
        return Ok(players);
    }
}