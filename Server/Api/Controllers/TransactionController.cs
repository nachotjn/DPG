using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Service;

[ApiController]  
[Route("api/[controller]")] 
public class TransactionController(IAppService appService) : ControllerBase{
    [HttpPost]
    [Route("")]
    public ActionResult<Transaction> CreateTransaction(CreateTransactionDto createTransactionDto){
         var transaction = appService.CreateTransaction(createTransactionDto);
         return Ok(transaction);
    }

}