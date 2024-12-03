namespace PublicTransportNavigator.DTOs
{
    public class BusOnBusStopDTO
    {
        public long Id { get; set; }
        public string? Number { get; set; }
        public List<string>? Time { get; set; }
    }
}
