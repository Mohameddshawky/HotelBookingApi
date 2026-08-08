using HotelBookingApi.Domain.Entities;
using HotelBookingApi.Application.Interfaces.Repositories;
using HotelBookingApi.Infrastructure.Data;

namespace HotelBookingApi.Infrastructure.Repositories;

public class AmenityRepository : GenericRepository<Amenity>, IAmenityRepository
{
    public AmenityRepository(ApplicationDbContext context) : base(context)
    {
    }
}
