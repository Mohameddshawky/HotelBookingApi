using AutoMapper;
using HotelBookingApi.Application.DTOs;
using HotelBookingApi.Application.Interfaces.Repositories;
using HotelBookingApi.Application.Interfaces.Services;
using HotelBookingApi.Domain.Entities;
using HotelBookingApi.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBookingApi.Application.Services;

public class GuestService : IGuestService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GuestService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GuestDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var guest = await _unitOfWork.Guests.GetByIdAsync(id);
        return guest == null ? null : _mapper.Map<GuestDto>(guest);
    }

    public async Task<GuestDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var guest = await _unitOfWork.Guests.GetByEmailAsync(email);
        return guest == null ? null : _mapper.Map<GuestDto>(guest);
    }

    public async Task<Guid> CreateAsync(CreateOrUpdateGuestDto dto, CancellationToken cancellationToken = default)
    {
        var guest = _mapper.Map<Guest>(dto);
        await _unitOfWork.Guests.AddAsync(guest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return guest.Id;
    }

    public async Task UpdateAsync(Guid id, CreateOrUpdateGuestDto dto, CancellationToken cancellationToken = default)
    {
        var guest = await _unitOfWork.Guests.GetByIdAsync(id);
        if (guest == null) throw new NotFoundException("Guest not found");

        _mapper.Map(dto, guest);
        _unitOfWork.Guests.Update(guest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<BookingHistoryDto>> GetBookingHistoryAsync(Guid guestId, CancellationToken cancellationToken = default)
    {
        var bookings = await _unitOfWork.Bookings.GetBookingsByGuestIdAsync(guestId);
        return _mapper.Map<IEnumerable<BookingHistoryDto>>(bookings);
    }
}
