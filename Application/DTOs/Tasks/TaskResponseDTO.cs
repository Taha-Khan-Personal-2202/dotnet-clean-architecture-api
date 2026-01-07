using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs.Tasks;

public class TaskResponseDTO : BaseEntity
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; } = string.Empty;
    public Status Status { get; set; } = Status.Pending;
    public Guid ProjectId { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? AssignedUserId { get; set; }
}
