using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NQEW6C_HSZF_2024251.Application;
using NQEW6C_HSZF_2024251.Model;
using NQEW6C_HSZF_2024251.Persistence.MsSql;

namespace NQEW6C_HSZF_2024251
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Application....");

            var host = Host.CreateDefaultBuilder()
                            .ConfigureServices((hostContext, services) =>
                            {
                                //TODO: Add services
                                services.AddScoped<AppDBContext>();

                                services.AddSingleton<IF1DataProvider, F1DataProvider>();
                                services.AddSingleton<IF1Service, F1Service>();
                            })
                            .Build();

            host.Start();

            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;
            IF1Service f1Service = serviceProvider.GetRequiredService<IF1Service>();

            using (var context = new AppDBContext())
            {
                var budget = new Budget
                {
                    TotalBudget = 202302302
                };

                context.Budgets.Add(budget);
                context.SaveChanges();

                var team = new TeamsEntity
                {
                    TeamName = "Mercedes",
                    HeadQuarters = "London",
                    TeamPrincipal = "Toto Wolf",
                    ConstructorsChampionshipWins = 7,
                    Year = 2023,
                    BudgetId = budget.Id 
                };
                
                context.Teams.Add(team);
                context.SaveChanges();
                context.RemoveRange(0, 12);
            }
        }
    }
}

