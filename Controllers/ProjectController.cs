using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgendaxApi.Data;
using AgendaxApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgendaxApi.Controllers
{
    public class ProjectController : BaseApiController
    {
        private readonly DataContext _context;
        
        public ProjectController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Project>> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return project;
        }


        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject(Project project)
        {
            // Check if the associated user with UserId exists
            var user = await _context.Users.FindAsync(project.UserId);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            project.User = user;

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }


        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetProjectsByUserId(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var projects = await _context.Projects
                .Where(p => p.UserId == userId)
                .Select(p => new { Id=p.Id ,Name = p.Name, Color = p.color })
                .ToListAsync();

            return projects;
        }
    }
}

