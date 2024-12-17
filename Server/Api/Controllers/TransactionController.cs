using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;

[ApiController]  
[Route("api/[controller]")] 
public class TransactionController(IAppService appService) : ControllerBase{
    [HttpPost]
    [Route("")]
    [Authorize]
    public ActionResult<TransactionDto> CreateTransaction(CreateTransactionDto createTransactionDto){
         var transaction = appService.CreateTransaction(createTransactionDto);
         return Ok(transaction);
    }

    [HttpGet]
    [Route("player/{playerId}")]
    [Authorize]
    public ActionResult<List<TransactionDto>> GetTransactionsForPlayer(Guid playerId){
        var transactions = appService.GetTransactionsForPlayer(playerId);
        return Ok(transactions);
    }

    [HttpPut]
    [Route("{transactionId}/transactionStatus")]
    [Authorize(Roles = "Admin")]
    public IActionResult ChangeTransactionStatus(Guid transactionId, [FromBody] ChangeTransactionStatusDto request){
       var transaction = new TransactionDto();
       transaction.Transactionid = transactionId;

         try {
            bool status = request.Isconfirmed;
            appService.UpdateTransaction(transaction, status);
            return Ok(); 
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }

}