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
            _context.Tasks.Add(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProjectTask project)
        {
            _context.Tasks.Remove(project);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProjectTask>> GetAllAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<ProjectTask?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(f => f.Id == id);
        }

        public Task<bool> UpdateAsync(ProjectTask project)
        {
            throw new NotImplementedException();
        }
    }
}
