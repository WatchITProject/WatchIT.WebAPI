using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class Account
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public byte[] Password { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public bool Admin { get; set; }

    public string? Description { get; set; }

    public int? AccountProfilePictureId { get; set; }

    public int? AccountBackgroundPictureId { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime PasswordChangeDate { get; set; }

    public DateTime LastActiveDate { get; set; }

    public virtual AccountBackgroundPicture? AccountBackgroundPicture { get; set; }

    public virtual AccountProfilePicture? AccountProfilePicture { get; set; }

    public virtual ICollection<AccountRefreshToken> AccountRefreshToken { get; set; } = new List<AccountRefreshToken>();

    public virtual ICollection<RatingMedia> RatingMedia { get; set; } = new List<RatingMedia>();

    public virtual ICollection<RatingMediaSeriesEpisode> RatingMediaSeriesEpisode { get; set; } = new List<RatingMediaSeriesEpisode>();

    public virtual ICollection<RatingMediaSeriesSeason> RatingMediaSeriesSeason { get; set; } = new List<RatingMediaSeriesSeason>();

    public virtual ICollection<RatingPersonActor> RatingPersonActor { get; set; } = new List<RatingPersonActor>();

    public virtual ICollection<RatingPersonCreator> RatingPersonCreator { get; set; } = new List<RatingPersonCreator>();
}
