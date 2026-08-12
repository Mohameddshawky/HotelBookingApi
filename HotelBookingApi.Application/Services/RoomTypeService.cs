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

public class RoomTypeService : IRoomTypeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RoomTypeService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoomTypeDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var roomTypes = await _unitOfWork.RoomTypes.GetAllAsync();
        return _mapper.Map<IEnumerable<RoomTypeDto>>(roomTypes);
    }

    public async Task<RoomTypeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var roomType = await _unitOfWork.RoomTypes.GetByIdAsync(id);
        return roomType == null ? null : _mapper.Map<RoomTypeDto>(roomType);
    }

    public async Task<Guid> CreateAsync(CreateOrUpdateRoomTypeDto dto, CancellationToken cancellationToken = default)
    {
        var isUnique = await _unitOfWork.RoomTypes.IsNameUniqueAsync(dto.Name);
        if (!isUnique) throw new BadRequestException("RoomType name must be unique");

        var roomType = _mapper.Map<RoomType>(dto);
        await _unitOfWork.RoomTypes.AddAsync(roomType);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return roomType.Id;
    }

    public async Task UpdateAsync(Guid id, CreateOrUpdateRoomTypeDto dto, CancellationToken cancellationToken = default)
    {
        var roomType = await _unitOfWork.RoomTypes.GetByIdAsync(id);
        if (roomType == null) throw new NotFoundException("RoomType not found");

        _mapper.Map(dto, roomType);
        _unitOfWork.RoomTypes.Update(roomType);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
