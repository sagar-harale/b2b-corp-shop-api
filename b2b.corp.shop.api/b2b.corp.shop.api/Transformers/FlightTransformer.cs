using System.Collections.Generic;
using System.Linq;
using System.Xml;
using b2b.corp.shop.api.Models.Amadeus;
using b2b.corp.shop.api.Models.Api;

namespace b2b.corp.shop.api.Transformers
{
    public static class FlightTransformer
    {
        public static FlightOfferSearchRequest ToAmadeusRequest(this FlightListingApiRequest listing)
        {
            var travelers = new List<Traveler>();
            int id = 1;
            for (int i = 0; i < listing.Adults; i++)
                travelers.Add(new Traveler { Id = (id++).ToString(), TravelerType = "ADULT" });
            for (int i = 0; i < listing.Children; i++)
                travelers.Add(new Traveler { Id = (id++).ToString(), TravelerType = "CHILD" });
            for (int i = 0; i < listing.Infants; i++)
                travelers.Add(new Traveler { Id = (id++).ToString(), TravelerType = "HELD_INFANT" });

            var originDestinations = new List<OriginDestination>
            {
                new OriginDestination
                {
                    Id = "1",
                    OriginLocationCode = listing.From,
                    DestinationLocationCode = listing.To,
                    DepartureDateTimeRange = new DepartureDateTimeRange
                    {
                        Date = listing.Date,
                        Time = "00:00:00"
                    }
                }
            };

            if (listing.TripType == "ROUND_TRIP" && !string.IsNullOrEmpty(listing.ReturnDate))
            {
                originDestinations.Add(new OriginDestination
                {
                    Id = "2",
                    OriginLocationCode = listing.To,
                    DestinationLocationCode = listing.From,
                    DepartureDateTimeRange = new DepartureDateTimeRange
                    {
                        Date = listing.ReturnDate,
                        Time = "00:00:00"
                    }
                });
            }

            var cabinRestrictions = originDestinations.Select(od => new CabinRestriction
            {
                Cabin = listing.Class,
                Coverage = "ALL_SEGMENTS",
                OriginDestinationIds = new List<string> { od.Id }
            }).ToList();

            return new FlightOfferSearchRequest
            {
                CurrencyCode = "USD",
                OriginDestinations = originDestinations,
                Travelers = travelers,
                Sources = new List<string> { "GDS" },
                SearchCriteria = new SearchCriteria
                {
                    MaxFlightOffers = 10,
                    FlightFilters = new FlightFilters
                    {
                        CabinRestrictions = cabinRestrictions
                    }
                }
            };
        }


        public static FlightListingApiResponse ToApiResponse(this FlightOfferSearchResponse supplierResponse, string? tripType = null, int adults = 1, int children = 0, int infants = 0)
        {
            var result = new FlightListingApiResponse
            {
                Success = true,
                Message = "Success",
                TripType = tripType ?? "",
                Adults = adults,
                Children = children,
                Infants = infants,
                Flights = supplierResponse.Data?.Select((offer, idx) =>
                {
                    var itinerary = offer.Itineraries?.FirstOrDefault();
                    if (itinerary == null) return null;

                    var segments = itinerary.Segments?.Select(segment =>
                    {
                        var fareSegment = offer.TravelerPricings?.FirstOrDefault()?.FareDetailsBySegment?.FirstOrDefault(fs => fs.SegmentId == segment.Id);

                        return new FlightSegment
                        {
                            FlightNumber = segment.Number ?? "",
                            Airline = segment.CarrierCode ?? "",
                            OperatingCarrier = segment.Operating?.CarrierCode ?? "",
                            Aircraft = segment.Aircraft?.Code ?? "",
                            From = segment.Departure?.IataCode ?? "",
                            To = segment.Arrival?.IataCode ?? "",
                            FromCity = "", // Optional: map from dictionaries.locations if available
                            ToCity = "",
                            TerminalFrom = segment.Departure?.Terminal ?? "",
                            TerminalTo = segment.Arrival?.Terminal ?? "",
                            DepartureTime = segment.Departure?.At ?? "",
                            ArrivalTime = segment.Arrival?.At ?? "",
                            Duration = segment.Duration ?? "",
                            CabinClass = fareSegment?.Cabin ?? "",
                            BookingClass = fareSegment?.Class ?? "",
                            FareBasis = fareSegment?.FareBasis ?? "",
                            BrandedFare = fareSegment?.BrandedFare ?? "",
                            BrandedFareLabel = "", // Optional: use if available
                            Stops = segment.NumberOfStops
                        };
                    }).ToList() ?? new List<FlightSegment>();

                    var firstSegment = segments.FirstOrDefault();
                    var lastSegment = segments.LastOrDefault();
                    var totalDuration = itinerary.Duration ?? "";

                    var tax = offer.Price?.Fees?.Sum(f => decimal.TryParse(f.Amount, out var amt) ? amt : 0) ?? 0;

                    return new FlightOption
                    {
                        ItineraryId = offer.Id ?? "",
                        Segments = segments,
                        Stops = Math.Max(segments.Count - 1, 0),
                        TotalDuration = FormatIsoDuration(totalDuration),
                        LayoverDuration = GetLayoverDuration(itinerary.Segments),
                        CabinClass = firstSegment?.CabinClass ?? offer.PricingOptions?.FareType?.FirstOrDefault() ?? "",
                        BrandedFare = firstSegment?.BrandedFare ?? "",
                        FareBasis = firstSegment?.FareBasis ?? "",
                        Price = offer.Price?.Base ?? "",
                        Tax = tax.ToString("F2"),
                        TotalPrice = offer.Price?.GrandTotal ?? "",
                        Currency = offer.Price?.Currency ?? "",
                        FareType = offer.PricingOptions?.FareType?.FirstOrDefault() ?? "",
                        LastTicketingDate = offer.LastTicketingDate ?? ""
                    };

                }).Where(x => x != null).ToList() ?? new List<FlightOption>()
            };

            return result;
        }

        private static string? GetLayoverDuration(List<Segment>? segments)
        {
            if (segments == null || segments.Count < 2) return null;

            var totalLayover = TimeSpan.Zero;

            for (int i = 0; i < segments.Count - 1; i++)
            {
                var arrivalTime = segments[i].Arrival?.At;
                var nextDepartureTime = segments[i + 1].Departure?.At;

                if (DateTime.TryParse(arrivalTime, out var arrival) && DateTime.TryParse(nextDepartureTime, out var nextDeparture))
                {
                    totalLayover += nextDeparture - arrival;
                }
            }

            return FormatIsoDuration(XmlConvert.ToString(totalLayover));
        }

        private static string FormatIsoDuration(string? isoDuration)
        {
            if (string.IsNullOrWhiteSpace(isoDuration)) return "";

            try
            {
                var ts = XmlConvert.ToTimeSpan(isoDuration);
                return $"{(int)ts.TotalHours}h {ts.Minutes}m";
            }
            catch
            {
                return isoDuration;
            }
        }
    }
}

