using Microsoft.EntityFrameworkCore;
using ShopTARgv24.Core.ServiceInterface;
using ShopTARgv24.ApplicationServices.Services;
using ShopTARgv24.Data;
namespace ShopTARgv24
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.Services.AddScoped< SpaceshipServices>();

            builder.Services.AddDbContext<ShopTARgv24Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ISpaceshipsServices, SpaceshipsServices>();
            builder.Services.AddScoped<IFileServices, FileServices>();
            builder.Services.AddScoped<IRealEstateServices, RealEstateServices>();
            builder.Services.AddScoped<IWeatherForecastServices, WeatherForecastServices>();
            // WebClient for ChuckNorrisServices
            builder.Services.AddScoped<IChuckNorrisServices, ChuckNorrisServices>();
            //HttpClient for ChuckNorrisServices
            builder.Services.AddHttpClient<IChuckNorrisServices, ChuckNorrisServices>();
            // HttpClient for CocktailServices
            builder.Services.AddHttpClient<ICocktailService, CocktailService>();
            // WebClient for OpenWeatherServices
            builder.Services.AddScoped<IOpenWeatherServices, OpenWeatherServices>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}