﻿using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Request
{
    public int RequestId { get; set; }

    public int? AccountId { get; set; }

    public string? Status { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<RequestDetail> RequestDetails { get; set; } = new List<RequestDetail>();
}
