using System;
using System.Collections.Generic;

namespace ProfileSpotDAL;

public partial class UserProfile
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? AddressLine { get; set; }

    public string? City { get; set; }

    public string? Province { get; set; }

    public string? PostalCode { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public bool? IsAdmin { get; set; }

    public byte[]? Picture { get; set; }

    public byte[] Timer { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();
}
