using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class WarehouseWine
{
    public int WarehouseWineId { get; set; }

    public int WareHouseId { get; set; }

    public int WineId { get; set; }

    public int? Quantity { get; set; }

    public string? Description { get; set; }

    public virtual WareHouse WareHouse { get; set; } = null!;

    public virtual Wine Wine { get; set; } = null!;
}
