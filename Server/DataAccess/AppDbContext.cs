using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Board> Boards { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Playerbalance> Playerbalances { get; set; }

    public virtual DbSet<Winner> Winners { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Board>(entity =>
        {
            entity.HasKey(e => e.Boardid).HasName("board_pkey");

            entity.ToTable("board");

            entity.Property(e => e.Boardid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("boardid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Gameid).HasColumnName("gameid");
            entity.Property(e => e.Isautoplay)
                .HasDefaultValue(false)
                .HasColumnName("isautoplay");
            entity.Property(e => e.Numbers).HasColumnName("numbers");
            entity.Property(e => e.Playerid).HasColumnName("playerid");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.Game).WithMany(p => p.Boards)
                .HasForeignKey(d => d.Gameid)
                .HasConstraintName("board_gameid_fkey");

            entity.HasOne(d => d.Player).WithMany(p => p.Boards)
                .HasForeignKey(d => d.Playerid)
                .HasConstraintName("board_playerid_fkey");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Gameid).HasName("game_pkey");

            entity.ToTable("game");

            entity.Property(e => e.Gameid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("gameid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Iscomplete)
                .HasDefaultValue(false)
                .HasColumnName("iscomplete");
            entity.Property(e => e.Prizesum)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("prizesum");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");
            entity.Property(e => e.Weeknumber).HasColumnName("weeknumber");
            entity.Property(e => e.Winningnumbers).HasColumnName("winningnumbers");
            entity.Property(e => e.Year).HasColumnName("year");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Playerid).HasName("player_pkey");

            entity.ToTable("player");

            entity.HasIndex(e => e.Email, "player_email_key").IsUnique();

            entity.Property(e => e.Playerid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("playerid");
            entity.Property(e => e.Annualfeepaid)
                .HasDefaultValue(false)
                .HasColumnName("annualfeepaid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(false)
                .HasColumnName("isactive");
            entity.Property(e => e.Isadmin)
                .HasDefaultValue(false)
                .HasColumnName("isadmin");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");
        });

        modelBuilder.Entity<Playerbalance>(entity =>
        {
            entity.HasKey(e => e.Balanceid).HasName("playerbalance_pkey");

            entity.ToTable("playerbalance");

            entity.Property(e => e.Balanceid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("balanceid");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.Balanceaftertransaction)
                .HasPrecision(10, 2)
                .HasColumnName("balanceaftertransaction");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Playerid).HasColumnName("playerid");
            entity.Property(e => e.Transactiontype)
                .HasMaxLength(50)
                .HasColumnName("transactiontype");

            entity.HasOne(d => d.Player).WithMany(p => p.Playerbalances)
                .HasForeignKey(d => d.Playerid)
                .HasConstraintName("playerbalance_playerid_fkey");
        });

        modelBuilder.Entity<Winner>(entity =>
        {
            entity.HasKey(e => e.Winnerid).HasName("winner_pkey");

            entity.ToTable("winner");

            entity.Property(e => e.Winnerid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("winnerid");
            entity.Property(e => e.Boardid).HasColumnName("boardid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Gameid).HasColumnName("gameid");
            entity.Property(e => e.Playerid).HasColumnName("playerid");
            entity.Property(e => e.Winningamount)
                .HasPrecision(10, 2)
                .HasColumnName("winningamount");

            entity.HasOne(d => d.Board).WithMany(p => p.Winners)
                .HasForeignKey(d => d.Boardid)
                .HasConstraintName("winner_boardid_fkey");

            entity.HasOne(d => d.Game).WithMany(p => p.Winners)
                .HasForeignKey(d => d.Gameid)
                .HasConstraintName("winner_gameid_fkey");

            entity.HasOne(d => d.Player).WithMany(p => p.Winners)
                .HasForeignKey(d => d.Playerid)
                .HasConstraintName("winner_playerid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
