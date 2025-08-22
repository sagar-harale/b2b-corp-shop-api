using b2b.corp.shop.api.Models.Common;
using b2b.corp.shop.api.Repository;
using b2b.corp.shop.dbmodel.Models;

namespace b2b.corp.shop.api.Service
{
    public interface ILeadService
    {
        Task<ApiBaseResponse<Lead?>> GetLeadAsync(Guid id);
        Task<ApiBaseResponse<IEnumerable<Lead>>> GetAllLeadsAsync();
        Task<ApiBaseResponse<Lead>> CreateLeadAsync(Lead lead);
    }
    public class LeadService : ILeadService
    {
        private readonly ILeadRepository _leadRepository;

        public LeadService(ILeadRepository leadRepository)
        {
            _leadRepository = leadRepository;
        }

        public async Task<ApiBaseResponse<Lead?>> GetLeadAsync(Guid id)
        {
            var transactionId = Guid.NewGuid().ToString();
            try
            {
                var lead = await _leadRepository.GetByIdAsync(id);
                if (lead == null)
                    return ApiBaseResponse<Lead?>.CreateError(transactionId, 404, "Lead not found");

                return ApiBaseResponse<Lead?>.CreateSuccess(lead, transactionId);
            }
            catch (Exception ex)
            {
                return ApiBaseResponse<Lead?>.CreateError(transactionId, 500, ex.Message);
            }
        }

        public async Task<ApiBaseResponse<IEnumerable<Lead>>> GetAllLeadsAsync()
        {
            var transactionId = Guid.NewGuid().ToString();
            try
            {
                var leads = await _leadRepository.GetAllAsync();
                return ApiBaseResponse<IEnumerable<Lead>>.CreateSuccess(leads, transactionId);
            }
            catch (Exception ex)
            {
                return ApiBaseResponse<IEnumerable<Lead>>.CreateError(transactionId, 500, ex.Message);
            }
        }

        public async Task<ApiBaseResponse<Lead>> CreateLeadAsync(Lead lead)
        {
            var transactionId = Guid.NewGuid().ToString();
            try
            {
                var created = await _leadRepository.CreateAsync(lead);
                return ApiBaseResponse<Lead>.CreateSuccess(created, transactionId, "Lead created successfully");
            }
            catch (Exception ex)
            {
                return ApiBaseResponse<Lead>.CreateError(transactionId, 500, ex.Message);
            }
        }
    }
}
