using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class RequestDetail
{
    public int RequestDetailId { get; set; }

    public int? RequestId { get; set; }

    public int? WineId { get; set; }

    public int Quantity { get; set; }

    public virtual Request? Request { get; set; }

    public virtual Wine? Wine { get; set; }
}
