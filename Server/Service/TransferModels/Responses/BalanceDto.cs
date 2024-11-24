using DataAccess.Models;

public class BalanceDto{
    public BalanceDto FromEntity(Playerbalance balance){
        return new BalanceDto{
            Balanceid = balance.Balanceid,
            Playerid = balance.Playerid,
            Transactiontype = balance.Transactiontype,
            Amount = balance.Amount,
            Balanceaftertransaction = balance.Balanceaftertransaction,
            Description = balance.Description,
            Player = balance.Player
        };
    }

    public Guid Balanceid { get; set; }

    public Guid Playerid { get; set; }

    public string Transactiontype { get; set; } = null!;

    public decimal Amount { get; set; }

    public decimal Balanceaftertransaction { get; set; }

    public string? Description { get; set; }

    public virtual Player Player { get; set; } = null!;
}