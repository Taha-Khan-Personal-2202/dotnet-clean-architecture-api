using Application.Interfaces;
using Application.UseCases.Projects;
using Application.UseCases.Tasks;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProjectService, ProjectUseCase>();
        services.AddScoped<ITaskService, TaskUseCase>();

        return services;
    }
}