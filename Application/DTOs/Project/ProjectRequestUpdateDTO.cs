namespace Application.DTOs.Project;

public sealed record ProjectRequestUpdateDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsArchived { get; init; }
    public bool IsActive { get; init; } = true;
    public bool IsDeleted { get; init; }
}