using System;
using System.Collections.Generic;

namespace CrowdsourcingPlatform.Models;

public partial class Bid
{
    public int BidID { get; set; }

    public int TaskID { get; set; }

    public int ExecutorID { get; set; }

    public string? Proposal { get; set; }

    public decimal? BidAmount { get; set; }

    public DateTime? BidDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Executor Executor { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;
}
