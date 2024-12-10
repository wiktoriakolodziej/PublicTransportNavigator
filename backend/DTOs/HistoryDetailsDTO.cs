namespace PublicTransportNavigator.DTOs
{
    public class HistoryDetailsDTO
    {
        public string Id { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan DestinationTime { get; set; }
        public string DepartureStopName { get; set; }
        public string DestinationStopName { get; set; }
        public double TravelTime { get; set; }
        public List<Coordinate> Coordinates { get; set; } = [];
    }
}
