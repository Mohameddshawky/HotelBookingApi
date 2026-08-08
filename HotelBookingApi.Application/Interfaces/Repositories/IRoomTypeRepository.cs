using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBookingApi.Domain.Entities;

namespace HotelBookingApi.Application.Interfaces.Repositories;

public interface IRoomTypeRepository : IGenericRepository<RoomType>
{
    Task<bool> IsNameUniqueAsync(string name);
    Task<RoomType?> GetRoomTypeWithAmenitiesAsync(Guid id);
    Task<RoomType?> GetRoomTypeWithRoomsAsync(Guid id);
}
