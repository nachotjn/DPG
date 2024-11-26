using DataAccess.Models;

public class TransactionDto{
    public TransactionDto FromEntity(Transaction transaction){
        return new TransactionDto{
            Transactionid = transaction.Transactionid,
            Playerid = transaction.Playerid,
            Transactiontype = transaction.Transactiontype,
            Amount = transaction.Amount,
            Balanceaftertransaction = transaction.Balanceaftertransaction,
            Description = transaction.Description,
            Isconfirmed = transaction.Isconfirmed,
            Player = transaction.Player
        };
    }

    public Guid Transactionid { get; set; }
    public Guid Balanceid { get; set; }
    public Guid Playerid { get; set; }
    public string Transactiontype { get; set; } = null!;
    public decimal Amount { get; set; }
    public decimal Balanceaftertransaction { get; set; }
    public string? Description { get; set; }
    public bool Isconfirmed { get; set; }
    public virtual Player Player { get; set; } = null!;
}