using AutoMapper;
using be_movie_booking.Domain.Entities;

namespace be_movie_booking.Domain.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, Movie>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<ShowTime, ShowTime>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Food, Food>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Voucher, Voucher>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Cinema, Cinema>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Room, Room>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Seat, Seat>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
