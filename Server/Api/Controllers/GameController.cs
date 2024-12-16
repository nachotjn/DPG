using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;

[ApiController]
[Route("api/[controller]")]
public class GameController(IAppService appService) : ControllerBase{
    [HttpPost]
    [Route("")]
    [Authorize(Roles = "Admin")]
    public ActionResult<GameDto> CreateGame(CreateGameDto createGameDto){
        var game = appService.CreateGame(createGameDto);
        return Ok(game);
    }

    [HttpGet]
    [Route("")]
    [Authorize]
    public ActionResult<List<GameDto>> GetAllGames(){
        var games = appService.GetAllGames();
        return Ok(games);
    }

    [HttpPut]
    [Route("{gameId}")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateGame(Guid gameId, GameDto gameDto){
        if (gameId != gameDto.GameID){
            return BadRequest("Game ID mismatch.");
        }

        try{
            appService.UpdateGame(gameDto);
            return NoContent(); 
        }
        catch (Exception ex){
            return NotFound(new { Message = ex.Message });
        }
    }
}