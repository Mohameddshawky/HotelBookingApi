using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HotelBookingApi.Domain.Entities;

namespace HotelBookingApi.Infrastructure.Data.Configurations;

public class RoomTypeAmenityConfiguration : IEntityTypeConfiguration<RoomTypeAmenity>
{
    public void Configure(EntityTypeBuilder<RoomTypeAmenity> builder)
    {
        // Composite Key
        builder.HasKey(rta => new { rta.RoomTypeId, rta.AmenityId });

        // Relationships
        builder.HasOne(rta => rta.RoomType)
            .WithMany(rt => rt.RoomTypeAmenities)
            .HasForeignKey(rta => rta.RoomTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rta => rta.Amenity)
            .WithMany(a => a.RoomTypeAmenities)
            .HasForeignKey(rta => rta.AmenityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
