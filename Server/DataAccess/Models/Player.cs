using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Player
{
    public Guid Playerid { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string Password { get; set; } = null!;

    public bool Isadmin { get; set; }

    public bool Isactive { get; set; }

    public decimal Balance { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual ICollection<Board> Boards { get; set; } = new List<Board>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<Winner> Winners { get; set; } = new List<Winner>();
}
