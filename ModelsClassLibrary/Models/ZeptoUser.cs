using System;
using System.Collections.Generic;

namespace ModelsClassLibrary.Models;

public partial class ZeptoUser
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string? Address { get; set; }

    public long Phone { get; set; }

    public string UserType { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }

    public virtual Cart? Cart { get; set; }
}
