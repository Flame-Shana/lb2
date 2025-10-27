using System;
using System.Collections.Generic;

namespace CrowdsourcingPlatform.Models;

public partial class View_ExecutorSummary
{
    public int ExecutorID { get; set; }

    public string? FullName { get; set; }

    public string Nickname { get; set; } = null!;

    public string? SkillsDescription { get; set; }

    public decimal? Rating { get; set; }

    public int? ReviewsCount { get; set; }

    public int TotalBids { get; set; }

    public int AcceptedBidsCount { get; set; }

    public int TotalExecutions { get; set; }

    public int ApprovedExecutions { get; set; }

    public decimal TotalPaidAmount { get; set; }

    public decimal? AvgPaymentAmount { get; set; }
}
