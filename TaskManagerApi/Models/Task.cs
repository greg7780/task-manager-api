namespace TaskManagerApi.Models
{
    public class Task
    {
        public long Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public int Priority { get; set; }
        public bool IsCompleted { get; set; }
    }
}
