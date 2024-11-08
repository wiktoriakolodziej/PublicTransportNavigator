using PublicTransportNavigator.Models;

namespace PublicTransportNavigator.DTOs.Create
{
    public class UserFavouriteBusStopCreateDTO
    {
        public long UserId { get; set; }
        public long BusStopId { get; set; }
       
    }
}
