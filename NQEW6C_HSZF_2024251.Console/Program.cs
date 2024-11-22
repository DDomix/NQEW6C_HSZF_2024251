using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NQEW6C_HSZF_2024251.Application;
using NQEW6C_HSZF_2024251.Persistence.MsSql;
using System;
using System.Threading.Tasks;

namespace NQEW6C_HSZF_2024251
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Application...");

            var host = Host.CreateDefaultBuilder()
    .ConfigureServices((hostContext, services) =>
    {
        // Register DbContext
        services.AddDbContext<AppDBContext>();

        // Register the data provider and service using the interfaces
        services.AddScoped<IF1DataProvider, F1DataProvider>();
        services.AddScoped<IF1Service, F1Service>();

        // Register additional dependencies
        services.AddTransient<DatabaseSeeder>();
        services.AddTransient<Menu>();
        services.AddTransient<ToConsole>();
    })
    .Build();




            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    // Resolve services from the service provider
                    var menu = services.GetRequiredService<Menu>();

                    // Run the main menu

                    await menu.ShowMainMenuAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hiba történt: {ex.Message}");
                }
            }
        }
    }
}
