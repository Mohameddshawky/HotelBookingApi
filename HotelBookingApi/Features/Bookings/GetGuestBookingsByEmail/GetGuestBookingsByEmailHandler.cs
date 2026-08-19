using AutoMapper;
using HotelBookingApi.Application.DTOs;
using HotelBookingApi.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBookingApi.Features.Bookings.GetGuestBookingsByEmail;

public class GetGuestBookingsByEmailHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetGuestBookingsByEmailHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookingHistoryDto>> Handle(GetGuestBookingsByEmailQuery query, CancellationToken cancellationToken)
    {
        var guest = await _unitOfWork.Guests.GetByEmailAsync(query.Email);
        if (guest == null) return Enumerable.Empty<BookingHistoryDto>();

        var bookings = await _unitOfWork.Bookings.GetBookingsByGuestIdAsync(guest.Id);
        return _mapper.Map<IEnumerable<BookingHistoryDto>>(bookings);
    }
}
