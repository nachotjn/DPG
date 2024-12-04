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
    [Route("{winnerId}/winningAmount")]
    public IActionResult ChangeWinningAmount(Guid winnerId, [FromBody] ChangeWinningAmountDto request){
       var winner = new WinnerDto();
       winner.Winnerid = winnerId;

         try {
            decimal winningAmount = request.Winningamount;
            appService.UpdateWinner(winner, winningAmount);
            return Ok(); 
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }
}