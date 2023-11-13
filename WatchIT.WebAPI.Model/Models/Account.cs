using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Model.Models;

public partial class Account
{
    public int AccId { get; set; }

    public string AccName { get; set; } = null!;

    public string AccEmail { get; set; } = null!;

    public byte[] AccPassword { get; set; } = null!;

    public string AccSalt { get; set; } = null!;

    public bool AccAdmin { get; set; }

    public DateTime AccCreationDate { get; set; }

    public DateTime? AccLastLoginDate { get; set; }

    public DateTime AccPasswordChangeDate { get; set; }

    public string? AccDescription { get; set; }

    public virtual ICollection<RatingActor> RatingActors { get; set; } = new List<RatingActor>();

    public virtual ICollection<RatingMovie> RatingMovies { get; set; } = new List<RatingMovie>();
}
