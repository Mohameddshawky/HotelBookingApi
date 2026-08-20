using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotelBookingApi.Domain.Entities;
using HotelBookingApi.Application.DTOs;
using HotelBookingApi.Application.Interfaces.Repositories;
using HotelBookingApi.Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;

namespace HotelBookingApi.Application.Services;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    private const string OccupancyCacheKey = "OccupancyReport";

    public ReportService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<IEnumerable<AvailableRoomDto>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut, CancellationToken cancellationToken = default)
    {
        var availableRooms = await _unitOfWork.Rooms.GetAvailableRoomsAsync(checkIn, checkOut);
        return _mapper.Map<IEnumerable<AvailableRoomDto>>(availableRooms);
    }

    public async Task<OccupancyReportDto> GetOccupancyReportAsync(CancellationToken cancellationToken = default)
    {
        // Decision: Using a short expiration only (5 minutes) instead of manual invalidation.
        // Justification: The occupancy report aggregates data across all rooms and active bookings.
        // It can be affected by numerous write paths: creating/deactivating rooms, making new bookings,
        // and checking in/out guests. Manually invalidating the cache across all these disparate 
        // handlers and services adds immense coupling and complexity. A 5-minute cache provides a massive 
        // reduction in DB load for this heavy query while keeping the staleness window acceptable for a 
        // general managerial report.
        if (!_cache.TryGetValue(OccupancyCacheKey, out OccupancyReportDto? report))
        {
            var rooms = await _unitOfWork.Rooms.GetAllAsync();
            var totalRooms = rooms.Count();
            
            var activeBookings = await _unitOfWork.Bookings.GetActiveBookingsAsync();
        
            var occupiedRooms = activeBookings.Count(b => b.Status == Domain.Enums.BookingStatus.CheckedIn);

            report = new OccupancyReportDto
            {
                TotalRooms = totalRooms,
                OccupiedRooms = occupiedRooms,
                OccupancyPercentage = totalRooms > 0 ? (decimal)occupiedRooms / totalRooms * 100 : 0
            };

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            _cache.Set(OccupancyCacheKey, report, cacheOptions);
        }

        return report!;
    }

    public async Task<IEnumerable<RoomTypeRatingReportDto>> GetRoomTypeRatingsAsync(CancellationToken cancellationToken = default)
    {
        var roomTypes = await _unitOfWork.RoomTypes.GetAllAsync();
        var report = new List<RoomTypeRatingReportDto>();

        foreach (var roomType in roomTypes)
        {
            var reviews = await _unitOfWork.Reviews.GetReviewsByRoomTypeIdAsync(roomType.Id);
            if (reviews.Any())
            {
                report.Add(new RoomTypeRatingReportDto
                {
                    RoomTypeId = roomType.Id,
                    RoomTypeName = roomType.Name,
                    AverageRating = reviews.Average(r => r.Rating)
                });
            }
        }

        return report;
    }

    public async Task<IEnumerable<BookingHistoryDto>> GetGuestBookingHistoryAsync(Guid guestId, CancellationToken cancellationToken = default)
    {
        var bookings = await _unitOfWork.Bookings.GetBookingsByGuestIdAsync(guestId);
        return _mapper.Map<IEnumerable<BookingHistoryDto>>(bookings);
    }
}
