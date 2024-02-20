namespace HouseRentingSystem.Web;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using System.Reflection;

using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Models;
using HouseRentingSystem.Services.Mapping;
using HouseRentingSystem.Services.Interfaces;
using HouseRentingSystem.Web.ViewModels.Home;
using HouseRentingSystem.Web.Infrastructure.Extensions;
using HouseRentingSystem.Web.Infrastructure.Middlewares;
using HouseRentingSystem.Web.Infrastructure.ModelBinders;

using static HouseRentingSystem.Common.GeneralApplicationConstants;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<HouseRentingSystemDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount =
                builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
            options.Password.RequireLowercase =
                builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");
            options.Password.RequireUppercase =
                builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");
            options.Password.RequireNonAlphanumeric =
                builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric");
            options.Password.RequiredLength =
                builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");
        })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<HouseRentingSystemDbContext>();

        builder.Services.AddControllersWithViews()
            .AddMvcOptions(opt =>
            {
                opt.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
                opt.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            });

        builder.Services.AddServicesReflection(typeof(IHouseService));
        builder.Services.AddRecaptchaService();

        builder.Services.AddMemoryCache();

        builder.Services.ConfigureApplicationCookie(opt =>
        {
            opt.AccessDeniedPath = "/Home/Error/401";
        });

        var app = builder.Build();

        AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error/500");
            app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<OnlineUsersMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.AddUserInAdnimRole(AdminEmail);
        }

        app.UseEndpoints(config =>
        {
            config.MapControllerRoute(
            name: "areas",
            pattern: "/{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );

            config.MapDefaultControllerRoute();
            config.MapRazorPages();
        });

        app.Run();
    }
}
