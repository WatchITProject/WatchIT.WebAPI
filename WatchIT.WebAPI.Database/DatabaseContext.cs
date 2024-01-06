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

    public virtual DbSet<Account> Account { get; set; }

    public virtual DbSet<AccountBackgroundPicture> AccountBackgroundPicture { get; set; }

    public virtual DbSet<AccountProfilePicture> AccountProfilePicture { get; set; }

    public virtual DbSet<AccountRefreshToken> AccountRefreshToken { get; set; }

    public virtual DbSet<AuthBackgroundImage> AuthBackgroundImage { get; set; }

    public virtual DbSet<Genre> Genre { get; set; }

    public virtual DbSet<GenreMedia> GenreMedia { get; set; }

    public virtual DbSet<Media> Media { get; set; }

    public virtual DbSet<MediaMovie> MediaMovie { get; set; }

    public virtual DbSet<MediaSeries> MediaSeries { get; set; }

    public virtual DbSet<MediaSeriesEpisode> MediaSeriesEpisode { get; set; }

    public virtual DbSet<MediaSeriesSeason> MediaSeriesSeason { get; set; }

    public virtual DbSet<Person> Person { get; set; }

    public virtual DbSet<PersonActor> PersonActor { get; set; }

    public virtual DbSet<PersonCreator> PersonCreator { get; set; }

    public virtual DbSet<PersonCreatorType> PersonCreatorType { get; set; }

    public virtual DbSet<RatingMedia> RatingMedia { get; set; }

    public virtual DbSet<RatingMediaSeriesEpisode> RatingMediaSeriesEpisode { get; set; }

    public virtual DbSet<RatingMediaSeriesSeason> RatingMediaSeriesSeason { get; set; }

    public virtual DbSet<RatingPersonActor> RatingPersonActor { get; set; }

    public virtual DbSet<RatingPersonCreator> RatingPersonCreator { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=database");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(320)
                .IsUnicode(false);
            entity.Property(e => e.LastActiveDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PasswordChangeDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Salt)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(320)
                .IsUnicode(false);

            entity.HasOne(d => d.AccountBackgroundPicture).WithMany(p => p.Account)
                .HasForeignKey(d => d.AccountBackgroundPictureId)
                .HasConstraintName("FK_Account_AccountBackgroundPicture");

            entity.HasOne(d => d.AccountProfilePicture).WithMany(p => p.Account)
                .HasForeignKey(d => d.AccountProfilePictureId)
                .HasConstraintName("FK_Account_AccountProfilePicture");
        });

        modelBuilder.Entity<AccountBackgroundPicture>(entity =>
        {
            entity.Property(e => e.UploadDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<AccountProfilePicture>(entity =>
        {
            entity.Property(e => e.UploadDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<AccountRefreshToken>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.AccountRefreshToken)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountRefreshToken_Account");
        });

        modelBuilder.Entity<AuthBackgroundImage>(entity =>
        {
            entity.Property(e => e.ContentType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<GenreMedia>(entity =>
        {
            entity.HasOne(d => d.Genre).WithMany(p => p.GenreMedia)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GenreMedia_Genre");

            entity.HasOne(d => d.Media).WithMany(p => p.GenreMedia)
                .HasForeignKey(d => d.MediaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GenreMedia_Media");
        });

        modelBuilder.Entity<Media>(entity =>
        {
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.OriginalTitle)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.PosterImageContentType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(400)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MediaMovie>(entity =>
        {
            entity.HasIndex(e => e.MediaId, "UQ__MediaMov__B2C2B5CE2DDED6DE").IsUnique();

            entity.HasOne(d => d.Media).WithOne(p => p.MediaMovie)
                .HasForeignKey<MediaMovie>(d => d.MediaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MediaMovie_Media");
        });

        modelBuilder.Entity<MediaSeries>(entity =>
        {
            entity.HasIndex(e => e.MediaId, "UQ__MediaSer__B2C2B5CE267E98C0").IsUnique();

            entity.HasOne(d => d.Media).WithOne(p => p.MediaSeries)
                .HasForeignKey<MediaSeries>(d => d.MediaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MediaSeries_Media");
        });

        modelBuilder.Entity<MediaSeriesEpisode>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.MediaSeriesSeason).WithMany(p => p.MediaSeriesEpisode)
                .HasForeignKey(d => d.MediaSeriesSeasonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MediaSeriesEpisode_MediaSeriesSeason");
        });

        modelBuilder.Entity<MediaSeriesSeason>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.MediaSeries).WithMany(p => p.MediaSeriesSeason)
                .HasForeignKey(d => d.MediaSeriesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MediaSeriesSeason_MediaSeries");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PersonActor>(entity =>
        {
            entity.Property(e => e.Role)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Media).WithMany(p => p.PersonActor)
                .HasForeignKey(d => d.MediaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonActor_Media");

            entity.HasOne(d => d.Person).WithMany(p => p.PersonActor)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonActor_Person");
        });

        modelBuilder.Entity<PersonCreator>(entity =>
        {
            entity.HasOne(d => d.Media).WithMany(p => p.PersonCreator)
                .HasForeignKey(d => d.MediaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonCreator_Media");

            entity.HasOne(d => d.PersonCreatorType).WithMany(p => p.PersonCreator)
                .HasForeignKey(d => d.PersonCreatorTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonCreator_PersonCreatorType");

            entity.HasOne(d => d.Person).WithMany(p => p.PersonCreator)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonCreator_Person");
        });

        modelBuilder.Entity<PersonCreatorType>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RatingMedia>(entity =>
        {
            entity.HasOne(d => d.Account).WithMany(p => p.RatingMedia)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingMedia_Account");

            entity.HasOne(d => d.Media).WithMany(p => p.RatingMedia)
                .HasForeignKey(d => d.MediaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingMedia_Media");
        });

        modelBuilder.Entity<RatingMediaSeriesEpisode>(entity =>
        {
            entity.HasOne(d => d.Account).WithMany(p => p.RatingMediaSeriesEpisode)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingMediaSeriesEpisode_Account");

            entity.HasOne(d => d.MediaSeriesEpisode).WithMany(p => p.RatingMediaSeriesEpisode)
                .HasForeignKey(d => d.MediaSeriesEpisodeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingMediaSeriesEpisode_MediaSeriesEpisode");
        });

        modelBuilder.Entity<RatingMediaSeriesSeason>(entity =>
        {
            entity.HasOne(d => d.Account).WithMany(p => p.RatingMediaSeriesSeason)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingMediaSeriesSeason_Account");

            entity.HasOne(d => d.MediaSeriesSeason).WithMany(p => p.RatingMediaSeriesSeason)
                .HasForeignKey(d => d.MediaSeriesSeasonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingMediaSeriesSeason_SeriesSeason");
        });

        modelBuilder.Entity<RatingPersonActor>(entity =>
        {
            entity.HasOne(d => d.Account).WithMany(p => p.RatingPersonActor)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingPersonActor_Account");

            entity.HasOne(d => d.PersonActor).WithMany(p => p.RatingPersonActor)
                .HasForeignKey(d => d.PersonActorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingPersonActor_PersonActor");
        });

        modelBuilder.Entity<RatingPersonCreator>(entity =>
        {
            entity.HasOne(d => d.Account).WithMany(p => p.RatingPersonCreator)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingPersonCreator_Account");

            entity.HasOne(d => d.PersonCreator).WithMany(p => p.RatingPersonCreator)
                .HasForeignKey(d => d.PersonCreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingPersonCreator_PersonCreator");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
