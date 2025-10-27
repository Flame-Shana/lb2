using System;
using System.Collections.Generic;

namespace CrowdsourcingPlatform.Models;

public partial class ExecutionTask
{
    public int ExecutionTaskID { get; set; }

    public int TaskID { get; set; }

    public int ExecutorID { get; set; }

    public DateTime? CompletionDate { get; set; }

    public string? Result { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? SubmittedDate { get; set; }

    public virtual Executor Executor { get; set; } = null!;

    public virtual Payment? Payment { get; set; }

    public virtual Task Task { get; set; } = null!;
}
