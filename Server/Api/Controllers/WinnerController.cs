using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Service;

[ApiController]  
[Route("api/[controller]")]  
public class WinnerController(IAppService appService) : ControllerBase{
        [HttpPost]
        [Route("")]
        public ActionResult<Winner> CreateWinner(CreateWinnerDto createWinnerDto){
            var winner = appService.CreateWinner(createWinnerDto);
            return Ok(winner);
        }

        [HttpGet]
        [Route("")]
        public ActionResult<List<Winner>> GetAllWinners(){
            var winners = appService.GetAllWinners();
            return Ok(winners);
        }
}