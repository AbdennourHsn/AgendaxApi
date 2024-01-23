using System;
using System.Security.Cryptography;
using System.Text;
using AgendaxApi.Data;
using AgendaxApi.DTOs;
using AgendaxApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgendaxApi.Controllers
{
	public class TodosController : BaseApiController
	{
        private readonly DataContext _context;
        public TodosController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetUsersTodosAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }
            var todos = await _context.Todos
                    .Where(t => t.UserId == userId)
                    .Select(t => new TodoDto
                    {
                        Id=t.Id,
                        Title = t.Title,
                        Description = t.Description,
                        Date = DateTime.UtcNow,
                        DeadTime = t.DeadTime,
                        State = (TodoState)t.State,
                        Color = t.Color,
                        ProjectId = t.ProjectId
                    })
                    .ToListAsync();

            return todos;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpDelete("deleteTodo/{todoId}")]
        public async Task<IActionResult> DeleteTodo(int todoId)
        {
            try
            {
                var todo = await _context.Todos.FindAsync(todoId);

                if (todo == null)
                {
                    return NotFound("Todo not found");
                }

                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();

                return Ok("Todo deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("updateTodo/{todoId}")]
        public async Task<IActionResult> UpdateTodo(int todoId, Todo updateTodoDto)
        {
            try
            {
                var todo = await _context.Todos.FindAsync(todoId);

                if (todo == null)
                {
                    return NotFound("Todo not found");
                }

                todo.Title = updateTodoDto.Title;
                todo.Description = updateTodoDto.Description;
                todo.Date = updateTodoDto.Date;
                todo.DeadTime = updateTodoDto.DeadTime;
                todo.State = updateTodoDto.State;
                todo.Color = updateTodoDto.Color;

                await _context.SaveChangesAsync();

                return Ok("Todo updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("addTodo/{userId}")]
        public async Task<IActionResult> AddTodo(int userId, TodoDto createTodoDto)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var newTodo = new Todo
                {
                    Title = createTodoDto.Title,
                    Description = createTodoDto.Description,
                    Date = createTodoDto.Date,
                    DeadTime = createTodoDto.DeadTime.ToUniversalTime(),
                    State = createTodoDto.State,
                    Color = createTodoDto.Color,
                    UserId = userId,
                    ProjectId=createTodoDto.ProjectId
                };

                _context.Todos.Add(newTodo);
                await _context.SaveChangesAsync();

                return Ok("Todo added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}

