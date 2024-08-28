using System.Text;
using Microsoft.EntityFrameworkCore;
using SimpleBlog.Data.Context;
using SimpleBlog.MVC.Shared;
using SimpleBlog.MVC.Validation.Extensions;
using SimpleBlog.Services.Extensions;

namespace SimpleBlog.MVC;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();
        
        builder.Services.AddDistributedMemoryCache();
        
        builder.Services.AddHttpContextAccessor();
        
        builder.Services.AddScoped<ISessionHandler, SessionHandler>();
        
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDb"),
                npgsqlOptionsAction: npSqlActions =>
                {
                    npSqlActions.EnableRetryOnFailure();
                });
            options.EnableDetailedErrors();
        });
        
        builder.Services.AddDbServices();

        builder.Services.AddValidationServices();
        
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromHours(1);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        WebApplication app = builder.Build();

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

        app.UseAuthorization();

        app.UseSession();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}