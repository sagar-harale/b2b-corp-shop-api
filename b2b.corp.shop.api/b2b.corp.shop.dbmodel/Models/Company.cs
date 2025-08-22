using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace b2b.corp.shop.dbmodel.Models
{
    [Table("companies", Schema = "travel_quotations")]
    public class Company
    {
        [Key]
        [Column("company_id")]
        public int CompanyId { get; set; }

        [Required, Column("company_name")]
        public string CompanyName { get; set; }

        [Required, Column("partner_type")]
        public string PartnerType { get; set; }

        [Column("display_name")]
        public string? DisplayName { get; set; }

        [Column("status")]
        public bool Status { get; set; } = true;

        [Column("currency")]
        public string Currency { get; set; } = "USD";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("created_by")]
        public int? CreatedBy { get; set; }

        [Column("updated_by")]
        public int? UpdatedBy { get; set; }

        // Navigation
        public ICollection<CompanyTraveller> Travellers { get; set; } = new List<CompanyTraveller>();
        public ICollection<Lead> Leads { get; set; } = new List<Lead>();
    }
}
