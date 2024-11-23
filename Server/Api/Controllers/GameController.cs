using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Service;

[ApiController]
[Route("api/[controller]")]
public class GameController(IAppService appService) : ControllerBase{
    [HttpPost]
    [Route("")]
    public ActionResult<Game> CreateGame(CreateGameDto createGameDto){
        var game = appService.CreateGame(createGameDto);
        return Ok(game);
    }

    [HttpGet]
    [Route("")]
    public ActionResult<List<Game>> GetAllGames(){
        var games = appService.GetAllGames();
        return Ok(games);
    }
}