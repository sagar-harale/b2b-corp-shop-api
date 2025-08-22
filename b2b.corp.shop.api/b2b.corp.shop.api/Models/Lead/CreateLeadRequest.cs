namespace b2b.corp.shop.api.Models.Lead
{
    public class CreateLeadRequest
    {
        public int CompanyId { get; set; }
        public int TravellerId { get; set; }
        public int? AssignedManagerId { get; set; }
        public string? Notes { get; set; }
    }
    public class LeadResponse
    {
        public int LeadId { get; set; }
        public int CompanyId { get; set; }
        public int TravellerId { get; set; }
        public int? AssignedManagerId { get; set; }
        public string Status { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        // Optional: flatten navigation info if required
        public string? CompanyName { get; set; }
        public string? TravellerName { get; set; }
    }
}
