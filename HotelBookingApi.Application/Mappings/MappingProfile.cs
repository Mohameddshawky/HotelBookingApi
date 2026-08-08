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
        CreateMap<SaveRoomTypeDto, RoomType>();

        // Room mappings
        CreateMap<Room, RoomDto>();
        CreateMap<Room, AvailableRoomDto>()
            .ForMember(dest => dest.PricePerNight, opt => opt.MapFrom(src => src.RoomType != null ? src.RoomType.PricePerNight : 0));
        CreateMap<SaveRoomDto, Room>();

        // Guest mappings
        CreateMap<Guest, GuestDto>();
        CreateMap<SaveGuestDto, Guest>();

        // Booking mappings
        CreateMap<Booking, BookingDetailsDto>();
        CreateMap<SaveBookingDto, Booking>();
        CreateMap<Booking, BookingHistoryDto>();

        // Review mappings
        CreateMap<Review, ReviewDto>();
        CreateMap<SaveReviewDto, Review>();
    }
}
