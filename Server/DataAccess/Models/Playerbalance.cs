﻿using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Playerbalance
{
    public Guid Balanceid { get; set; }

    public Guid Playerid { get; set; }

    public string Transactiontype { get; set; } = null!;

    public decimal Amount { get; set; }

    public decimal Balanceaftertransaction { get; set; }

    public string? Description { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual Player Player { get; set; } = null!;
}