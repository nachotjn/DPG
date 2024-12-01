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
    

}