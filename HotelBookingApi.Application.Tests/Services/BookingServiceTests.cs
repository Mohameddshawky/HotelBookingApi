using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using HotelBookingApi.Application.DTOs;
using HotelBookingApi.Application.Interfaces.Notifications;
using HotelBookingApi.Application.Interfaces.Repositories;
using HotelBookingApi.Application.Services;
using HotelBookingApi.Domain.Entities;
using Moq;
using Xunit;

namespace HotelBookingApi.Application.Tests.Services;

public class BookingServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly BookingService _sut;

    public BookingServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        var strategies = new List<INotificationStrategy>();
        _sut = new BookingService(_mockUnitOfWork.Object, _mockMapper.Object, strategies);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnBooking_WhenExists()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var booking = new Booking { Id = bookingId };
        var dto = new BookingDetailsDto { Id = bookingId };

        _mockUnitOfWork.Setup(u => u.Bookings.GetByIdAsync(bookingId)).ReturnsAsync(booking);
        _mockMapper.Setup(m => m.Map<BookingDetailsDto>(booking)).Returns(dto);

        // Act
        var result = await _sut.GetByIdAsync(bookingId, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(bookingId);
    }
}
