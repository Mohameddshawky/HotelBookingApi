using System;
using System.Threading;
using System.Threading.Tasks;
using HotelBookingApi.Application.DTOs;

namespace HotelBookingApi.Application.Interfaces.Services;

public interface IReviewService
{
    Task<ReviewDto?> GetByBookingIdAsync(Guid bookingId, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(SaveReviewDto dto, CancellationToken cancellationToken = default);
}
