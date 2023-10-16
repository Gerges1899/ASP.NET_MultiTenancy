using Microsoft.Extensions.Options;
using MultiTenancy.Settings;

namespace MultiTenancy.Services
{
    public class TenantService : ITenantService
    {
        private readonly TenantSettings _tenantSettings;
        private HttpContext? _context;
        private Tenant? _currentTenant;
        public TenantService(IHttpContextAccessor contextAccessor,IOptions<TenantSettings> tenantSetting)
        {
            _context = contextAccessor.HttpContext;
            _tenantSettings = tenantSetting.Value;
            if (_context is not null)
            {
                if (_context.Request.Headers.TryGetValue("tenant", out var tenantId))
                {
                    SetCurrentTenant(tenantId!);
                }
                else
                {
                    throw new Exception("No Tenant Provided!");
                }
            }
        }

        public string? GetConnectionString()
        {
            return _currentTenant?.ConnectionString;
        }

        public Tenant? GetCurrentTenant()
        {
            return _currentTenant;
        }

        public string? GetDbProvider()
        {
            return _tenantSettings.Defaults.DBProvider;
        }

        private void SetCurrentTenant(string tenantId)
        {
            _currentTenant = _tenantSettings.Tenants.FirstOrDefault(t => t.TId == tenantId);

            if (_currentTenant is null)
            {
                throw new Exception("Inavlid Tenant Id!");
            }
            if (string.IsNullOrEmpty(_currentTenant.ConnectionString))
            {
                _currentTenant.ConnectionString = _tenantSettings.Defaults.ConnectionString;
            }
        }
    }
}
