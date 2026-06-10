namespace TodoApi.Models
{
    public enum TodoStatus
    {
        Pending = 0,      // 진행 전/진행중
        Completed = 1     // 완료
    }
    public enum TodoPriority
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    public class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TodoStatus Status { get; set; } = TodoStatus.NotStarted;
        public TodoPriority Priority { get; set; } = TodoPriority.Medium;
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // FK — 이 Todo의 주인
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
