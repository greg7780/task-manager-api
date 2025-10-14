namespace TaskManagerApi.DTOs
{
    public class RegisterRequestDTO
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
    }

    public class RegisterResponseDTO
    {
        public string? Username { get; set; } = "";
        public string Message { get; set; } = "";
    }
}
