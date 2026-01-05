using Domain.Enums;

namespace Application.DTOs.Tasks;

public class TaskRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; } = string.Empty;
    public Status Status { get; set; } = Status.Pending;
}

