using Application.DTOs.Project;
using FluentValidation;

namespace Application.Validators.Projects;

public sealed class ProjectRequestValidator : AbstractValidator<ProjectRequestDTO>
{
    public ProjectRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
    }
}

public sealed class ProjectRequestUpdateValidator : AbstractValidator<ProjectRequestUpdateDTO>
{
    public ProjectRequestUpdateValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Project ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
    }
}