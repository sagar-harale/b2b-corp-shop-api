namespace b2b.corp.shop.api.Models.Api
{
    public class FlightListingApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string TripType { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public int Infants { get; set; }
        public List<FlightOption> Flights { get; set; } = new();
    }

}
