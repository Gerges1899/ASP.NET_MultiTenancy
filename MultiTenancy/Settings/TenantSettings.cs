namespace MultiTenancy.Settings
{
    public class TenantSettings
    {
        public Confugration Defaults { get; set; } = default!;
        public List<Tenant> Tenants { get; set; }=new();
    }
}
