using E_Ticket_Stor.Data;
using E_Ticket_Stor.Models;
using E_Ticket_Stor.Repository;
using E_Ticket_Stor.Repository.IRepository;
using E_Ticket_Stor.Utility;
using E_Ticket_Stor.Utility.DBInitializer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Ticket_Stor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Add DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 2. Add Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // 3. Configure authentication cookies
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

           
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IUserOTPRepository, UserOTPRepository>();
            builder.Services.AddScoped<IActorRepository, ActorRepository>();
            builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            builder.Services.AddScoped<ICinemaRepository, CinemaRepository>();
            builder.Services.AddScoped<IDBInitializer, DBInitializer>();

            var app = builder.Build();

            // 6. Error handling
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // 7. Middleware
            app.UseHttpsRedirection();
            app.UseStaticFiles(); // تأكد أنك تستخدم هذه بدل MapStaticAssets لو كانت غير معرفة

            app.UseRouting();

            app.UseAuthentication(); // مهم جدًا قبل Authorization
            app.UseAuthorization();

            // 8. Initialize Database
            using (var scope = app.Services.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDBInitializer>();
                dbInitializer.Initialize();
            }

            // 9. Route
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
