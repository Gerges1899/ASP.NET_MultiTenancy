using Microsoft.EntityFrameworkCore;
using MultiTenancy.Data;

namespace MultiTenancy
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddTenancy(this IServiceCollection services,ConfigurationManager Configuration)
        {
            services.AddScoped<ITenantService, TenantService>();
            services.Configure<TenantSettings>(Configuration.GetSection(nameof(TenantSettings)));
            TenantSettings options = new();
            Configuration.GetSection(nameof(TenantSettings)).Bind(options);

            var defaultDbProvider = options.Defaults.DBProvider;

            if (defaultDbProvider.ToLower() == "mssql")
            {
                services.AddDbContext<ApplicationDbContext>(m => m.UseSqlServer());
            }
            foreach (var tenant in options.Tenants)
            {
                var connectionString = tenant.ConnectionString ?? options.Defaults.ConnectionString;
                using var scope = services.BuildServiceProvider().CreateScope();
                var DbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
                DbContext.Database.SetConnectionString(connectionString);

                if (DbContext.Database.GetPendingMigrations().Any())
                {
                    DbContext.Database.Migrate();
                }
                
            }
            return services;
        }
    }
}
