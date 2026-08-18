using System.Linq;
using HotelBookingApi.Domain.Entities;

namespace HotelBookingApi.Application.Strategies.Sorting;

public interface IRoomSortStrategy
{
    IQueryable<Room> ApplySort(IQueryable<Room> query);
}
