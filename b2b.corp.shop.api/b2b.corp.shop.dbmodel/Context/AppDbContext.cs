using b2b.corp.shop.dbmodel.Models;
using Microsoft.EntityFrameworkCore;

namespace b2b.corp.shop.dbmodel.Context
{
    public class TravelQuotationDbContext : DbContext
    {
        public TravelQuotationDbContext(DbContextOptions<TravelQuotationDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyTraveller> CompanyTravellers { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<Quotation> Quotations { get; set; }
        public DbSet<QuotationHotel> QuotationHotels { get; set; }
        public DbSet<QuotationFlight> QuotationFlights { get; set; }
        public DbSet<QuotationTransfer> QuotationTransfers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("travel_quotations");

            // Company → Travellers
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Travellers)
                .WithOne(t => t.Company)
                .HasForeignKey(t => t.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Company → Leads
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Leads)
                .WithOne(l => l.Company)
                .HasForeignKey(l => l.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Traveller → Leads
            modelBuilder.Entity<CompanyTraveller>()
                .HasMany(t => t.Leads)
                .WithOne(l => l.Traveller)
                .HasForeignKey(l => l.TravellerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Lead → Quotations
            modelBuilder.Entity<Lead>()
                .HasMany(l => l.Quotations)
                .WithOne(q => q.Lead)
                .HasForeignKey(q => q.LeadId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quotation → Hotels
            modelBuilder.Entity<Quotation>()
                .HasMany(q => q.Hotels)
                .WithOne(h => h.Quotation)
                .HasForeignKey(h => h.QuotationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quotation → Flights
            modelBuilder.Entity<Quotation>()
                .HasMany(q => q.Flights)
                .WithOne(f => f.Quotation)
                .HasForeignKey(f => f.QuotationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quotation → Transfers
            modelBuilder.Entity<Quotation>()
                .HasMany(q => q.Transfers)
                .WithOne(t => t.Quotation)
                .HasForeignKey(t => t.QuotationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Profile → Audit (CreatedBy/UpdatedBy)
            modelBuilder.Entity<Company>()
                .HasOne<Profile>()
                .WithMany()
                .HasForeignKey(c => c.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Company>()
                .HasOne<Profile>()
                .WithMany()
                .HasForeignKey(c => c.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
