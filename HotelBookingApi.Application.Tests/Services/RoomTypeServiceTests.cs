using System;
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

public class RoomTypeServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly RoomTypeService _sut;

    public RoomTypeServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        var cache = new MemoryCache(new MemoryCacheOptions());
        _sut = new RoomTypeService(_mockUnitOfWork.Object, _mockMapper.Object, cache);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRoomType_WhenExists()
    {
        var roomTypeId = Guid.NewGuid();
        var roomType = new RoomType { Id = roomTypeId, Name = "Deluxe" };
        var dto = new RoomTypeDto { Id = roomTypeId, Name = "Deluxe" };

        _mockUnitOfWork.Setup(u => u.RoomTypes.GetByIdAsync(roomTypeId)).ReturnsAsync(roomType);
        _mockMapper.Setup(m => m.Map<RoomTypeDto>(roomType)).Returns(dto);

        var result = await _sut.GetByIdAsync(roomTypeId, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(roomTypeId);
    }
}
