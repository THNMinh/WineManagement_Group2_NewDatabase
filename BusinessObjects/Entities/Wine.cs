using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Wine
{
    public int WineId { get; set; }

    public string Name { get; set; } = null!;

    public int? VintageYear { get; set; }

    public decimal? AlcoholContent { get; set; }

    public decimal? Price { get; set; }

    public int? CategoryId { get; set; }

    public int? SupplierId { get; set; }

    public string? Status { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<RequestDetail> RequestDetails { get; set; } = new List<RequestDetail>();

    public virtual Supplier? Supplier { get; set; }

    public virtual ICollection<WarehouseWine> WarehouseWines { get; set; } = new List<WarehouseWine>();
}
