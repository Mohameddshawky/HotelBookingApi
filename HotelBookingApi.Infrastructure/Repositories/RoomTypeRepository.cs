using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotelBookingApi.Domain.Entities;
using HotelBookingApi.Application.Interfaces.Repositories;
using HotelBookingApi.Infrastructure.Data;

namespace HotelBookingApi.Infrastructure.Repositories;

public class RoomTypeRepository : GenericRepository<RoomType>, IRoomTypeRepository
{
    public RoomTypeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> IsNameUniqueAsync(string name)
    {
        return !await _dbSet.AnyAsync(rt => rt.Name == name);
    }

    public async Task<RoomType?> GetRoomTypeWithAmenitiesAsync(Guid id)
    {
        return await _dbSet
            .Include(rt => rt.RoomTypeAmenities)
            .ThenInclude(rta => rta.Amenity)
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Id == id);
    }

    public async Task<RoomType?> GetRoomTypeWithRoomsAsync(Guid id)
    {
        return await _dbSet
            .Include(rt => rt.Rooms)
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Id == id);
    }
}
