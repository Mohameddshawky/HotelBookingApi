using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotelBookingApi.Domain.Entities;
using HotelBookingApi.Application.DTOs;
using HotelBookingApi.Application.Interfaces.Repositories;
using HotelBookingApi.Application.Interfaces.Services;
using HotelBookingApi.Domain.Exceptions;

namespace HotelBookingApi.Application.Services;

public class AmenityService : IAmenityService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AmenityService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AmenityDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var amenities = await _unitOfWork.Amenities.GetAllAsync();
        return _mapper.Map<IEnumerable<AmenityDto>>(amenities);
    }

    public async Task<AmenityDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var amenity = await _unitOfWork.Amenities.GetByIdAsync(id);
        return amenity == null ? null : _mapper.Map<AmenityDto>(amenity);
    }

    public async Task<Guid> CreateAsync(CreateOrUpdateAmenityDto dto, CancellationToken cancellationToken = default)
    {
        var amenity = _mapper.Map<Amenity>(dto);
        await _unitOfWork.Amenities.AddAsync(amenity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return amenity.Id;
    }

    public async Task UpdateAsync(Guid id, CreateOrUpdateAmenityDto dto, CancellationToken cancellationToken = default)
    {
        var amenity = await _unitOfWork.Amenities.GetByIdAsync(id);
        if (amenity == null) throw new NotFoundException("Amenity not found");

        _mapper.Map(dto, amenity);
        _unitOfWork.Amenities.Update(amenity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var amenity = await _unitOfWork.Amenities.GetByIdAsync(id);
        if (amenity == null) throw new NotFoundException("Amenity not found");

        _unitOfWork.Amenities.Delete(amenity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
