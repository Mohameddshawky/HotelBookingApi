using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using HotelBookingApi.Application.DTOs;
using HotelBookingApi.Application.Interfaces.Repositories;
using HotelBookingApi.Application.Services;
using HotelBookingApi.Application.Strategies.Sorting;
using System.Collections.Generic;
using HotelBookingApi.Domain.Entities;
using Moq;
using Xunit;

namespace HotelBookingApi.Application.Tests.Services;

public class RoomServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly RoomService _sut;

    public RoomServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        var factory = new RoomSortStrategyFactory(new List<IRoomSortStrategy>());
        _sut = new RoomService(_mockUnitOfWork.Object, _mockMapper.Object, factory);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRoom_WhenExists()
    {
        var roomId = Guid.NewGuid();
        var room = new Room { Id = roomId, Number = "101" };
        var dto = new RoomDto { Id = roomId, Number = "101" };

        _mockUnitOfWork.Setup(u => u.Rooms.GetByIdAsync(roomId)).ReturnsAsync(room);
        _mockMapper.Setup(m => m.Map<RoomDto>(room)).Returns(dto);

        var result = await _sut.GetByIdAsync(roomId, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(roomId);
    }
}
