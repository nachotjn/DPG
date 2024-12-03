using Microsoft.AspNetCore.Mvc;
using Service;

[ApiController]  
[Route("api/[controller]")] 
public class TransactionController(IAppService appService) : ControllerBase{
    [HttpPost]
    [Route("")]
    public ActionResult<TransactionDto> CreateTransaction(CreateTransactionDto createTransactionDto){
         var transaction = appService.CreateTransaction(createTransactionDto);
         return Ok(transaction);
    }

    [HttpGet]
    [Route("player/{playerId}")]
    public ActionResult<List<TransactionDto>> GetTransactionsForPlayer(Guid playerId){
        var transactions = appService.GetTransactionsForPlayer(playerId);
        return Ok(transactions);
    }

    [HttpPut]
    [Route("{transactionId}")]
    public IActionResult UpdateTransaction(Guid transactionId, TransactionDto transactionDto){
        if (transactionId != transactionDto.Transactionid){
            return BadRequest("Transaction ID mismatch.");
        }

        try{
            appService.UpdateTransaction(transactionDto);
            return NoContent(); 
        }
        catch (Exception ex){
            return NotFound(new { Message = ex.Message });
        }
    }

}