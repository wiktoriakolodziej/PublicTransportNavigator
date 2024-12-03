namespace PublicTransportNavigator.DTOs.SyncData
{
    public class BusSyncDTO
    {
        public long RouteId { get; set; }
        public long Id { get; set; }
        public string? Number { get; set; }
        public long? TypeId { get; set; }
        public long? FirstBusStopId { get; set; }
        public long? LastBusStopId { get; set; }
        public bool? WheelchairAccessible { get; set; }
        public DateTime LastModified { get; set; }
        public long CalendarId { get; set; }
    }
}
