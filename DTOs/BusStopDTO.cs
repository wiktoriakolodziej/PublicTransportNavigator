namespace PublicTransportNavigator.DTOs
{
    public class BusStopDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public float CoordX { get; set; }
        public float CoordY { get; set; }
        public bool OnRequest { get; set; }
    }
}
