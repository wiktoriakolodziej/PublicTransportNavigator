using AutoMapper;
using PublicTransportNavigator.Dijkstra;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.DTOs.old;
using PublicTransportNavigator.Models;

namespace PublicTransportNavigator
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Bus, BusDTO>();
            CreateMap<BusDTO, Bus>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<BusCreateDTO, Bus>();
            CreateMap<Bus, BusCreateDTO>();
            CreateMap<BusStop, BusStopDTO>();
            CreateMap<BusStop, BusStopDetailsDTO>();
            CreateMap<BusStopCreateDTO, BusStop>();
            CreateMap<BusStopDTO, BusStop>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UserFavouriteBusStop, UserFavouriteBusStopDTO>();
            CreateMap<UserFavouriteBusStopDTO, UserFavouriteBusStop>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<Timetable, TimetableDTO>();
            CreateMap<TimetableDTO, Timetable>();
            CreateMap<TimetableCreateDTO, Timetable>();
            CreateMap<RouteDetails, RouteDetailsDTO>()
                .ForMember(dest => dest.Parts, opt => opt.MapFrom(src => src.Parts.Values.ToList()));
        }
    }

}
