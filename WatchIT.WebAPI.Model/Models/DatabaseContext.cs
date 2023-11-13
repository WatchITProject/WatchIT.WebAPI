using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WatchIT.WebAPI.Model.Models;

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

    public virtual DbSet<Actor> Actors { get; set; }

    public virtual DbSet<Director> Directors { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<MovieGenre> MovieGenres { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<RatingActor> RatingActors { get; set; }

    public virtual DbSet<RatingMovie> RatingMovies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=database");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccId);

            entity.ToTable("account");

            entity.Property(e => e.AccId).HasColumnName("acc_id");
            entity.Property(e => e.AccAdmin).HasColumnName("acc_admin");
            entity.Property(e => e.AccCreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("acc_creation_date");
            entity.Property(e => e.AccDescription)
                .IsUnicode(false)
                .HasColumnName("acc_description");
            entity.Property(e => e.AccEmail)
                .HasMaxLength(320)
                .IsUnicode(false)
                .HasColumnName("acc_email");
            entity.Property(e => e.AccLastLoginDate)
                .HasColumnType("datetime")
                .HasColumnName("acc_last_login_date");
            entity.Property(e => e.AccName)
                .HasMaxLength(320)
                .IsUnicode(false)
                .HasColumnName("acc_name");
            entity.Property(e => e.AccPassword).HasColumnName("acc_password");
            entity.Property(e => e.AccPasswordChangeDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("acc_password_change_date");
            entity.Property(e => e.AccSalt)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("acc_salt");
        });

        modelBuilder.Entity<Actor>(entity =>
        {
            entity.HasKey(e => e.ActId);

            entity.ToTable("actor");

            entity.Property(e => e.ActId).HasColumnName("act_id");
            entity.Property(e => e.ActMovId).HasColumnName("act_mov_id");
            entity.Property(e => e.ActPerId).HasColumnName("act_per_id");
            entity.Property(e => e.ActRole)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("act_role");

            entity.HasOne(d => d.ActMov).WithMany(p => p.Actors)
                .HasForeignKey(d => d.ActMovId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_actor_movie");

            entity.HasOne(d => d.ActPer).WithMany(p => p.Actors)
                .HasForeignKey(d => d.ActPerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_actor_person");
        });

        modelBuilder.Entity<Director>(entity =>
        {
            entity.HasKey(e => e.DirId);

            entity.ToTable("director");

            entity.Property(e => e.DirId).HasColumnName("dir_id");
            entity.Property(e => e.DirMovId).HasColumnName("dir_mov_id");
            entity.Property(e => e.DirPerId).HasColumnName("dir_per_id");

            entity.HasOne(d => d.DirMov).WithMany(p => p.Directors)
                .HasForeignKey(d => d.DirMovId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_director_movie");

            entity.HasOne(d => d.DirPer).WithMany(p => p.Directors)
                .HasForeignKey(d => d.DirPerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_director_person");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenId);

            entity.ToTable("genre");

            entity.Property(e => e.GenId).HasColumnName("gen_id");
            entity.Property(e => e.GenDescription)
                .IsUnicode(false)
                .HasColumnName("gen_description");
            entity.Property(e => e.GenName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("gen_name");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.MovId);

            entity.ToTable("movie");

            entity.Property(e => e.MovId).HasColumnName("mov_id");
        });

        modelBuilder.Entity<MovieGenre>(entity =>
        {
            entity.HasKey(e => e.MovgenId);

            entity.ToTable("movie_genre");

            entity.Property(e => e.MovgenId).HasColumnName("movgen_id");
            entity.Property(e => e.MovgenGenId).HasColumnName("movgen_gen_id");
            entity.Property(e => e.MovgenMovId).HasColumnName("movgen_mov_id");

            entity.HasOne(d => d.MovgenGen).WithMany(p => p.MovieGenres)
                .HasForeignKey(d => d.MovgenGenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_movie_genre_genre");

            entity.HasOne(d => d.MovgenMov).WithMany(p => p.MovieGenres)
                .HasForeignKey(d => d.MovgenMovId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_movie_genre_movie");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PerId);

            entity.ToTable("person");

            entity.Property(e => e.PerId).HasColumnName("per_id");
            entity.Property(e => e.PerBirthday)
                .HasColumnType("date")
                .HasColumnName("per_birthday");
            entity.Property(e => e.PerDescription)
                .IsUnicode(false)
                .HasColumnName("per_description");
            entity.Property(e => e.PerFullName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("per_full_name");
            entity.Property(e => e.PerName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("per_name");
        });

        modelBuilder.Entity<RatingActor>(entity =>
        {
            entity.HasKey(e => e.RatactId);

            entity.ToTable("rating_actor");

            entity.Property(e => e.RatactId).HasColumnName("ratact_id");
            entity.Property(e => e.RatactAccId).HasColumnName("ratact_acc_id");
            entity.Property(e => e.RatactActId).HasColumnName("ratact_act_id");
            entity.Property(e => e.RatactRating).HasColumnName("ratact_rating");

            entity.HasOne(d => d.RatactAcc).WithMany(p => p.RatingActors)
                .HasForeignKey(d => d.RatactAccId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_rating_actor_account");

            entity.HasOne(d => d.RatactAct).WithMany(p => p.RatingActors)
                .HasForeignKey(d => d.RatactActId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_rating_actor_actor");
        });

        modelBuilder.Entity<RatingMovie>(entity =>
        {
            entity.HasKey(e => e.RatmovId);

            entity.ToTable("rating_movie");

            entity.Property(e => e.RatmovId).HasColumnName("ratmov_id");
            entity.Property(e => e.RatmovAccId).HasColumnName("ratmov_acc_id");
            entity.Property(e => e.RatmovMovId).HasColumnName("ratmov_mov_id");
            entity.Property(e => e.RatmovRating).HasColumnName("ratmov_rating");

            entity.HasOne(d => d.RatmovAcc).WithMany(p => p.RatingMovies)
                .HasForeignKey(d => d.RatmovAccId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_rating_movie_account");

            entity.HasOne(d => d.RatmovMov).WithMany(p => p.RatingMovies)
                .HasForeignKey(d => d.RatmovMovId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_rating_movie_movie");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
