using System.Linq;
using HotelBookingApi.Domain.Entities;

namespace HotelBookingApi.Application.Strategies.Sorting;

public class SortByPriceStrategy : IRoomSortStrategy
{
    public IQueryable<Room> ApplySort(IQueryable<Room> query)
    {
        return query.OrderBy(r => r.RoomType!.PricePerNight);
    }
}
