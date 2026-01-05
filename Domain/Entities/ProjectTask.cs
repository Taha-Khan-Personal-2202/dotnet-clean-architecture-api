
using Domain.Entities;
using Domain.Enums;

public class ProjectTask : BaseEntity
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public Status Status { get; set; }
    public DateTime? DueDate { get; set; }

    public Guid ProjectId { get; set; }
    public Guid? AssignedUserId { get; set; }

    private ProjectTask() { }

    public ProjectTask(string title, string? description, Guid projectId, DateTime? dueDate)
    {
        if (string.IsNullOrEmpty(title))
            throw new ArgumentException("Task title is required");

        Title = title;
        Description = description;
        ProjectId = projectId;
        DueDate = dueDate;
        Status = Status.Pending;
    }

    public void AssignTo(Guid userId)
    {
        AssignedUserId = userId;
    }

    public void ChangeStatus(Status newStatus)
    {
        if (Status == Status.Completed)
            throw new InvalidOperationException("Completed task cannot change status");

        if (newStatus < Status)
            throw new InvalidOperationException("Task status cannot move backward");

        Status = newStatus;
    }

    public void SetDueDate(DateTime dueDate)
    {
        DueDate = dueDate;
    }
}
