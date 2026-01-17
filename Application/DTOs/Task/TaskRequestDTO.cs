using Domain.Enums;

namespace Application.DTOs.Task;

public class TaskRequestDTO
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; } = string.Empty;
    public Status Status { get; set; } = Status.Pending;
    public Guid ProjectId { get; set; }
    public DateTime? DueDate { get; set; }
}

