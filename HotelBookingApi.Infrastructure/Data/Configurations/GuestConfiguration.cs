using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HotelBookingApi.Domain.Entities;

namespace HotelBookingApi.Infrastructure.Data.Configurations;

public class GuestConfiguration : IEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(g => g.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(g => g.Email)
            .IsUnique();

        builder.Property(g => g.PhoneNumber)
            .HasMaxLength(50);
            
        // Relationships
        builder.HasMany(g => g.Bookings)
            .WithOne(b => b.Guest)
            .HasForeignKey(b => b.GuestId);
    }
}
