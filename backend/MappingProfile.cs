using AutoMapper;
using PublicTransportNavigator.Dijkstra;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.DTOs.old;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Services;

namespace PublicTransportNavigator
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Bus, BusDTO>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type != null ? src.Type.Type.ToString() : null));
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

            CreateMap<BusType, BusTypeDTO>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

            CreateMap<RouteDetails, RouteDetailsDTO>()
                .ForMember(dest => dest.Parts, opt => opt.MapFrom(src =>
                    src.Parts.Select(p => new RoutePartDTO
                    {
                        BusId = p.Key, 
                        BusName = p.Value.BusName,
                        Details = p.Value.Details
                    }).ToList()))
                .ForMember(dest => dest.Coordinates, opt => opt.MapFrom(src => src.Coordinates));

            // Mapping for RoutePart to RoutePartDTO (if you need it separately)
            CreateMap<RoutePart, RoutePartDTO>()
                .ForMember(dest => dest.BusId, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.BusName, opt => opt.MapFrom(src => src.BusName))
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details));


            CreateMap<UserFavouriteBusStopCreateDTO, UserFavouriteBusStop>();


        }
    }

}
