using Application.Common.Responses;
using Application.DTOs.Project;

namespace Application.Interfaces;

public interface IProjectService
{
    Task<OperationResult<bool>> ExistsByNameAsync(string name);
    Task<OperationResult<ProjectResponseDTO>> AddAsync(ProjectRequestDTO project);
    Task<OperationResult<ProjectResponseDTO>> UpdateAsync(ProjectRequestUpdateDTO request);
    Task<OperationResult<List<ProjectResponseDTO>>> GetAllAsync();
    Task<OperationResult<ProjectResponseDTO>> GetByIdAsync(Guid id);
    Task<OperationResult<bool>> DeleteAsync(Guid id);
}
