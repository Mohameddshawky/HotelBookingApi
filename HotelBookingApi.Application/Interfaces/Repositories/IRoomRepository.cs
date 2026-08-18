using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingApi.Domain.Entities;

namespace HotelBookingApi.Application.Interfaces.Repositories;

public interface IRoomRepository : IGenericRepository<Room>
{
    Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut);
    Task<Room?> GetByRoomNumberAsync(string roomNumber);
    Task<IEnumerable<Room>> GetRoomsByRoomTypeIdAsync(Guid roomTypeId);
    Task<(IEnumerable<Room> Items, int TotalCount)> GetPagedRoomsAsync(int pageNumber, int pageSize, Guid? roomTypeId = null, HotelBookingApi.Application.Strategies.Sorting.IRoomSortStrategy? sortStrategy = null);
}
