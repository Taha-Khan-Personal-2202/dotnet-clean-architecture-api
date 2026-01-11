using Domain.Entities;

namespace Application.DTOs.Project;
public class ProjectRequestUpdateDTO : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsArchived { get; set; }
}