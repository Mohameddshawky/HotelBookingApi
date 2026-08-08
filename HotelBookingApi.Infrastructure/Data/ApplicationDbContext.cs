using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HotelBookingApi.Domain.Entities;

namespace HotelBookingApi.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<Staff>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<RoomType> RoomTypes { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<Guest> Guests { get; set; } = null!;
    public DbSet<Booking> Bookings { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<Amenity> Amenities { get; set; } = null!;
    public DbSet<RoomTypeAmenity> RoomTypeAmenities { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
