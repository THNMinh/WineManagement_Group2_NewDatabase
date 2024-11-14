﻿using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class WareHouse
{
    public int WareHouseId { get; set; }

    public string Address { get; set; } = null!;

    public string? Description { get; set; }

    public string? Status { get; set; }

    public string? ContactPerson { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual ICollection<WarehouseWine> WarehouseWines { get; set; } = new List<WarehouseWine>();
}