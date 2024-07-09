using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProfileSpotDAL;

public partial class ProfileSpotContext : DbContext
{
    public ProfileSpotContext()
    {
    }

    public ProfileSpotContext(DbContextOptions<ProfileSpotContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=profilespot.database.windows.net;Database=profilespot;User Id=evankwak;Password=Ryankwak1;");
        optionsBuilder.UseLazyLoadingProxies(); // Disable lazy loading

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => e.LoginId).HasName("PK__UserLogi__4DDA28180F75A4B5");

            entity.ToTable("UserLogin");

            entity.HasIndex(e => e.Username, "UQ__UserLogi__536C85E4EE2DF8DF").IsUnique();

            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.UserLogins)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UserLogin_UserProfile");
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserProf__1788CC4C5D11B2D6");

            entity.ToTable("UserProfile");

            entity.HasIndex(e => e.Email, "UQ__UserProf__A9D105343D8FFF75").IsUnique();

            entity.Property(e => e.AddressLine).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.PostalCode).HasMaxLength(10);
            entity.Property(e => e.Province).HasMaxLength(100);
            entity.Property(e => e.Timer)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
