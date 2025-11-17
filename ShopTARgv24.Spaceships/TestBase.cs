using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShopTARgv24.ApplicationServices.Services;
using ShopTARgv24.Core.ServiceInterface;
using ShopTARgv24.Data;
using ShopTARgv24.RealEstateTest.Mock;

namespace ShopTARgv24.SpaceShipsTest
{
    public class TestBase
    {
        private readonly IServiceProvider _serviceProvider;

        public TestBase()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ShopTARgv24Context>(options =>
                options.UseInMemoryDatabase("SpaceShipsTestDb"));

            services.AddSingleton<IHostEnvironment, MockHostEnvironment>();

            services.AddScoped<IFileServices, FileServices>();

            services.AddScoped<ISpaceshipsServices, SpaceshipsServices>();

            _serviceProvider = services.BuildServiceProvider();
        }

        protected T Svc<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }
    }
}