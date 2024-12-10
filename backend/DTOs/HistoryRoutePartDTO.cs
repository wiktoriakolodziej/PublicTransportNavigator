namespace PublicTransportNavigator.DTOs
{
    public class HistoryRoutePartDTO
    {
        public long BusId { get; set; }
        public int BusName { get; set; }
        public List<string> Details { get; set; } = [];
    }
}
