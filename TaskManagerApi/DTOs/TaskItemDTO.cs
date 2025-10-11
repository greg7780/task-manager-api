namespace TaskManagerApi.DTOs
{
    public class TaskItemDTO
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Priority { get; set; }
        public bool IsCompleted { get; set; } = false;
        public string CreatedBy { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; } = "";
        public DateTime? ModifiedAt { get; set; }
    }
}
