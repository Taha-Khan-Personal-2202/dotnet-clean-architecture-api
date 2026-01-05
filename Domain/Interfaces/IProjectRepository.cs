using Domain.Entities;

namespace Domain.Interfaces;

public interface IProjectRepository
{
    Task<bool> ExistsByNameAsync(string name);
    Task AddAsync(Project project);
    Task UpdateAsync(Project project);
    Task<List<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(Guid id);
    Task DeleteAsync(Project project);
}