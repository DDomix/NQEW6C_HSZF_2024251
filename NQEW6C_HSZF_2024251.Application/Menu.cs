using NQEW6C_HSZF_2024251.Persistence.MsSql;

namespace NQEW6C_HSZF_2024251.Application
{
    public class Menu
    {
        private readonly IF1Service _service;
        private readonly DatabaseSeeder _seeder;
        private readonly ToConsole _toConsole;

        public Menu(AppDBContext context, DatabaseSeeder seeder, IF1Service f1Service, ToConsole console)
        {
            _service = f1Service;
            _seeder = seeder;
            _toConsole = console;
        }

        public async Task ShowMainMenuAsync()
        {
            bool exitMainMenu = false;

            while (!exitMainMenu)
            {
                Console.Clear();
                Console.WriteLine("Főmenü:");
                Console.WriteLine("1. Adatbázis feltöltése");
                Console.WriteLine("2. Lekérdezések");
                Console.WriteLine("ESC: Kilépés");
                Console.Write("Kérjük, válasszon egy opciót (1, 2 vagy ESC): ");

                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        await ShowDatabaseMenuAsync();
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        await ShowQueryMenuAsync();
                        break;
                    case ConsoleKey.Escape:
                        exitMainMenu = true;
                        break;
                    default:
                        Console.WriteLine("Érvénytelen választás. Kérjük, próbálja újra.");
                        break;
                }
            }
        }

        private async Task ShowDatabaseMenuAsync()
        {
            bool backToMainMenu = false;

            while (!backToMainMenu)
            {
                Console.Clear();
                Console.WriteLine("Almenü: Adatbázis feltöltés");
                Console.WriteLine("1. Adatbázis feltöltése alapértelmezett módon");
                Console.WriteLine("2. JSON fájl megadása adatbázis feltöltéséhez");
                Console.WriteLine("ESC: Vissza a főmenübe");
                Console.Write("Kérjük, válasszon egy opciót (1, 2 vagy ESC): ");

                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        await FillDatabaseWithDefaultDataAsync();
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        await FillDatabaseFromJsonFileAsync();
                        break;
                    case ConsoleKey.Escape:
                        backToMainMenu = true;
                        break;
                    default:
                        Console.WriteLine("Érvénytelen választás. Kérjük, próbálja újra.");
                        break;
                }
            }
        }

        private async Task ShowQueryMenuAsync()
        {
            bool backToMainMenu = false;

            while (!backToMainMenu)
            {
                Console.Clear();
                Console.WriteLine("Lekérdezések: ");
                Console.WriteLine("1. Összes csapat lekérdezése");
                Console.WriteLine("2. Csapat lekérdezése ID alapján");
                Console.WriteLine("3. Csapat lekérdezése csapat név alapján");
                Console.WriteLine("ESC: Vissza a főmenübe");
                Console.Write("Kérjük, válasszon egy opciót (1, 2, 3 vagy ESC): ");

                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        var allteams = _service.GetTeamsEntity();
                        _toConsole.Display(allteams);
                        Console.WriteLine("Nyomjon meg egy billentyűt a folytatáshoz...");
                        Console.ReadKey();
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        Console.WriteLine("Adja meg az azonosítót (id): ");
                        if (int.TryParse(Console.ReadLine(), out int id))
                        {
                            var team = _service.GetF1EntityById(id);
                            if (team != null)
                            {
                                _toConsole.Display(team);
                            }
                            else
                            {
                                Console.WriteLine("Nincs ilyen azonosítóval rendelkező csapat.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Érvénytelen azonosító.");
                        }
                        Console.WriteLine("Nyomjon meg egy billentyűt a folytatáshoz...");
                        Console.ReadKey();
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        Console.WriteLine("Adja meg a csapat nevét: ");
                        string teamName = Console.ReadLine();
                        var teams = _service.GetTeamsEntityByName(teamName);
                        _toConsole.Display(teams);
                        Console.WriteLine("Nyomjon meg egy billentyűt a folytatáshoz...");
                        Console.ReadKey();
                        break;
                    case ConsoleKey.Escape:
                        backToMainMenu = true;
                        break;
                    default:
                        Console.WriteLine("Érvénytelen választás. Kérjük, próbálja újra.");
                        break;
                }
            }
        }


        private async Task FillDatabaseWithDefaultDataAsync()
        {
            Console.WriteLine("Adatbázis feltöltése alapértelmezett adatokkal...");
            await _seeder.SeedDataAsync("F1TeamsData.json"); // alapértelmezett JSON fájl használata
            Console.WriteLine("Feltöltés sikeres.");
            Console.WriteLine("Nyomjon meg egy billentyűt a folytatáshoz...");
            Console.ReadKey();
        }

        private async Task FillDatabaseFromJsonFileAsync()
        {
            Console.Write("Adja meg a JSON fájl elérési útját (üres ENTER a kilépéshez): ");
            string filePath = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath))
            {
                Console.WriteLine("Adatbázis feltöltése a megadott JSON fájl alapján...");
                await _seeder.SeedDataAsync(filePath);
                Console.WriteLine("Feltöltés sikeres.");
            }
            else
            {
                Console.WriteLine("A megadott fájl nem található.");
            }

            Console.WriteLine("Nyomjon meg egy billentyűt a folytatáshoz...");
            Console.ReadKey();
        }
    }
}
