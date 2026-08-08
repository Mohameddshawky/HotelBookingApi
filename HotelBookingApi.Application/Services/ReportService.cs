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

namespace HotelBookingApi.Application.Services;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AvailableRoomDto>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut, CancellationToken cancellationToken = default)
    {
        var availableRooms = await _unitOfWork.Rooms.GetAvailableRoomsAsync(checkIn, checkOut);
        return _mapper.Map<IEnumerable<AvailableRoomDto>>(availableRooms);
    }

    public async Task<OccupancyReportDto> GetOccupancyReportAsync(CancellationToken cancellationToken = default)
    {
        var rooms = await _unitOfWork.Rooms.GetAllAsync();
        var totalRooms = rooms.Count();
        
        var activeBookings = await _unitOfWork.Bookings.GetActiveBookingsAsync();
        // Since GetActiveBookingsAsync gets Confirmed and CheckedIn, real occupancy is technically CheckedIn.
        // Let's assume occupied = currently CheckedIn
        var occupiedRooms = activeBookings.Count(b => b.Status == Domain.Enums.BookingStatus.CheckedIn);

        return new OccupancyReportDto
        {
            TotalRooms = totalRooms,
            OccupiedRooms = occupiedRooms,
            OccupancyPercentage = totalRooms > 0 ? (decimal)occupiedRooms / totalRooms * 100 : 0
        };
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
