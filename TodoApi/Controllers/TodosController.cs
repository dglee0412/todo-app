using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class TodosController : ControllerBase
    {
        private readonly AppDbContext _db;
        public TodosController(AppDbContext db) => _db = db;

        // 임시 사용자 ID — Phase 2에서 로그인 토큰 기반으로 교체
        private const int TempUserId = 1;

        // GET /api/todos — 목록
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoResponseDto>>> GetAll()
        {
            var todos = await _db.Todos
                              .Where(t => t.UserId == TempUserId)
                              .OrderByDescending(t => t.CreatedAt)
                              .Select(t => ToDto(t))
                              .ToListAsync();

            return Ok(todos);
        }

        // GET /api/todos/5 — 단건
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoResponseDto>> GetById(int id)
        {
            var todo = await _db.Todos
                            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == TempUserId);

            if (todo is null)
                return NotFound();

            return Ok(ToDto(todo));
        }

        // POST /api/todos — 생성
        [HttpPost]
        public async Task<ActionResult<TodoResponseDto>> Create(CreateTodoDto dto)
        {
            var todo = new Todo
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                DueDate = dto.DueDate,
                Status = TodoStatus.NotStarted,
                UserId = TempUserId,
                CreatedAt = DateTime.UtcNow
            };

            _db.Todos.Add(todo);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = todo.Id }, ToDto(todo));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateTodoTdo dto)
        {
            var todo = await _db.Todos
                            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == TempUserId);

            if(todo is null) return NotFound();

            todo.Title = dto.Title;
            todo.Description = dto.Description;
            todo.Status = dto.Status;
            todo.Priority = dto.Priority;
            todo.DueDate = dto.DueDate;
            todo.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var todo = await _db.Todos
                             .FirstOrDefaultAsync(t => t.Id == id && t.UserId == TempUserId);

            if(todo is null) return NotFound();

            _db.Todos .Remove(todo);
            await _db.SaveChangesAsync();
            
            return NoContent();
        }

        private static TodoResponseDto ToDto(Todo t) => new(
            t.Id, t.Title, t.Description, t.Status, t.Priority, 
            t.DueDate, t.CreatedAt, t.UpdatedAt
        );
    }
}
