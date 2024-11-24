using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Service;

[ApiController]  
[Route("api/[controller]")] 
public class BalanceController(IAppService appService) : ControllerBase{
    [HttpPost]
    [Route("")]
    public ActionResult<Playerbalance> CreatePlayerBalance(CreateBalanceDto createBalanceDto){
         var balance = appService.CreatePlayerBalance(createBalanceDto);
         return Ok(balance);
    }

}