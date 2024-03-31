using Microsoft.Extensions.DependencyInjection;

namespace SP.Domain
{
    public class MainModule
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<Settings>();
            services.AddSingleton<Logger>();
            services.AddSingleton<FiltersManager>();
            services.AddSingleton<ShemeManager>();
        }
    }
}
