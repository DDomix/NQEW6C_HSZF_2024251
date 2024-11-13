using NQEW6C_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NQEW6C_HSZF_2024251.Persistence.MsSql;
using System.Threading.Channels;
using System.Reflection.Metadata;

namespace NQEW6C_HSZF_2024251.Application
{
    public class Menu
    {
        private readonly AppDBContext _context;
        private readonly DatabaseSeeder _seeder;
        private readonly F1DataProvider _provider;
        private readonly ToConsole _toConsole;

        public Menu(AppDBContext context, DatabaseSeeder seeder, F1DataProvider provider, ToConsole toConsole)
        {
            _context = context;
            _seeder = seeder;
            _provider = provider;
            _toConsole = toConsole;
        }

        public async Task ShowMainMenuAsync()
        {
            Console.WriteLine("Főmenü:");
            Console.WriteLine("1. Adatbázis feltöltése");
            Console.WriteLine("2. Lekérdezések");
            Console.Write("Kérjük, válasszon egy opciót (1 vagy 2): ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine("Almenü: Adatbázis feltöltés");
                    Console.WriteLine("1. Adatbázis feltöltése alapértelmezett módon");
                    Console.WriteLine("2. JSON fájl megadása adatbázis feltöltéséhez");
                    Console.Write("Kérjük, válasszon egy opciót (1 vagy 2): ");
                    choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            await FillDatabaseWithDefaultDataAsync();
                            break;
                        case "2":
                            await FillDatabaseFromJsonFileAsync();
                            break;
                        default:
                            Console.WriteLine("Érvénytelen választás. Kérjük, próbálja újra.");
                            await ShowMainMenuAsync();
                            break;
                    }
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine("Almenü: GetTeamEntities");
                    Console.WriteLine("Adja meg az azonosítót(id): ");
                    int id = int.Parse(Console.ReadLine());
                    var team=_provider.GetTeamsEntityById(id);
                    _toConsole.Display(team);
                    break;
                default:
                    Console.WriteLine("Érvénytelen választás. Kérjük, próbálja újra.");
                    await ShowMainMenuAsync();
                    break;
            }


        }

        private async Task FillDatabaseWithDefaultDataAsync()
        {
            // Alapértelmezett adatok feltöltése
            Console.WriteLine("Adatbázis feltöltése alapértelmezett adatokkal...");
            await _seeder.SeedDataAsync("F1TeamsData.json"); // alapértelmezett JSON fájl használata
            Console.WriteLine("Feltöltés sikeres.");
        }

        private async Task FillDatabaseFromJsonFileAsync()
        {
            Console.Write("Adja meg a JSON fájl(ok) elérési útját. Nyomjon üres ENTERT a bevitel leállításához");
            Console.WriteLine();
            string filePath;
            do
            {
                filePath = Console.ReadLine();
                if (File.Exists(filePath))
                {
                    Console.WriteLine("Adatbázis feltöltése a megadott JSON fájl alapján...");
                    await _seeder.SeedDataAsync(filePath);
                    Console.WriteLine("Feltöltés sikeres.");
                    
                }
                else
                {
                    Console.WriteLine("A megadott fájl nem található.");
                    
                }
            }
            while (filePath!="");
        }
    }
}
