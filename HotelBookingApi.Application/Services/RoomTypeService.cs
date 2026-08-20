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
using Microsoft.Extensions.Caching.Memory;

namespace HotelBookingApi.Application.Services;

public class RoomTypeService : IRoomTypeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "RoomTypes_All";

    public RoomTypeService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<IEnumerable<RoomTypeDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        if (!_cache.TryGetValue(CacheKey, out IEnumerable<RoomTypeDto>? cachedRoomTypes))
        {
            var roomTypes = await _unitOfWork.RoomTypes.GetAllAsync();
            cachedRoomTypes = _mapper.Map<IEnumerable<RoomTypeDto>>(roomTypes);

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
            };

            _cache.Set(CacheKey, cachedRoomTypes, cacheEntryOptions);
        }

        return cachedRoomTypes!;
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
        
        _cache.Remove(CacheKey);
        
        return roomType.Id;
    }

    public async Task UpdateAsync(Guid id, CreateOrUpdateRoomTypeDto dto, CancellationToken cancellationToken = default)
    {
        var roomType = await _unitOfWork.RoomTypes.GetByIdAsync(id);
        if (roomType == null) throw new NotFoundException("RoomType not found");

        _mapper.Map(dto, roomType);
        _unitOfWork.RoomTypes.Update(roomType);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _cache.Remove(CacheKey);
    }
}
