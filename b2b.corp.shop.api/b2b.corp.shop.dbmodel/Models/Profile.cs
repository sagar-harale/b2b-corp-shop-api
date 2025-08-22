using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace b2b.corp.shop.dbmodel.Models
{
    [Table("profiles", Schema = "travel_quotations")]
    public class Profile
    {
        [Key]
        [Column("profile_id")]
        public int ProfileId { get; set; }

        [Required, Column("username")]
        public string Username { get; set; }

        [Required, Column("email")]
        public string Email { get; set; }

        [Required, Column("password_hash")]
        public string PasswordHash { get; set; }

        [Column("full_name")]
        public string? FullName { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Company>? CompaniesCreated { get; set; }
        public ICollection<Company>? CompaniesUpdated { get; set; }
    }

    [Table("company_travellers", Schema = "travel_quotations")]
    public class CompanyTraveller
    {
        [Key]
        [Column("traveller_id")]
        public int TravellerId { get; set; }

        [Required, Column("company_id")]
        public int CompanyId { get; set; }

        [Required, Column("full_name")]
        public string FullName { get; set; }

        [Required, Column("email")]
        public string Email { get; set; }

        [Column("phone_number")]
        public string? PhoneNumber { get; set; }

        [Column("designation")]
        public string? Designation { get; set; }

        [Column("assigned_manager_id")]
        public int? AssignedManagerId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Company Company { get; set; }
        public ICollection<Lead> Leads { get; set; } = new List<Lead>();
    }

    [Table("leads", Schema = "travel_quotations")]
    public class Lead
    {
        [Key]
        [Column("lead_id")]
        public int LeadId { get; set; }

        [Required, Column("company_id")]
        public int CompanyId { get; set; }

        [Required, Column("traveller_id")]
        public int TravellerId { get; set; }

        [Column("assigned_manager_id")]
        public int? AssignedManagerId { get; set; }

        [Column("status")]
        public string Status { get; set; } = "OPEN";

        [Column("notes")]
        public string? Notes { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Company Company { get; set; }
        public CompanyTraveller Traveller { get; set; }
        public ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();
    }

    [Table("quotations", Schema = "travel_quotations")]
    public class Quotation
    {
        [Key]
        [Column("quotation_id")]
        public int QuotationId { get; set; }

        [Required, Column("lead_id")]
        public int LeadId { get; set; }

        [Column("version_no")]
        public int VersionNo { get; set; } = 1;

        [Column("is_latest")]
        public bool IsLatest { get; set; } = true;

        [Column("total_price")]
        public decimal TotalPrice { get; set; }

        [Column("currency")]
        public string Currency { get; set; } = "USD";

        [Column("status")]
        public string Status { get; set; } = "DRAFT";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Lead Lead { get; set; }
        public ICollection<QuotationHotel> Hotels { get; set; } = new List<QuotationHotel>();
        public ICollection<QuotationFlight> Flights { get; set; } = new List<QuotationFlight>();
        public ICollection<QuotationTransfer> Transfers { get; set; } = new List<QuotationTransfer>();
    }

    [Table("quotation_hotels", Schema = "travel_quotations")]
    public class QuotationHotel
    {
        [Key]
        [Column("hotel_id")]
        public int HotelId { get; set; }

        [Required, Column("quotation_id")]
        public int QuotationId { get; set; }

        [Column("hotel_name")]
        public string? HotelName { get; set; }

        [Column("check_in")]
        public DateTime CheckIn { get; set; }

        [Column("check_out")]
        public DateTime CheckOut { get; set; }

        [Column("room_type")]
        public string? RoomType { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("is_included")]
        public bool IsIncluded { get; set; } = true;

        // Navigation
        public Quotation Quotation { get; set; }
    }

    [Table("quotation_flights", Schema = "travel_quotations")]
    public class QuotationFlight
    {
        [Key]
        [Column("flight_id")]
        public int FlightId { get; set; }

        [Required, Column("quotation_id")]
        public int QuotationId { get; set; }

        [Column("airline")]
        public string? Airline { get; set; }

        [Column("flight_number")]
        public string? FlightNumber { get; set; }

        [Column("departure_airport")]
        public string? DepartureAirport { get; set; }

        [Column("arrival_airport")]
        public string? ArrivalAirport { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("is_included")]
        public bool IsIncluded { get; set; } = true;

        // Navigation
        public Quotation Quotation { get; set; }
    }

    [Table("quotation_transfers", Schema = "travel_quotations")]
    public class QuotationTransfer
    {
        [Key]
        [Column("transfer_id")]
        public int TransferId { get; set; }

        [Required, Column("quotation_id")]
        public int QuotationId { get; set; }

        [Column("transfer_type")]
        public string? TransferType { get; set; }

        [Column("vehicle_type")]
        public string? VehicleType { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("is_included")]
        public bool IsIncluded { get; set; } = true;

        // Navigation
        public Quotation Quotation { get; set; }
    }
}
