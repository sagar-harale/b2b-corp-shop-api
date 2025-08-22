﻿using System.Collections.Generic;

namespace b2b.corp.shop.api.Models.Amadeus
{
    /// <summary>
    /// Represents the root response object returned by Amadeus Flight Offer Search API.
    /// </summary>
    public class FlightOfferSearchResponse
    {
        public List<FlightOffer>? Data { get; set; }
        public Meta? Meta { get; set; }
        public Dictionary<string, object>? Dictionaries { get; set; }
    }

    public class FlightOffer
    {

        public string? Type { get; set; }
        public string? Id { get; set; }
        public string? Source { get; set; }
        public bool InstantTicketingRequired { get; set; }
        public bool NonHomogeneous { get; set; }
        public bool OneWay { get; set; }
        public string? LastTicketingDate { get; set; }
        public int NumberOfBookableSeats { get; set; }
        public List<Itinerary>? Itineraries { get; set; }
        public Price? Price { get; set; }
        public PricingOptions? PricingOptions { get; set; }
        public List<string>? ValidatingAirlineCodes { get; set; }
        public List<TravelerPricing>? TravelerPricings { get; set; }
    }

    public class Meta
    {
        public int Count { get; set; }
        public Links? Links { get; set; }
    }

    public class Links
    {
        public string? Self { get; set; }
    }

    public class Price
    {
        public string? Currency { get; set; }
        public string? Total { get; set; }
        public string? Base { get; set; }
        public List<Fee>? Fees { get; set; }
        public string? GrandTotal { get; set; }
    }

    public class Fee
    {
        public string? Amount { get; set; }
        public string? Type { get; set; }
    }

    public class Itinerary
    {
        public string? Duration { get; set; }
        public List<Segment>? Segments { get; set; }
    }

    public class Segment
    {
        public Departure? Departure { get; set; }
        public Arrival? Arrival { get; set; }
        public string? CarrierCode { get; set; }
        public string? Number { get; set; }
        public Aircraft? Aircraft { get; set; }
        public Operating? Operating { get; set; }
        public string? Duration { get; set; }
        public string? Id { get; set; }
        public int NumberOfStops { get; set; }
        public bool BlacklistedInEU { get; set; }
    }

    public class Departure
    {
        public string? IataCode { get; set; }
        public string? Terminal { get; set; }
        public string? At { get; set; }
    }

    public class Arrival
    {
        public string? IataCode { get; set; }
        public string? Terminal { get; set; }
        public string? At { get; set; }
    }

    public class Aircraft
    {
        public string? Code { get; set; }
    }

    public class Operating
    {
        public string? CarrierCode { get; set; }
    }

    public class PricingOptions
    {
        public List<string>? FareType { get; set; }
        public bool IncludedCheckedBagsOnly { get; set; }
    }

    public class TravelerPricing
    {
        public string? TravelerId { get; set; }
        public string? FareOption { get; set; }
        public string? TravelerType { get; set; }
        public Price? Price { get; set; }
        public List<FareDetailsBySegment>? FareDetailsBySegment { get; set; }
    }

    public class FareDetailsBySegment
    {
        public string? SegmentId { get; set; }
        public string? Cabin { get; set; }
        public string? FareBasis { get; set; }
        public string? BrandedFare { get; set; }
        public string? Class { get; set; }
        public IncludedCheckedBags? IncludedCheckedBags { get; set; }
    }

    public class IncludedCheckedBags
    {
        public int Quantity { get; set; }
    }
}
