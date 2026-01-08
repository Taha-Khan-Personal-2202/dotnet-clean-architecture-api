using Application.DTOs.Projects;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IProjectService
{
    Task<bool> ExistsByNameAsync(string name);
    Task<ProjectResponseDTO> AddAsync(ProjectRequestDTO project);
    Task UpdateAsync(ProjectRequestUpdateDTO request);
    Task<List<ProjectResponseDTO>> GetAllAsync();
    Task<ProjectResponseDTO?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
}
