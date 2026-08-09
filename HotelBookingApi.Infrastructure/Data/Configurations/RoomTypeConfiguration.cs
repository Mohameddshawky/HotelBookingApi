using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HotelBookingApi.Domain.Entities;

namespace HotelBookingApi.Infrastructure.Data.Configurations;

public class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
{
    public void Configure(EntityTypeBuilder<RoomType> builder)
    {
        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(rt => rt.PricePerNight)
            .HasColumnType("decimal(18,2)");
            
        // Relationships
        builder.HasMany(rt => rt.Rooms)
            .WithOne(r => r.RoomType)
            .HasForeignKey(r => r.RoomTypeId);
            
        builder.HasMany(rt => rt.RoomTypeAmenities)
            .WithOne(rta => rta.RoomType)
            .HasForeignKey(rta => rta.RoomTypeId);
    }
}
