namespace HouseRentingSystem.Web.Infrastructure.Extensions;

using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

public static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Finds all service interfaces and service types and registers them in the dependency injection container
    /// </summary>
    /// <param name="services">Services collection</param>
    /// <param name="serviceType">The type we are looking for</param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void AddServicesReflection(this IServiceCollection services, Type serviceType)
    {
        Assembly? assembly = Assembly.GetAssembly(serviceType) ?? throw new InvalidOperationException();

        Type[] serviceTypes = assembly.GetTypes()
            .Where(t => t.Name.EndsWith("Service") && !t.IsInterface)
            .ToArray();

        foreach (var implementationType in serviceTypes)
        {
            Type? interfaceType = implementationType
                .GetInterface($"I{implementationType.Name}") ?? throw new InvalidOperationException();

            services.AddScoped(interfaceType, implementationType);
        }
    }
}
