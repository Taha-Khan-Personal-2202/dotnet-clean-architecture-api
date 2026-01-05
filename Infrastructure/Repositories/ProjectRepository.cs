using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Project project)
    {
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        return await _context.Projects
            .AnyAsync(p => p.Name.Trim().ToLower() == name.Trim().ToLower());
    }

    public async Task<List<Project>> GetAllAsync()
    {
        return await _context.Projects
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(Guid id)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task UpdateAsync(Project project)
    {
        var existing = await _context.Projects.FindAsync(project.Id);
        if (existing is null)
            throw new KeyNotFoundException($"Project with ID {project.Id} not found.");

        _context.Entry(existing).CurrentValues.SetValues(project);
        await _context.SaveChangesAsync();
    }
}