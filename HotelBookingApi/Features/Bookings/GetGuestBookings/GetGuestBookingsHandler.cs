using AutoMapper;
using HotelBookingApi.Application.DTOs;
using HotelBookingApi.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBookingApi.Features.Bookings.GetGuestBookings;

public class GetGuestBookingsHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetGuestBookingsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookingHistoryDto>> Handle(GetGuestBookingsQuery query, CancellationToken cancellationToken)
    {
        var bookings = await _unitOfWork.Bookings.GetBookingsByGuestIdAsync(query.GuestId);
        return _mapper.Map<IEnumerable<BookingHistoryDto>>(bookings);
    }
}
