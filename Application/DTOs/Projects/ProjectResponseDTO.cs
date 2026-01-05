using Domain.Entities;

namespace Application.DTOs.Projects;

public class ProjectResponseDTO : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsArchived { get; set; }
}

