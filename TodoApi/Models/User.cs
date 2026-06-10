namespace TodoApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //한 User가 여러 Todo를 가짐 (1:N 관계)
        public List<Todo> Todos { get; set; } = new();
    }
}
