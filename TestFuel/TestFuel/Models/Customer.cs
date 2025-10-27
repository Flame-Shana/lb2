using System;
using System.Collections.Generic;

namespace CrowdsourcingPlatform.Models;

public partial class Customer
{
    public int CustomerID { get; set; }

    public string CustomerName { get; set; } = null!;

    public string? ContactEmail { get; set; }

    public string? ContactPhone { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
