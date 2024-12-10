using System;
using System.Collections.Generic;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public partial class AppDbContext : IdentityDbContext<Player, IdentityRole<Guid>, Guid>
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Board> Boards { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Winner> Winners { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=postgres_db;Port=5432;Database=DeadPigeonsDb;Username=RataTech;Password=1127344");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 

        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<IdentityUserLogin<Guid>>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
        });

        modelBuilder.Entity<IdentityUserRole<Guid>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });
        });

        modelBuilder.Entity<IdentityUserToken<Guid>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
        });

        modelBuilder.Entity<Board>(entity =>
        {
            entity.HasKey(e => e.Boardid).HasName("board_pkey");

            entity.ToTable("board");

            entity.Property(e => e.Boardid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("boardid");
            entity.Property(e => e.Autoplayweeks)
                .HasDefaultValue(0)
                .HasColumnName("autoplayweeks");
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
            entity.HasKey(e => e.Id).HasName("player_pkey"); 

            entity.ToTable("player");

            entity.Property(e => e.Balance)
                .HasPrecision(10, 2)
                .HasColumnName("balance");

            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");

            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");

            entity.Property(e => e.Isadmin)
                .HasDefaultValue(false)
                .HasColumnName("isadmin");

            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Transactionid).HasName("transaction_pkey");

            entity.ToTable("transaction");

            entity.Property(e => e.Transactionid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("transactionid");
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
            entity.Property(e => e.Isconfirmed)
                .HasDefaultValue(false)
                .HasColumnName("isconfirmed");
            entity.Property(e => e.Playerid).HasColumnName("playerid");
            entity.Property(e => e.Transactiontype)
                .HasMaxLength(50)
                .HasColumnName("transactiontype");

            entity.HasOne(d => d.Player).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.Playerid)
                .HasConstraintName("transaction_playerid_fkey");
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
