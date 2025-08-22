using b2b.corp.shop.api.Models.Lead;
using b2b.corp.shop.dbmodel.Models;

namespace b2b.corp.shop.api.Transformers
{
    public static class LeadTransformer
    {
        public static Lead ToEntity(CreateLeadRequest request)
        {
            return new Lead
            {
                CompanyId = request.CompanyId,
                TravellerId = request.TravellerId,
                AssignedManagerId = request.AssignedManagerId,
                Notes = request.Notes,
                Status = "OPEN",
                CreatedAt = DateTime.UtcNow
            };
        }

        public static LeadResponse ToResponse(Lead entity)
        {
            return new LeadResponse
            {
                LeadId = entity.LeadId,
                CompanyId = entity.CompanyId,
                TravellerId = entity.TravellerId,
                AssignedManagerId = entity.AssignedManagerId,
                Status = entity.Status,
                Notes = entity.Notes,
                CreatedAt = entity.CreatedAt,
                CompanyName = entity.Company?.CompanyName, // optional flatten
                TravellerName = entity.Traveller?.FullName
            };
        }
    }
}
