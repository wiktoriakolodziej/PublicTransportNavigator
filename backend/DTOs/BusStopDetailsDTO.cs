namespace PublicTransportNavigator.DTOs
{
    public class BusStopDetailsDTO
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public bool? OnRequest { get; set; }
        public List<BusOnBusStopDTO>? BusList { get; set; }
    }
}
