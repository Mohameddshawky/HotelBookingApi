using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using HotelBookingApi.Application.DTOs;
using HotelBookingApi.Application.Interfaces.Repositories;
using HotelBookingApi.Application.Services;
using HotelBookingApi.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace HotelBookingApi.Application.Tests.Services;

public class ReportServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ReportService _sut;

    public ReportServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        var cache = new MemoryCache(new MemoryCacheOptions());
        _sut = new ReportService(_mockUnitOfWork.Object, _mockMapper.Object, cache);
    }

    [Fact]
    public async Task GetAvailableRoomsAsync_ShouldReturnAvailableRooms()
    {
        var checkIn = DateTime.UtcNow;
        var checkOut = DateTime.UtcNow.AddDays(2);
        var rooms = new List<Room> { new Room { Id = Guid.NewGuid() } };
        var dtos = new List<AvailableRoomDto> { new AvailableRoomDto { Id = rooms[0].Id } };

        _mockUnitOfWork.Setup(u => u.Rooms.GetAvailableRoomsAsync(checkIn, checkOut)).ReturnsAsync(rooms);
        _mockMapper.Setup(m => m.Map<IEnumerable<AvailableRoomDto>>(rooms)).Returns(dtos);

        var result = await _sut.GetAvailableRoomsAsync(checkIn, checkOut, CancellationToken.None);

        result.Should().BeEquivalentTo(dtos);
    }
}
