using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Microblogging.Data.Entities;

public partial class microbloggingContext : DbContext
{
    public microbloggingContext()
    {
    }

    public microbloggingContext(DbContextOptions<microbloggingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<ImageVariant> ImageVariants { get; set; }

    public virtual DbSet<LkImageStatus> LkImageStatuses { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Initial Catalog=microblogging;TrustServerCertificate=True;encrypt=false;Persist Security Info=True;User ID=sa;Password=123456");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Image>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.FkPostId).HasColumnName("FK_PostId");
            entity.Property(e => e.FkStatus).HasColumnName("FK_Status");

            entity.HasOne(d => d.FkPost).WithMany(p => p.Images)
                .HasForeignKey(d => d.FkPostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Image_Post");

            entity.HasOne(d => d.FkStatusNavigation).WithMany(p => p.Images)
                .HasForeignKey(d => d.FkStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Image_Status");
        });

        modelBuilder.Entity<ImageVariant>(entity =>
        {
            entity.Property(e => e.FkImageId).HasColumnName("FK_ImageId");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.WebpUrl).HasColumnName("webp_URL");
            entity.Property(e => e.Width).HasColumnName("width");

            entity.HasOne(d => d.FkImage).WithMany(p => p.ImageVariants)
                .HasForeignKey(d => d.FkImageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ImageVariants_Image");
        });

        modelBuilder.Entity<LkImageStatus>(entity =>
        {
            entity.ToTable("LK_ImageStatus");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.FkUserId).HasColumnName("FK_UserId");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.Text).HasMaxLength(140);

            entity.HasOne(d => d.FkUser).WithMany(p => p.Posts)
                .HasForeignKey(d => d.FkUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Post_User");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshToken");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FkUserId).HasColumnName("FK_UserId");
            entity.Property(e => e.RefreshToken1).HasColumnName("RefreshToken");

            entity.HasOne(d => d.FkUser).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.FkUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Token");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
