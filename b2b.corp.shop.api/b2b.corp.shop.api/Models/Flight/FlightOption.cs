namespace b2b.corp.shop.api.Models.Api
{
    public class FlightOption
    {
        public string ItineraryId { get; set; }
        public List<FlightSegment> Segments { get; set; } = new();
        public int Stops { get; set; }
        public string TotalDuration { get; set; }
        public string LayoverDuration { get; set; }
        public string CabinClass { get; set; }
        public string BrandedFare { get; set; }
        public string FareBasis { get; set; }
        public string Price { get; set; }
        public string Tax { get; set; }
        public string TotalPrice { get; set; }
        public string Currency { get; set; }
        public string FareType { get; set; }
        public string ChangeFee { get; set; }
        public string CancelFee { get; set; }
        public string LastTicketingDate { get; set; }
    }


}
