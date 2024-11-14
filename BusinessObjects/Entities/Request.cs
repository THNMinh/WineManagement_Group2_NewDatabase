using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Request
{
    public int RequestId { get; set; }

    public int? AccountId { get; set; }

    public string? Status { get; set; }

    public bool? Export { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<RequestDetail> RequestDetails { get; set; } = new List<RequestDetail>();
}
