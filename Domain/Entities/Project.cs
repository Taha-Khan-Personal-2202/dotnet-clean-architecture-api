namespace Domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsArchived { get; set; } = false;
    
    private Project() { }

    public Project(string name, string? description)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Project name is required");

        Name = name;
        Description = description;
        IsArchived = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void Archive()
    {
        IsArchived = true;
    }
}
