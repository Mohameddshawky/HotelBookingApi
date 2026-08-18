using System.Linq;
using HotelBookingApi.Domain.Entities;

namespace HotelBookingApi.Application.Strategies.Sorting;

public class SortByRatingStrategy : IRoomSortStrategy
{
    public IQueryable<Room> ApplySort(IQueryable<Room> query)
    {
        return query.OrderByDescending(r => 
            r.Bookings.Where(b => b.Review != null).Any()
                ? r.Bookings.Where(b => b.Review != null).Average(b => b.Review!.Rating) 
                : 0);
    }
}
