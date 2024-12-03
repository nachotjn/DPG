using Microsoft.AspNetCore.Mvc;
using Service;

[ApiController]  
[Route("api/[controller]")]  
public class WinnerController(IAppService appService) : ControllerBase{
    [HttpPost]
    [Route("")]
    public ActionResult<WinnerDto> CreateWinner(CreateWinnerDto createWinnerDto){
        var winner = appService.CreateWinner(createWinnerDto);
        return Ok(winner);
    }

    [HttpGet]
    [Route("")]
    public ActionResult<List<WinnerDto>> GetAllWinners(){
        var winners = appService.GetAllWinners();
        return Ok(winners);
    }

    [HttpGet]
    [Route("games/{gameId}")]
    public ActionResult<List<WinnerDto>> GetWinnersForGame(Guid gameId){
        var winners = appService.GetWinnersForGame(gameId);
        return Ok(winners);
    }

    [HttpPut]
    [Route("{winnerId}")]
    public IActionResult UpdateWinner(Guid winnerId, WinnerDto winnerDto){
        if (winnerId != winnerDto.Winnerid){
            return BadRequest("Winner ID mismatch.");
        }

        try{
            appService.UpdateWinner(winnerDto);
            return NoContent(); 
        }
        catch (Exception ex){
            return NotFound(new { Message = ex.Message });
        }
    }
}