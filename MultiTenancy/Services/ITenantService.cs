namespace MultiTenancy.Services
{
    public interface ITenantService
    {
        string? GetDbProvider();
        string? GetConnectionString();
        Tenant? GetCurrentTenant();
    }
}
