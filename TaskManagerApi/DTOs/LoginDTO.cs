namespace TaskManagerApi.DTOs
{
    public class LoginRequestDTO
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class LoginResponseDTO
    {
        public string? Username { get; set; } = "";
        public string? AccessToken { get; set; } = "";
        public string? RefreshToken { get; set; } = "";
        public int ExpiresIn { get; set; }
        public string Message { get; set; } = "";
    }
}
