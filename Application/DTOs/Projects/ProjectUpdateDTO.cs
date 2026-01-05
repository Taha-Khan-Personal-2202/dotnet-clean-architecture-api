namespace Application.DTOs.Projects;
public class ProjectUpdateDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsArchived { get; set; }
}