using Microsoft.Extensions.DependencyInjection;
using ShopTARgv24.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ZendeskApi_v2.Requests;
using ShopTARgv24.Core.ServiceInterface;
using ShopTARgv24.ApplicationServices.Services;
using Microsoft.Extensions.Hosting;
using ShopTARgv24.KindergartenTest.Mock;

namespace ShopTARgv24.KindergartenTest
{
    public abstract class TestBase
    {
        protected IServiceProvider serviceProvider { get; set; }

        protected TestBase()
        {
            var services = new ServiceCollection();
            SetupServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        public virtual void SetupServices(IServiceCollection services)
        {
            services.AddScoped<IKindergartenServices, KindergartenServices>();
            services.AddScoped<IFileServices, FileServices>();
            services.AddScoped<IHostEnvironment, MockHostEnvironment>();

            services.AddDbContext<ShopTARgv24Context>(x =>
            {
                object value = x.UseInMemoryDatabase("TestDb");
                x.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            RegisterMacros(services);
        }

        private void RegisterMacros(IServiceCollection services)
        {
            var macroBaseType = typeof(Macros.IMacros);

            var macros = macroBaseType.Assembly.GetTypes()
                .Where(t => macroBaseType.IsAssignableFrom(t)
                && !t.IsInterface && !t.IsAbstract);
        }
        protected T Svc<T>()
        {
            return serviceProvider.GetService<T>();
        }


        public void Dispose()
        {

        }
    }
}