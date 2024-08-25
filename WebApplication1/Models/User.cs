using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class User
{
    public int Id { get; set; }

    public string UserTag { get; set; }

    public string Password { get; set; }

    public byte[]? Salt { get; set; }

    public virtual ICollection<SessionToken> Sessiontokens { get; set; } = new List<SessionToken>();
}
