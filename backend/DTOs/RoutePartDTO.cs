namespace PublicTransportNavigator.DTOs
{
    public class RoutePartDTO
    {
        public long BusId { get; set; }
        public string BusName { get; set; }
        public List<string> Details { get; set; } = [];
    }
}
