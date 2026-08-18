using AutoMapper;
using HotelBookingApi.Application.DTOs;
using HotelBookingApi.Application.Interfaces.Repositories;
using HotelBookingApi.Application.Interfaces.Services;
using HotelBookingApi.Application.Strategies.Sorting;
using HotelBookingApi.Domain.Entities;
using HotelBookingApi.Domain.Enums;
using HotelBookingApi.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBookingApi.Application.Services;

public class RoomService : IRoomService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly RoomSortStrategyFactory _sortFactory;

    public RoomService(IUnitOfWork unitOfWork, IMapper mapper, RoomSortStrategyFactory sortFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _sortFactory = sortFactory;
    }

    public async Task<PagedResult<RoomDto>> GetAllAsync(int pageNumber, int pageSize, Guid? roomTypeId = null, string? sortBy = null, CancellationToken cancellationToken = default)
    {
        var strategy = _sortFactory.GetStrategy(sortBy);
        var (items, totalCount) = await _unitOfWork.Rooms.GetPagedRoomsAsync(pageNumber, pageSize, roomTypeId, strategy);
        
        return new PagedResult<RoomDto>
        {
            Items = _mapper.Map<IEnumerable<RoomDto>>(items),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<RoomDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var room = await _unitOfWork.Rooms.GetByIdAsync(id);
        return room == null ? null : _mapper.Map<RoomDto>(room);
    }

    public async Task<IEnumerable<AvailableRoomDto>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut, CancellationToken cancellationToken = default)
    {
        var availableRooms = await _unitOfWork.Rooms.GetAvailableRoomsAsync(checkIn, checkOut);
        return _mapper.Map<IEnumerable<AvailableRoomDto>>(availableRooms);
    }

    public async Task<Guid> CreateAsync(CreateOrUpdateRoomDto dto, CancellationToken cancellationToken = default)
    {
        var existingRoom = await _unitOfWork.Rooms.GetByRoomNumberAsync(dto.Number);
        if (existingRoom != null) throw new Exception("Room number must be unique");

        var room = _mapper.Map<Room>(dto);
        room.Status = RoomStatus.Available;
        await _unitOfWork.Rooms.AddAsync(room);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return room.Id;
    }

    public async Task DeactivateAsync(Guid roomId, CancellationToken cancellationToken = default)
    {
        var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
        if (room == null) throw new NotFoundException("Room not found");

        room.Status = RoomStatus.Maintenance;
        _unitOfWork.Rooms.Update(room);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
