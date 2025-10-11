namespace TaskManagerApi.Models
{
    public class User
    {
        public long Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required bool Deleted { get; set; } = false;
    }
}
