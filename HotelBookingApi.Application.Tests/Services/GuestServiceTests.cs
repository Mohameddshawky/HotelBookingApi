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

public class GuestServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GuestService _sut;

    public GuestServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _sut = new GuestService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnGuest_WhenExists()
    {
        var guestId = Guid.NewGuid();
        var guest = new Guest { Id = guestId, FullName = "John Doe" };
        var dto = new GuestDto { Id = guestId, FullName = "John Doe" };

        _mockUnitOfWork.Setup(u => u.Guests.GetByIdAsync(guestId)).ReturnsAsync(guest);
        _mockMapper.Setup(m => m.Map<GuestDto>(guest)).Returns(dto);

        var result = await _sut.GetByIdAsync(guestId, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(guestId);
    }
}
