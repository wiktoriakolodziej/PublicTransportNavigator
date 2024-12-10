namespace PublicTransportNavigator.DTOs
{
    public class ReservedSeatDTO
    {
        public long Id { get; set; }
        public long? BusSeatId { get; set; }

        public string? TimeIn { get; set; }

        public string? TimeOff { get; set; }

        public long? UserTravelId { get; set; }

        public string? Date { get; set; }
    }
}
