using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.Interfaces;
using ECommerce.Models.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            string connString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services
                .AddDbContext<ApplicationDbContext>
            (opions => opions.UseSqlServer(connString));
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddControllersWithViews();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<ApplicationDbContext>();


            builder.Services.AddTransient<IUser, IdentityUserService>();
            builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
            builder.Services.AddTransient<IProduct, ProductService>();
            builder.Services.AddTransient<ICategory, CategoryService>();


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
                pattern: "{controller=Main}/{action=Index}/{id?}");

            app.Run();
        }
    }
}