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
using Moq;
using Xunit;

namespace HotelBookingApi.Application.Tests.Services;

public class AmenityServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly AmenityService _sut;

    public AmenityServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _sut = new AmenityService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAmenities()
    {
        // Arrange
        var amenities = new List<Amenity> { new Amenity { Id = Guid.NewGuid(), Name = "WiFi" } };
        var dtos = new List<AmenityDto> { new AmenityDto { Id = amenities[0].Id, Name = "WiFi" } };

        _mockUnitOfWork.Setup(u => u.Amenities.GetAllAsync()).ReturnsAsync(amenities);
        _mockMapper.Setup(m => m.Map<IEnumerable<AmenityDto>>(amenities)).Returns(dtos);

        // Act
        var result = await _sut.GetAllAsync(CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(dtos);
        _mockUnitOfWork.Verify(u => u.Amenities.GetAllAsync(), Times.Once);
    }
}
