using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Infrastructure.Data;

public class SpaceRentDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public SpaceRentDbContext(DbContextOptions<SpaceRentDbContext> options) : base(options)
    {
    }

    public DbSet<Space> Spaces => Set<Space>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Amenity> Amenities => Set<Amenity>();
    public DbSet<SpaceAmenity> SpaceAmenities => Set<SpaceAmenity>();
    public DbSet<SpaceMedia> SpaceMedia => Set<SpaceMedia>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Space>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.PricePerHour).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Location).HasMaxLength(500);
            entity.Property(e => e.City).HasMaxLength(100);
            
            entity.HasIndex(e => e.City);
            entity.HasIndex(e => e.IsPublished);
            entity.HasIndex(e => e.Type);
            entity.HasIndex(e => e.PricePerHour);

            entity.HasOne(e => e.Owner)
                  .WithMany()
                  .HasForeignKey(e => e.OwnerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Amenity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Icon).HasMaxLength(50);
            entity.HasIndex(e => e.Name).IsUnique(); // Amenity names must be unique
        });

        modelBuilder.Entity<SpaceAmenity>(entity =>
        {
            entity.HasKey(e => new { e.SpaceId, e.AmenityId }); // Composite PK

            entity.HasOne(e => e.Space)
                  .WithMany(s => s.SpaceAmenities)
                  .HasForeignKey(e => e.SpaceId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Amenity)
                  .WithMany(a => a.SpaceAmenities)
                  .HasForeignKey(e => e.AmenityId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.HasOne(e => e.Space)
                  .WithMany(s => s.Bookings)
                  .HasForeignKey(e => e.SpaceId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<SpaceMedia>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Url).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.FileName).HasMaxLength(500);
            entity.HasIndex(e => e.SpaceId);
            entity.HasIndex(e => new { e.SpaceId, e.MediaType });

            entity.HasOne(e => e.Space)
                  .WithMany(s => s.Media)
                  .HasForeignKey(e => e.SpaceId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
