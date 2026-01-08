using Application.DTOs.Projects;
using FluentValidation;

namespace Application.Validators.Projects;

public class ProjectValidator : AbstractValidator<ProjectRequestDTO>
{
    public ProjectValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required.")
            .MinimumLength(2).WithMessage("Project name must be at least 2 characters long.");

        RuleFor(x => x.Description)
            .MaximumLength(300).WithMessage("Description cannot exceed 300 characters.");
    }
}


public class UpdateProjectValidator : AbstractValidator<ProjectRequestUpdateDTO>
{
    public UpdateProjectValidator()
    {
        RuleFor(x => x.Id).NotNull();

        RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

        RuleFor(x => x.Description).
            MaximumLength(300);
    }
}