using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string Name { get; set; } = null!;

    public string? ContactPerson { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Wine> Wines { get; set; } = new List<Wine>();
}
