using Application.Interfaces;
using Application.UseCases.Projects;
using Application.UseCases.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(
    typeof(DependencyInjection).Assembly);

        services.AddScoped<IProjectService, ProjectUseCase>();
        services.AddScoped<ITaskService, TaskUseCase>();

        return services;
    }
}