using Microsoft.AspNetCore.Mvc;
using Service;

[ApiController]
[Route("api/[controller]")]
public class PlayerController(IAppService appService) : ControllerBase{
    [HttpPost]
    [Route("")]
    public async Task<ActionResult<PlayerDto>> CreatePlayer(CreatePlayerDto createPlayerDto){
        var player = await appService.CreatePlayer(createPlayerDto);
        return Ok(player);
    }

    [HttpGet]
    [Route("")]
    public ActionResult<List<PlayerDto>> GetAllPlayers(){
        var players = appService.GetAllPlayers();
        return Ok(players);
    }


    [HttpPut]
    [Route("{playerId}")]
    public IActionResult UpdatePlayer(Guid playerId, PlayerDto playerDto){
        if (playerId != playerDto.PlayerId){
            return BadRequest("Player ID mismatch.");
        }

        try{
            appService.UpdatePlayer(playerDto);
            return NoContent(); 
        }
        catch (Exception ex){
            return NotFound(new { Message = ex.Message });
        }
    }

    [HttpGet]
    [Route("games/{gameId}")]
    public ActionResult<List<PlayerDto>> GetPlayersForGame(Guid gameId){
        var players = appService.GetPlayersForGame(gameId);
        return Ok(players);
    }
    

}