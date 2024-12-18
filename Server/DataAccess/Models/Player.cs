using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Models;

public partial class Player : IdentityUser<Guid> {
    public bool Isadmin { get; set; }

    public bool Isactive { get; set; }

    public decimal Balance { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual ICollection<Board> Boards { get; set; } = new List<Board>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<Winner> Winners { get; set; } = new List<Winner>();
}
