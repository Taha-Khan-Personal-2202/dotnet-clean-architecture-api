using Application.DTOs.Projects;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.Projects;

public class ProjectUseCase : IProjectService
{
    private readonly IProjectRepository _repository;

    public ProjectUseCase(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProjectResponseDTO> AddAsync(ProjectRequestDTO request)
    {
        if (await _repository.ExistsByNameAsync(request.Name))
        {
            throw new InvalidOperationException($"Project with name '{request.Name}' already exists.");
        }

        var project = new Project(
            request.Name.Trim(),
            request.Description?.Trim());

        await _repository.AddAsync(project);

        return new ProjectResponseDTO
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            IsArchived = project.IsArchived,
            CreatedAt = project.CreatedAt
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        var project = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Project with ID {id} not found.");

        await _repository.DeleteAsync(project);
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _repository.ExistsByNameAsync(name);
    }

    public async Task<List<ProjectResponseDTO>> GetAllAsync()
    {
        var projects = await _repository.GetAllAsync();

        return projects.Select(p => new ProjectResponseDTO
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            IsArchived = p.IsArchived,
            CreatedAt = p.CreatedAt
        }).ToList();
    }

    public async Task<ProjectResponseDTO?> GetByIdAsync(Guid id)
    {
        var project = await _repository.GetByIdAsync(id);

        if (project is null)
            return null;

        return new ProjectResponseDTO
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            IsArchived = project.IsArchived,
            CreatedAt = project.CreatedAt
        };
    }

    public async Task UpdateAsync(ProjectUpdateDTO request)
    {
        var project = await _repository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Project with ID {request.Id} not found.");

        // Check name uniqueness only when name is actually changing
        if (!string.Equals(project.Name, request.Name, StringComparison.OrdinalIgnoreCase))
        {
            if (await _repository.ExistsByNameAsync(request.Name))
            {
                throw new InvalidOperationException($"Project name '{request.Name}' is already taken.");
            }
        }

        project.Name = request.Name.Trim();
        project.Description = request.Description?.Trim();
        project.IsArchived = request.IsArchived;

        await _repository.UpdateAsync(project);
    }
}