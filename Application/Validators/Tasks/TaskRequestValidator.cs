using Application.DTOs.Task;
using FluentValidation;

namespace Application.Validators.Tasks;

public sealed class TaskRequestValidator : AbstractValidator<TaskRequestDTO>
{
    public TaskRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Task title is required.")
            .MaximumLength(150).WithMessage("Title cannot exceed 150 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("Project ID is required.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid task status.");
    }
}