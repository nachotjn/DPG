using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Game
{
    public Guid Gameid { get; set; }

    public int Weeknumber { get; set; }

    public int Year { get; set; }

    public List<int>? Winningnumbers { get; set; }

    public bool Iscomplete { get; set; }

    public decimal? Prizesum { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual ICollection<Board> Boards { get; set; } = new List<Board>();

    public virtual ICollection<Winner> Winners { get; set; } = new List<Winner>();
}
