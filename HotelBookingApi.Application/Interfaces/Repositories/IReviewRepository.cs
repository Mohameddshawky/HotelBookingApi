using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingApi.Domain.Entities;

namespace HotelBookingApi.Application.Interfaces.Repositories;

public interface IReviewRepository : IGenericRepository<Review>
{
    Task<bool> ExistsForBookingAsync(Guid bookingId);
    Task<IEnumerable<Review>> GetReviewsByRoomTypeIdAsync(Guid roomTypeId);
    Task<Review?> GetByBookingIdAsync(Guid bookingId);
}
