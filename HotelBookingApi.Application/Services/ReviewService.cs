using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotelBookingApi.Domain.Entities;
using HotelBookingApi.Application.DTOs;
using HotelBookingApi.Application.Interfaces.Repositories;
using HotelBookingApi.Application.Interfaces.Services;

namespace HotelBookingApi.Application.Services;

public class ReviewService : IReviewService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ReviewDto?> GetByBookingIdAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        var review = await _unitOfWork.Reviews.GetByBookingIdAsync(bookingId);
        return review == null ? null : _mapper.Map<ReviewDto>(review);
    }

    public async Task<Guid> CreateAsync(CreateOrUpdateReviewDto dto, CancellationToken cancellationToken = default)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(dto.BookingId);
        if (booking == null) throw new Exception("Booking not found");

        var reviewExists = await _unitOfWork.Reviews.ExistsForBookingAsync(dto.BookingId);
        if (reviewExists) throw new Exception("Review already exists for this booking");

        var review = _mapper.Map<Review>(dto);
        await _unitOfWork.Reviews.AddAsync(review);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return review.Id;
    }
}
