namespace TaskManagerApi.Models
{
    public class TaskItem
    {
        public long Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int Priority { get; set; }
        public required bool IsCompleted { get; set; } = false;
        public required string CreatedBy { get; set; }
        public required DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public required bool Deleted { get; set; } = false;
    }
}
