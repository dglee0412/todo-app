using TodoApi.Models;

namespace TodoApi.Dtos;

//생성 요청 - 클라이언트가 보낼 수 있는 것만
public record CreateTodoDto
(
    string Title,
    string? Description,
    TodoPriority Priority,
    DateTime? DueDate
);

// 수정 요청 - 상태도 바꿀 수 있게
public record UpdateTodoTdo
(
    string Title,
    string? Description,
    TodoStatus Status,
    TodoPriority Priority,
    DateTime? DueDate
);

//응답 - 클라이언트에게 돌려줄 것
public record TodoResponseDto
(
    int Id,
    string Title,
    string? Description,
    TodoStatus Status,
    TodoPriority Priority,
    DateTime? DueDate,
    DateTime CreatedAt,      // 항상 있음 → not-null
    DateTime? UpdatedAt      // 수정 전이면 null → nullable
);
