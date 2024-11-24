using DataAccess.Models;

public class CreateBalanceDto{
    public Guid Playerid { get; set; }

    public string Transactiontype { get; set; } = null!;

    public decimal Amount { get; set; }

    public decimal Balanceaftertransaction { get; set; }

    public string? Description { get; set; }

    public Playerbalance ToBalance(){
        return new Playerbalance{
            Playerid = Playerid,
            Transactiontype = Transactiontype,
            Amount = Amount,
            Balanceaftertransaction = Balanceaftertransaction,
            Description = Description
        };
    }
    
}