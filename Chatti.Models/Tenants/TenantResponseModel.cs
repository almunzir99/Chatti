namespace Chatti.Models.Tenants
{
    public class TenantResponseModel
    {
        public required string Id { get; set; }
        public required string TenantName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
