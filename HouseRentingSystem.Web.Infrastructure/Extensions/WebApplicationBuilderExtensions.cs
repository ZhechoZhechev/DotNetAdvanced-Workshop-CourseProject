namespace HouseRentingSystem.Web.Infrastructure.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

using HouseRentingSystem.Data.Models;
using static HouseRentingSystem.Common.GeneralApplicationConstants;
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

    /// <summary>
    /// creating the admin role and assingning it to a user via his email
    /// </summary>
    /// <param name="app"></param>
    /// <param name="adminEmail"> users email</param>
    /// <returns></returns>
    public static IApplicationBuilder AddUserInAdnimRole(this IApplicationBuilder app, string adminEmail)
    {
        using IServiceScope scopedService = app.ApplicationServices.CreateScope();

        IServiceProvider serviceProvier = scopedService.ServiceProvider;

        UserManager<ApplicationUser> userManager =
            serviceProvier.GetService<UserManager<ApplicationUser>>()!;
        RoleManager<IdentityRole<Guid>> roleManager = 
            serviceProvier.GetService<RoleManager<IdentityRole<Guid>>>()!;

        Task.Run(async () =>
        {
            if (await roleManager.RoleExistsAsync(AdminRoleName))
            {
                return;
            }

            var role = new IdentityRole<Guid>(AdminRoleName);
            await roleManager.CreateAsync(role);

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            await userManager.AddToRoleAsync(adminUser, AdminRoleName);
        })
            .GetAwaiter()
            .GetResult();

        return app;
    }
}
