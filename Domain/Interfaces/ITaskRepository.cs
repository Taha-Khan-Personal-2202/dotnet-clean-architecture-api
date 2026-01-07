using Domain.Entities;

namespace Domain.Interfaces;

public interface ITaskRepository
{
    Task AddAsync(ProjectTask ProjectTask);
    Task UpdateAsync(ProjectTask ProjectTask);
    Task<List<ProjectTask>> GetAllAsync();
    Task<ProjectTask?> GetByIdAsync(Guid id);
    Task DeleteAsync(ProjectTask ProjectTask);
}
