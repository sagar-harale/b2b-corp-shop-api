using System.Collections.Generic;

namespace b2b.corp.shop.api.Models.Api
{
    public class FlightListingApiRequest
    {
        public string TripType { get; set; } = "ONE_WAY"; // Default to one-way, can be changed to "ROUND_TRIP"
        public string From { get; set; }
        public string To { get; set; }
        public string Date { get; set; } // ISO format recommended
        public string ReturnDate { get; set; } // Required for round trips
        public string Class { get; set; } // e.g., "ECONOMY", "BUSINESS"
        public int Adults { get; set; } = 1;
        public int Children { get; set; } = 0;
        public int Infants { get; set; } = 0;
    }

}
