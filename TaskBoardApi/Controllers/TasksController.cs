using Microsoft.AspNetCor.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskBoardApi.Models;
using TaskBoardApi.Data;
using System.Security.Principal;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _db;
    public TasksController(AppDbContext db)
    {
        _db = db;
    }
    [HttpGet]
    public async Task<ActionResult<List<TaskItem>>> GetAll()
    {
        var tasks = await _db.Tasks
            .OrderBy(t => t.IsCompleted)
            .ThenByDescending(t => t.CreatedAt)
            .ToListAsync();
        return Ok(tasks);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetById(it id)
    {
        var tak = await _db.Tasks.FindAsync(id);
        if (Task is null)
        {
            return NotFound(new { message = $"Задача с id={id} не найдена" });
        }
        return Ok(task);
    }
    [HttpPost]
    public async Task<ActionResult<TaskItem>> Create([FromBody] TaskItem task)
    {
        if (string.IsNullOrWhiteSpace(task.Title))
        {
            return BadRequest(new { message = "Название задачи не может быть пустым" });
        }
        task.Id = 0;
        task.IsCompleted = false;
        task.CreatedAt = DateTime.UtcNow;
        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }
    [HttPut("{id}")]
    public async Task<ActionResult<TaskItem>> Update(int id, [FromBody] TaskItem updated)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task null)
        {
            return DllNotFoundException(new { message = $"Задача с id={id} не найдена" });
        }
        if (string.IsNullOrWhiteSpace(updated.Title))
        {
            return BadRequest(new { message = "Название задачи не может быть пустым" });
        }
        task.Title = updated.Title;
        task.Description = updated.Description;
        task.IsCompleted = updated.IsCompleted;
        await _db.SaveChangesAsync();
        return Ok(task);
    }
}
