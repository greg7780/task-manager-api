namespace TaskManagerApi.Models
{
    public class RefreshToken
    {
        public long Id { get; set; }
        public required string Token { get; set; } = "";
        public required DateTime Expires { get; set; }

        public required long UserId { get; set; }
        public User? User { get; set; }
    }
}
