using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using HotelBookingApi.Application.DTOs;
using HotelBookingApi.Application.Interfaces.Repositories;
using HotelBookingApi.Application.Services;
using HotelBookingApi.Domain.Entities;
using Moq;
using Xunit;

namespace HotelBookingApi.Application.Tests.Services;

public class ReviewServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ReviewService _sut;

    public ReviewServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _sut = new ReviewService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetByBookingIdAsync_ShouldReturnReview_WhenExists()
    {
        var bookingId = Guid.NewGuid();
        var review = new Review { Id = Guid.NewGuid(), BookingId = bookingId, Comment = "Great stay!" };
        var dto = new ReviewDto { Id = review.Id, BookingId = bookingId, Comment = "Great stay!" };

        _mockUnitOfWork.Setup(u => u.Reviews.GetByBookingIdAsync(bookingId)).ReturnsAsync(review);
        _mockMapper.Setup(m => m.Map<ReviewDto>(review)).Returns(dto);

        var result = await _sut.GetByBookingIdAsync(bookingId, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(review.Id);
        result.Comment.Should().Be("Great stay!");
    }
}
