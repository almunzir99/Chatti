namespace Chatti.Models
{
    public class ClientResponseModel
    {
        public required string Id { get; set; }
        public required string ClientName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
