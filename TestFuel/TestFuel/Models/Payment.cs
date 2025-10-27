using System;
using System.Collections.Generic;

namespace CrowdsourcingPlatform.Models;

public partial class Payment
{
    public int PaymentID { get; set; }

    public int ExecutionTaskID { get; set; }

    public decimal Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual ExecutionTask ExecutionTask { get; set; } = null!;
}
