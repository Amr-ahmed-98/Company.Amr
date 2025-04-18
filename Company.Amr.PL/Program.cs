using Company.Amr.BLL;
using Company.Amr.BLL.Interfaces;
using Company.Amr.BLL.Repositories;
using Company.Amr.DAL.Data.Contexts;
using Company.Amr.DAL.Models;
using Company.Amr.PL.Helper;
using Company.Amr.PL.Mapping;
using Company.Amr.PL.Services;
using Company.Amr.PL.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Company.Amr.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(); // Register Built-In MVC Services
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // Allow Dependency Injection for DepartmentRepository
            builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>(); // Allow Dependency Injection for EmployeeRepository
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>(); // Allow Dependency Injection for UnitOfWork
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }); // Allow Dependency Injection for CompanyDbContext

            //builder.Services.AddAutoMapper(typeof(EmployeeProfile));
            builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile())); // Allow Dependency Injection for EmployeeProfile());

            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
            builder.Services.AddScoped<IMailService, MailService>(); // Allow Dependency Injection for MailService>



            // Life Time
            //builder.Services.AddScoped();     // Create Object Life Time Per Request - Unreachable Request
            //builder.Services.AddTransient();  // Create Object Life Time Per Operation
            //builder.Services.AddSingleton();  // Create Object Life Time Per App

            builder.Services.AddScoped<IScopedServices, ScopedServices>();              // Per Request
            builder.Services.AddTransient<ITransentServices, TransentServices>();       // Per Operation
            builder.Services.AddSingleton<ISingletonServices, SingletonServices>();     // Per App

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<CompanyDbContext>()
                .AddDefaultTokenProviders();


            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
                //config.LogoutPath = "/Home/SignIn";
                //config.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();
        

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}



