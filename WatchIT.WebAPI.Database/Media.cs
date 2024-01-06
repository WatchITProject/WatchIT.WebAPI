using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class Media
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public string? OriginalTitle { get; set; }

    public string? Description { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public short? Length { get; set; }

    public byte[]? PosterImage { get; set; }

    public string? PosterImageContentType { get; set; }

    public virtual ICollection<GenreMedia> GenreMedia { get; set; } = new List<GenreMedia>();

    public virtual MediaMovie? MediaMovie { get; set; }

    public virtual MediaSeries? MediaSeries { get; set; }

    public virtual ICollection<PersonActor> PersonActor { get; set; } = new List<PersonActor>();

    public virtual ICollection<PersonCreator> PersonCreator { get; set; } = new List<PersonCreator>();

    public virtual ICollection<RatingMedia> RatingMedia { get; set; } = new List<RatingMedia>();
}
