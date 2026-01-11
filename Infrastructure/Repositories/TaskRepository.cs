using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TaskRepository(AppDbContext context) : ITaskRepository
    {
        private readonly AppDbContext _context = context;

        public async Task AddAsync(ProjectTask project)
        {
            await _context.Tasks.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProjectTask project)
        {
            _context.Tasks.Remove(project);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> FindCompletedTasksAsync()
        {
            return await _context.Tasks.AnyAsync(a => a.Status == Status.Pending || a.Status == Status.InProgress);
        }

        public async Task<List<ProjectTask>> GetAllAsync()
        {
            return await _context.Tasks
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<ProjectTask?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(ProjectTask project)
        {
            var existing = await _context.Tasks.FindAsync(project.Id);
            if (existing is null)
                throw new KeyNotFoundException($"Project with ID {project.Id} not found.");

            _context.Entry(existing).CurrentValues.SetValues(project);
            await _context.SaveChangesAsync();
        }
    }
}
