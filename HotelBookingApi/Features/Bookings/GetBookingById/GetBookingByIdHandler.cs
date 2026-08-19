using AutoMapper;
using HotelBookingApi.Application.DTOs;
using HotelBookingApi.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBookingApi.Features.Bookings.GetBookingById;

public class GetBookingByIdHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBookingByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BookingDetailsDto?> Handle(GetBookingByIdQuery query, CancellationToken cancellationToken)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(query.Id);
        return booking == null ? null : _mapper.Map<BookingDetailsDto>(booking);
    }
}
