using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs.Tasks;

public class TaskRequestUpdateDTO : BaseEntity
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; } = string.Empty;
    public Status Status { get; set; } = Status.Pending;
    public DateTime? DueDate { get; set; }
    public Guid? AssignedUserId { get; set; }
}

