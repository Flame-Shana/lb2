using System;
using System.Collections.Generic;

namespace CrowdsourcingPlatform.Models;

public partial class View_TaskExecutionPayment
{
    public int ExecutionTaskID { get; set; }

    public int TaskID { get; set; }

    public string? TaskTitle { get; set; }

    public string? TaskStatus { get; set; }

    public int? CustomerID { get; set; }

    public string? CustomerName { get; set; }

    public int ExecutorID { get; set; }

    public string? ExecutorNickname { get; set; }

    public DateTime? SubmittedDate { get; set; }

    public DateTime? CompletionDate { get; set; }

    public string ExecutionStatus { get; set; } = null!;

    public string? Result { get; set; }

    public int? PaymentID { get; set; }

    public decimal? PaymentAmount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? PaymentStatus { get; set; }
}
