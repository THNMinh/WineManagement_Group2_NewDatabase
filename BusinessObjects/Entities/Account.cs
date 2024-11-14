using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Account
{
    public int AccountId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? Status { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
