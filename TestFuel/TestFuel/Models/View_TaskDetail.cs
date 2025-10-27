using System;
using System.Collections.Generic;

namespace CrowdsourcingPlatform.Models;

public partial class View_TaskDetail
{
    public int TaskID { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? Deadline { get; set; }

    public decimal? Budget { get; set; }

    public string? Complexity { get; set; }

    public string? TaskStatus { get; set; }

    public int? CategoryID { get; set; }

    public string? CategoryName { get; set; }

    public int? CustomerID { get; set; }

    public string? CustomerName { get; set; }

    public int TotalBids { get; set; }

    public decimal? AvgBidAmount { get; set; }

    public decimal? MinBidAmount { get; set; }

    public decimal? MaxBidAmount { get; set; }

    public int? AcceptedBidID { get; set; }

    public int? AssignedExecutorID { get; set; }

    public string? AssignedExecutorNickname { get; set; }

    public decimal? AssignedBidAmount { get; set; }

    public DateTime? AcceptedBidDate { get; set; }
}
