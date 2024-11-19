using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Board
{
    public Guid Boardid { get; set; }

    public Guid Playerid { get; set; }

    public Guid Gameid { get; set; }

    public List<int> Numbers { get; set; } = null!;

    public bool Isautoplay { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual Player Player { get; set; } = null!;

    public virtual ICollection<Winner> Winners { get; set; } = new List<Winner>();
}
