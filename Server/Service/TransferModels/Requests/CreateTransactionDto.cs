using DataAccess.Models;

public class CreateTransactionDto{
    public Guid Playerid { get; set; }
    public string Transactiontype { get; set; } = null!;
    public decimal Amount { get; set; }
    public decimal Balanceaftertransaction { get; set; }
    public string? Description { get; set; }
    public bool Isconfirmed { get; set; }

    public Transaction ToTransaction(){
        return new Transaction{
            Playerid = Playerid,
            Transactiontype = Transactiontype,
            Amount = Amount,
            Balanceaftertransaction = Balanceaftertransaction,
            Description = Description,
            Isconfirmed = Isconfirmed
        };
    }
    
}