using AutoMapper;
using HotelBookingApi.Application.DTOs;
using HotelBookingApi.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBookingApi.Features.Bookings.GetAllBookings;

public class GetAllBookingsHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllBookingsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookingHistoryDto>> Handle(GetAllBookingsQuery query, CancellationToken cancellationToken)
    {
        var bookings = await _unitOfWork.Bookings.GetAllAsync();
        return _mapper.Map<IEnumerable<BookingHistoryDto>>(bookings);
    }
}
