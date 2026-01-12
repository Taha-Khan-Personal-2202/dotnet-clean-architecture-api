using Application.DTOs.Project;
using Application.Interfaces;
using Application.Validators.Projects;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;

namespace Application.UseCases.Projects;

public class ProjectUseCase(IProjectRepository repository,
        ProjectValidator createValidations,
        UpdateProjectValidator updataValidations,
        ITaskService taskService) : IProjectService
{
    private readonly IProjectRepository _repository = repository;
    private readonly ITaskService _taskService = taskService;
    private readonly ProjectValidator _createValidations = createValidations;
    private readonly UpdateProjectValidator _updataValidations = updataValidations;

    public async Task<ProjectResponseDTO> AddAsync(ProjectRequestDTO request)
    {
        await _createValidations.ValidateAndThrowAsync(request);

        if (await _repository.ExistsByNameAsync(request.Name.Trim()))
            throw new InvalidOperationException($"Project with name '{request.Name}' already exists.");

        var project = new Project(
            request.Name.Trim(),
            request.Description?.Trim());

        await _repository.AddAsync(project);

        return MapEntityToDTO(project);
    }

    public async Task DeleteAsync(Guid id)
    {
        var project = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Project with ID {id} not found.");

        if (await taskService.FindInProgressTasksAsync())
            throw new InvalidOperationException("The project cannot be archived because it has tasks in progress.");

        await _repository.DeleteAsync(project);
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _repository.ExistsByNameAsync(name);
    }

    public async Task<List<ProjectResponseDTO>> GetAllAsync()
    {
        var projects = await _repository.GetAllAsync();

        List<ProjectResponseDTO> responseDTOs = new List<ProjectResponseDTO>();
        projects.ForEach(f => { responseDTOs.Add(MapEntityToDTO(f)); });
        return responseDTOs;
    }

    public async Task<ProjectResponseDTO?> GetByIdAsync(Guid id)
    {
        var project = await _repository.GetByIdAsync(id);
        if (project is null) return null;

        return MapEntityToDTO(project);
    }

    public async Task<ProjectResponseDTO> UpdateAsync(ProjectRequestUpdateDTO request)
    {
        var project = await _repository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Project with ID {request.Id} not found.");

        if (!string.Equals(project.Name, request.Name, StringComparison.OrdinalIgnoreCase))
        {
            if (await _repository.ExistsByNameAsync(request.Name))
            {
                throw new InvalidOperationException($"Project name '{request.Name}' is already taken.");
            }
        }

        if (request.IsArchived && await taskService.FindInProgressTasksAsync())
            throw new InvalidOperationException("The project cannot be archived because it has tasks in progress.");

        project.Name = request.Name.Trim();
        project.Description = request.Description?.Trim();
        project.IsArchived = request.IsArchived;
        project.UpdatedAt = DateTime.UtcNow;
        project.IsDeleted = request.IsDeleted;
        project.IsActive = request.IsActive;
        await _repository.UpdateAsync(project);

        return MapEntityToDTO(project);
    }


    ProjectResponseDTO MapEntityToDTO(Project project)
    {
        return new ProjectResponseDTO
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            IsArchived = project.IsArchived,
            CreatedAt = project.CreatedAt,
            IsActive = project.IsActive,
            IsDeleted = project.IsDeleted,
            UpdatedAt = project.UpdatedAt,
        };
    }



}