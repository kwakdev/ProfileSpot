using System;
using System.Collections.Generic;

namespace ProfileSpotDAL;

public partial class UserLogin
{
    public int LoginId { get; set; }

    public int? UserId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public DateTime? LastLogin { get; set; }

    public virtual UserProfile? User { get; set; }
}
