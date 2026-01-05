namespace Application.DTOs.Projects;

public class ProjectRequestDTO
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsArchived { get; set; }
}

