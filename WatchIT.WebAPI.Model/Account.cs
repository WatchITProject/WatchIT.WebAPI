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

    public string? ProfilePicture { get; set; }

    public string? BackgroundPicture { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public DateTime PasswordChangeDate { get; set; }

    public virtual ICollection<AccountRefreshToken> AccountRefreshTokens { get; set; } = new List<AccountRefreshToken>();

    public virtual ICollection<RatingActorMovie> RatingActorMovies { get; set; } = new List<RatingActorMovie>();

    public virtual ICollection<RatingActorSeries> RatingActorSeries { get; set; } = new List<RatingActorSeries>();

    public virtual ICollection<RatingDirectorMovie> RatingDirectorMovies { get; set; } = new List<RatingDirectorMovie>();

    public virtual ICollection<RatingDirectorSeries> RatingDirectorSeries { get; set; } = new List<RatingDirectorSeries>();

    public virtual ICollection<RatingMovie> RatingMovies { get; set; } = new List<RatingMovie>();

    public virtual ICollection<RatingSeries> RatingSeries { get; set; } = new List<RatingSeries>();

    public virtual ICollection<RatingSeriesEpisode> RatingSeriesEpisodes { get; set; } = new List<RatingSeriesEpisode>();

    public virtual ICollection<RatingSeriesSeason> RatingSeriesSeasons { get; set; } = new List<RatingSeriesSeason>();
}
