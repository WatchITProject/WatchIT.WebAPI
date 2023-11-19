using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WatchIT.WebAPI.Database;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountRefreshToken> AccountRefreshTokens { get; set; }

    public virtual DbSet<ActorMovie> ActorMovies { get; set; }

    public virtual DbSet<ActorSeries> ActorSeries { get; set; }

    public virtual DbSet<DirectorMovie> DirectorMovies { get; set; }

    public virtual DbSet<DirectorSeries> DirectorSeries { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<MovieGenre> MovieGenres { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<RatingActorMovie> RatingActorMovies { get; set; }

    public virtual DbSet<RatingActorSeries> RatingActorSeries { get; set; }

    public virtual DbSet<RatingDirectorMovie> RatingDirectorMovies { get; set; }

    public virtual DbSet<RatingDirectorSeries> RatingDirectorSeries { get; set; }

    public virtual DbSet<RatingMovie> RatingMovies { get; set; }

    public virtual DbSet<RatingSeries> RatingSeries { get; set; }

    public virtual DbSet<RatingSeriesEpisode> RatingSeriesEpisodes { get; set; }

    public virtual DbSet<RatingSeriesSeason> RatingSeriesSeasons { get; set; }

    public virtual DbSet<Series> Series { get; set; }

    public virtual DbSet<SeriesEpisode> SeriesEpisodes { get; set; }

    public virtual DbSet<SeriesGenre> SeriesGenres { get; set; }

    public virtual DbSet<SeriesSeason> SeriesSeasons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=database");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.BackgroundPicture)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(320)
                .IsUnicode(false);
            entity.Property(e => e.LastLoginDate).HasColumnType("datetime");
            entity.Property(e => e.PasswordChangeDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProfilePicture)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Salt)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(320)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AccountRefreshToken>(entity =>
        {
            entity.ToTable("AccountRefreshToken");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.AccountRefreshTokens)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountRefreshToken_Account");
        });

        modelBuilder.Entity<ActorMovie>(entity =>
        {
            entity.ToTable("ActorMovie");

            entity.Property(e => e.Role)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Movie).WithMany(p => p.ActorMovies)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActorMovie_Movie");

            entity.HasOne(d => d.Person).WithMany(p => p.ActorMovies)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActorMovie_Person");
        });

        modelBuilder.Entity<ActorSeries>(entity =>
        {
            entity.Property(e => e.Role)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Person).WithMany(p => p.ActorSeries)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActorSeries_Person");

            entity.HasOne(d => d.Series).WithMany(p => p.ActorSeries)
                .HasForeignKey(d => d.SeriesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActorSeries_Movie");
        });

        modelBuilder.Entity<DirectorMovie>(entity =>
        {
            entity.ToTable("DirectorMovie");

            entity.HasOne(d => d.Movie).WithMany(p => p.DirectorMovies)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DirectorMovie_Movie");

            entity.HasOne(d => d.Person).WithMany(p => p.DirectorMovies)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DirectorMovie_Person");
        });

        modelBuilder.Entity<DirectorSeries>(entity =>
        {
            entity.HasOne(d => d.Person).WithMany(p => p.DirectorSeries)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DirectorSeries_Person");

            entity.HasOne(d => d.Series).WithMany(p => p.DirectorSeries)
                .HasForeignKey(d => d.SeriesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DirectorSeries_Movie");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.ToTable("Genre");

            entity.Property(e => e.Descripiton).IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("Movie");

            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.OriginalTitle)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.ReleaseDate).HasColumnType("date");
            entity.Property(e => e.Title)
                .HasMaxLength(256)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MovieGenre>(entity =>
        {
            entity.ToTable("MovieGenre");

            entity.HasOne(d => d.Genre).WithMany(p => p.MovieGenres)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieGenre_Genre");

            entity.HasOne(d => d.Movie).WithMany(p => p.MovieGenres)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieGenre_Movie");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("Person");

            entity.Property(e => e.Birthday).HasColumnType("date");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RatingActorMovie>(entity =>
        {
            entity.ToTable("RatingActorMovie");

            entity.HasOne(d => d.Account).WithMany(p => p.RatingActorMovies)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingActorMovie_Account");

            entity.HasOne(d => d.ActorMovie).WithMany(p => p.RatingActorMovies)
                .HasForeignKey(d => d.ActorMovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingActorMovie_ActorMovie");
        });

        modelBuilder.Entity<RatingActorSeries>(entity =>
        {
            entity.HasOne(d => d.Account).WithMany(p => p.RatingActorSeries)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingActorSeries_Account");

            entity.HasOne(d => d.ActorSeries).WithMany(p => p.RatingActorSeries)
                .HasForeignKey(d => d.ActorSeriesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingActorSeries_ActorSeries");
        });

        modelBuilder.Entity<RatingDirectorMovie>(entity =>
        {
            entity.ToTable("RatingDirectorMovie");

            entity.HasOne(d => d.Account).WithMany(p => p.RatingDirectorMovies)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingDirectorMovie_Account");

            entity.HasOne(d => d.DirectorMovie).WithMany(p => p.RatingDirectorMovies)
                .HasForeignKey(d => d.DirectorMovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingDirectorMovie_Director");
        });

        modelBuilder.Entity<RatingDirectorSeries>(entity =>
        {
            entity.HasOne(d => d.Account).WithMany(p => p.RatingDirectorSeries)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingDirectorSeries_Account");

            entity.HasOne(d => d.DirectorSeries).WithMany(p => p.RatingDirectorSeries)
                .HasForeignKey(d => d.DirectorSeriesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingDirectorSeries_Director");
        });

        modelBuilder.Entity<RatingMovie>(entity =>
        {
            entity.ToTable("RatingMovie");

            entity.HasOne(d => d.Account).WithMany(p => p.RatingMovies)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingMovie_Account");

            entity.HasOne(d => d.Movie).WithMany(p => p.RatingMovies)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingMovie_Actor");
        });

        modelBuilder.Entity<RatingSeries>(entity =>
        {
            entity.HasOne(d => d.Account).WithMany(p => p.RatingSeries)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingSeries_Account");

            entity.HasOne(d => d.Series).WithMany(p => p.RatingSeries)
                .HasForeignKey(d => d.SeriesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingSeries_Series");
        });

        modelBuilder.Entity<RatingSeriesEpisode>(entity =>
        {
            entity.ToTable("RatingSeriesEpisode");

            entity.HasOne(d => d.Account).WithMany(p => p.RatingSeriesEpisodes)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingSeriesEpisode_Account");

            entity.HasOne(d => d.SeriesEpisode).WithMany(p => p.RatingSeriesEpisodes)
                .HasForeignKey(d => d.SeriesEpisodeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingSeriesEpisode_SeriesEpisode");
        });

        modelBuilder.Entity<RatingSeriesSeason>(entity =>
        {
            entity.ToTable("RatingSeriesSeason");

            entity.HasOne(d => d.Account).WithMany(p => p.RatingSeriesSeasons)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingSeriesSeason_Account");

            entity.HasOne(d => d.SeriesSeason).WithMany(p => p.RatingSeriesSeasons)
                .HasForeignKey(d => d.SeriesSeasonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingSeriesSeason_SeriesSeason");
        });

        modelBuilder.Entity<SeriesEpisode>(entity =>
        {
            entity.ToTable("SeriesEpisode");

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Season).WithMany(p => p.SeriesEpisodes)
                .HasForeignKey(d => d.SeasonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SeriesEpisode_Season");
        });

        modelBuilder.Entity<SeriesGenre>(entity =>
        {
            entity.ToTable("SeriesGenre");

            entity.HasOne(d => d.Genre).WithMany(p => p.SeriesGenres)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SeriesGenre_Genre");

            entity.HasOne(d => d.Series).WithMany(p => p.SeriesGenres)
                .HasForeignKey(d => d.SeriesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SeriesGenre_Movie");
        });

        modelBuilder.Entity<SeriesSeason>(entity =>
        {
            entity.ToTable("SeriesSeason");

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Series).WithMany(p => p.SeriesSeasons)
                .HasForeignKey(d => d.SeriesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SeriesSeason_Series");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
