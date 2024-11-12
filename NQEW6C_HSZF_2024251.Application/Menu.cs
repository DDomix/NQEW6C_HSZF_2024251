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

        public Menu(AppDBContext context, DatabaseSeeder seeder)
        {
            _context = context;
            _seeder = seeder;
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
                    F1DataProvider asd = new F1DataProvider(_context);
                    asd.GetTeamEntities().Count();
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
            await _seeder.SeedDataAsync("C:\\Users\\Domi\\Desktop\\OE\\NQEW6C_HSZF_2024251.Console\\NQEW6C_HSZF_2024251.Persistence.MsSql\\F1TeamsData.json"); // alapértelmezett JSON fájl használata
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
