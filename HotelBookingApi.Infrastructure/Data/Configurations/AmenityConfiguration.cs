using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HotelBookingApi.Domain.Entities;

namespace HotelBookingApi.Infrastructure.Data.Configurations;

public class AmenityConfiguration : IEntityTypeConfiguration<Amenity>
{
    public void Configure(EntityTypeBuilder<Amenity> builder)
    {
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Description)
            .HasMaxLength(500);
            
        builder.HasMany(a => a.RoomTypeAmenities)
            .WithOne(rta => rta.Amenity)
            .HasForeignKey(rta => rta.AmenityId);
    }
}
