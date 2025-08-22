using b2b.corp.shop.dbmodel.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;

namespace b2b.corp.shop.dbmodel.DI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddB2bCorpShopDbModel(
            this IServiceCollection services,
            string connectionString,
            Action<DbContextOptionsBuilder>? extraOptions = null)
        {
            var dsb = new NpgsqlDataSourceBuilder(connectionString);

            // Spatial mapping (PostGIS / NetTopologySuite)
            dsb.UseNetTopologySuite();

            // Allow unmapped types (for JSONB, Geometry, etc.)
            dsb.EnableUnmappedTypes();

            var dataSource = dsb.Build();
            services.AddSingleton(dataSource);

            // --- Register DbContext ---
            services.AddDbContext<AppDbContext>((sp, opt) =>
            {
                var ds = sp.GetRequiredService<NpgsqlDataSource>();
                opt.UseNpgsql(ds, npg => npg.UseNetTopologySuite());

                // Allow caller to extend options (logging, lazy loading, etc.)
                extraOptions?.Invoke(opt);
            });

            return services;
        }
    }
}
