using System;
using System.Collections.Generic;

namespace CrowdsourcingPlatform.Models;

public partial class Executor
{
    public int ExecutorID { get; set; }

    public string? FullName { get; set; }

    public string Nickname { get; set; } = null!;

    public string? SkillsDescription { get; set; }

    public decimal? Rating { get; set; }

    public int? ReviewsCount { get; set; }

    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();

    public virtual ICollection<ExecutionTask> ExecutionTasks { get; set; } = new List<ExecutionTask>();
}
