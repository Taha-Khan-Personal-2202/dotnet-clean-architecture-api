using Application.DTOs.Project;
using Domain.Entities;

namespace Application.Interfaces;

public interface IProjectService
{
    Task<bool> ExistsByNameAsync(string name);
    Task<ProjectResponseDTO> AddAsync(ProjectRequestDTO project);
    Task<ProjectResponseDTO> UpdateAsync(ProjectRequestUpdateDTO request);
    Task<List<ProjectResponseDTO>> GetAllAsync();
    Task<ProjectResponseDTO?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
}
