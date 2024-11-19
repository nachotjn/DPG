using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Winner
{
    public Guid Winnerid { get; set; }

    public Guid Playerid { get; set; }

    public Guid Gameid { get; set; }

    public Guid Boardid { get; set; }

    public decimal Winningamount { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual Board Board { get; set; } = null!;

    public virtual Game Game { get; set; } = null!;

    public virtual Player Player { get; set; } = null!;
}
