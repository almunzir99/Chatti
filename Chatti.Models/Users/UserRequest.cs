namespace Chatti.Models.Users
{
    public class UserRequest
    {
        public required string Username { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ClientId { get; set; }
    }
}
