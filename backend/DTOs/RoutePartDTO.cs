namespace PublicTransportNavigator.DTOs
{
    public class RoutePartDTO
    {
        public long BusId { get; set; }
        public int BusName { get; set; }
        public Dictionary<TimeSpan, string> Details { get; set; } = [];
    }
}
