using b2b.corp.shop.api.Repository;
using b2b.corp.shop.dbmodel;
using b2b.corp.shop.dbmodel.Context;
using b2b.corp.shop.dbmodel.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace b2b.corp.shop.api.Repository
{
    public interface ILeadRepository
    {
        Task<Lead?> GetByIdAsync(Guid id);
        Task<IEnumerable<Lead>> GetAllAsync();
        Task<Lead> CreateAsync(Lead lead);
    }
    public class LeadRepository : ILeadRepository
    {
        private readonly AppDbContext _context;

        public LeadRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Lead?> GetByIdAsync(Guid id)
        {
            return await _context.Leads.FindAsync(id);
        }

        public async Task<IEnumerable<Lead>> GetAllAsync()
        {
            return await _context.Leads.AsNoTracking().ToListAsync();
        }

        public async Task<Lead> CreateAsync(Lead lead)
        {
            _context.Leads.Add(lead);
            await _context.SaveChangesAsync();
            return lead;
        }
    }
}
