using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Wine> Wines { get; set; } = new List<Wine>();
}
