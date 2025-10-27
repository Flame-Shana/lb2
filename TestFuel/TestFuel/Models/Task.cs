using System;
using System.Collections.Generic;

namespace CrowdsourcingPlatform.Models;

public partial class Task
{
    public int TaskID { get; set; }

    public int CategoryID { get; set; }

    public int CustomerID { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? Deadline { get; set; }

    public decimal? Budget { get; set; }

    public string? Complexity { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();

    public virtual Category Category { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<ExecutionTask> ExecutionTasks { get; set; } = new List<ExecutionTask>();
}
