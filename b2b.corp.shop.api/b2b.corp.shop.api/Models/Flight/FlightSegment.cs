namespace b2b.corp.shop.api.Models.Api
{
    public class FlightSegment
    {
        public string FlightNumber { get; set; }
        public string Airline { get; set; }
        public string Aircraft { get; set; }
        public string OperatingCarrier { get; set; }
        public string From { get; set; }
        public string FromCity { get; set; }
        public string To { get; set; }
        public string ToCity { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public string Duration { get; set; }
        public string CabinClass { get; set; }
        public string BookingClass { get; set; }
        public string FareBasis { get; set; }
        public string BrandedFare { get; set; }
        public string BrandedFareLabel { get; set; }
        public string TerminalFrom { get; set; }
        public string TerminalTo { get; set; }
        public int Stops { get; set; }
    }

}
