using AutoMapper;
using HotelBookingApi.Domain.Entities;
using HotelBookingApi.Application.DTOs;

namespace HotelBookingApi.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // RoomType mappings
        CreateMap<RoomType, RoomTypeDto>();
        CreateMap<CreateOrUpdateRoomTypeDto, RoomType>();

        // Room mappings
        CreateMap<Room, RoomDto>();
        CreateMap<Room, AvailableRoomDto>()
            .ForMember(dest => dest.PricePerNight, opt => opt.MapFrom(src => src.RoomType != null ? src.RoomType.PricePerNight : 0));
        CreateMap<CreateOrUpdateRoomDto, Room>();

        // Guest mappings
        CreateMap<Guest, GuestDto>();
        CreateMap<CreateOrUpdateGuestDto, Guest>();

        // Booking mappings
        CreateMap<Booking, BookingDetailsDto>();
        CreateMap<CreateOrUpdateBookingDto, Booking>();
        CreateMap<Booking, BookingHistoryDto>();

        // Review mappings
        CreateMap<Review, ReviewDto>();
        CreateMap<CreateOrUpdateReviewDto, Review>();

        // Amenity mappings
        CreateMap<Amenity, AmenityDto>();
        CreateMap<CreateOrUpdateAmenityDto, Amenity>();
    }
}
