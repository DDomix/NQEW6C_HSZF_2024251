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
                Console.WriteLine("1. Adatbázis műveletek");
                Console.WriteLine("2. Lekérdezések");
                Console.WriteLine("3. Admin");
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
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        await ShowAdminMenuAsync();
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
                Console.WriteLine("Lekérdezések:");
                Console.WriteLine("1. Csapat lekérdezések");
                Console.WriteLine("2. Költségvetés lekérdezések");
                Console.WriteLine("ESC: Vissza a főmenübe");
                Console.Write("Kérjük, válasszon egy opciót (1, 2 vagy ESC): ");

                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        TeamQueries(false); // Csapat lekérdezések almenü
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        Console.WriteLine("Költségvetés lekérdezések (TODO: implementáció)");
                        Console.WriteLine("Nyomjon meg egy billentyűt a folytatáshoz...");
                        Console.ReadKey();
                        break;
                    case ConsoleKey.Escape:
                        backToMainMenu = true; // Visszatérés a főmenübe
                        break;
                    default:
                        Console.WriteLine("Érvénytelen választás. Kérjük, próbálja újra.");
                        break;
                }
            }
        }
        private async Task ShowAdminMenuAsync()
        {
            bool backToMainMenu = false;
        
            while (!backToMainMenu)
            {
                Console.Clear();
                Console.WriteLine("Adminisztrációs menü:");
                Console.WriteLine("1. Csapatok frissítése");
                Console.WriteLine("2. Csapatok törlése");
                Console.WriteLine("3. Adatbázis törlése törlése");
                Console.WriteLine("ESC: Vissza a főmenübe");
                Console.Write("Kérjük, válasszon egy opciót (1, 2 vagy ESC): ");
        
                var key = Console.ReadKey(true);
        
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        await UpdateTeamAsync();
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        DeleteTeamAsync();
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        DeleteDataBaseAsync();
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


        private bool TeamQueries(bool backToQueryMenu)
        {
            while (!backToQueryMenu)
            {
                Console.Clear();
                Console.WriteLine("Csapat lekérdezések:");
                Console.WriteLine("1. Azonosító (id)");
                Console.WriteLine("2. Csapatnév");
                Console.WriteLine("3. Év");
                Console.WriteLine("4. Főhadiszállás");
                Console.WriteLine("5. Csapatfőnök neve");
                Console.WriteLine("6. Konstruktőri címek");
                Console.WriteLine("ESC: Vissza a lekérdezések menübe");
                Console.Write("Kérjük, válasszon egy opciót (1-6 vagy ESC): ");

                var keyasd = Console.ReadKey(true);

                switch (keyasd.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        GetTeamEntityByID();
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        GetTeamEntitiesByName();
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        GetTeamsByYear();
                        break;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        GetTeamsByHeadquarters();
                        break;
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        GetTeamsByTeamPrincipal();
                        break;
                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        GetTeamsByConstructorTitles();
                        break;
                    case ConsoleKey.Escape:
                        backToQueryMenu = true;
                        break;
                    default:
                        Console.WriteLine("Érvénytelen választás. Kérjük, próbálja újra.");
                        break;
                }
            }
            return backToQueryMenu;
        }


        private void DeleteDataBaseAsync()
        {
            Console.Clear();
            Console.WriteLine("Adatbázis törlése");
            Console.Write("Biztosan törli az adatbázis minden adatát? (i/n): ");
            if (Console.ReadLine().Trim().ToLower() == "i")
            {
                var teamdata = _service.GetTeamsEntity();
                var budgetdata = _service.GetBudgetEntities();
                var expensedata = _service.GetExpensesEntities();
                
                _service.DeleteTeamDataBase(teamdata);
                _service.DeleteBudgetDataBase(budgetdata);
                _service.DeleteExpenseDataBase(expensedata);
                Console.WriteLine("Adatbázis sikeresen törölve.");
            }
            else
            {
                Console.WriteLine("Az adatbázis törlése megszakítva.");
            }
        }
        private async Task UpdateTeamAsync()
        {
            Console.Clear();
            Console.WriteLine("Csapat frissítése:");
            Console.Write("Adja meg a frissítendő csapat azonosítóját: ");

            if (int.TryParse(Console.ReadLine(), out int teamId))
            {
                var team = _service.GetF1EntityById(teamId);

                if (team == null)
                {
                    Console.WriteLine("Nincs ilyen azonosítóval rendelkező csapat.");
                }
                else
                {
                    Console.WriteLine("Jelenlegi adatok:");
                    _toConsole.Display(team);

                    Console.WriteLine("Hagyja üresen az új értéket, ha nem szeretné módosítani azt a mezőt.");
                    Console.Write("Új csapatnév: ");
                    string newName = Console.ReadLine();
                    Console.Write("Új főhadiszállás: ");
                    string newHeadquarters = Console.ReadLine();
                    Console.Write("Új csapatfőnök neve: ");
                    string newPrincipal = Console.ReadLine();
                    Console.Write("Új konstruktőri címek száma: ");
                    int? newTitles = null;
                    string titlesInput = Console.ReadLine();
                    if (int.TryParse(titlesInput, out int parsedTitles)) newTitles = parsedTitles;

                    team.TeamName = string.IsNullOrWhiteSpace(newName) ? team.TeamName : newName;
                    team.HeadQuarters = string.IsNullOrWhiteSpace(newHeadquarters) ? team.HeadQuarters : newHeadquarters;
                    team.TeamPrincipal = string.IsNullOrWhiteSpace(newPrincipal) ? team.TeamPrincipal : newPrincipal;
                    team.ConstructorsChampionshipWins = newTitles ?? team.ConstructorsChampionshipWins;

                    _service.AddOrUpdateTeam(team);

                    Console.WriteLine("Csapat sikeresen frissítve.");
                }
            }
            else
            {
                Console.WriteLine("Érvénytelen azonosító.");
            }

            Console.WriteLine("Nyomjon meg egy billentyűt a folytatáshoz...");
            Console.ReadKey();
        }

        private void DeleteTeamAsync()
        {
            Console.Clear();
            Console.WriteLine("Csapat törlése:");
            Console.Write("Adja meg a törlendő csapat azonosítóját: ");

            if (int.TryParse(Console.ReadLine(), out int teamId))
            {
                var team = _service.GetF1EntityById(teamId);

                if (team == null)
                {
                    Console.WriteLine("Nincs ilyen azonosítóval rendelkező csapat.");
                }
                else
                {
                    Console.WriteLine("Törlendő csapat adatai:");
                    _toConsole.Display(team);

                    Console.Write("Biztosan törli ezt a csapatot? (i/n): ");
                    if (Console.ReadLine().Trim().ToLower() == "i")
                    {
                        _service.DeleteTeam(teamId);
                        Console.WriteLine("Csapat sikeresen törölve.");
                    }
                    else
                    {
                        Console.WriteLine("A csapat törlése megszakítva.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Érvénytelen azonosító.");
            }

            Console.WriteLine("Nyomjon meg egy billentyűt a folytatáshoz...");
            Console.ReadKey();
        }



        private void GetTeamsByConstructorTitles()
        {
            Console.Clear();
            Console.WriteLine("Adja meg a konstruktőri címek számát: ");
            if (int.TryParse(Console.ReadLine(), out int titleCount))
            {
                var teams = _service.GetTeamsByConstructorTitles(titleCount);
                if (teams.Any())
                {
                    _toConsole.Display(teams);
                }
                else
                {
                    Console.WriteLine("Nincs csapat a megadott konstruktőri címek számával.");
                }
            }
            else
            {
                Console.WriteLine("Érvénytelen számformátum.");
            }
            Console.WriteLine("Nyomjon meg egy billentyűt a folytatáshoz...");
            Console.ReadKey();
        }


        private void GetTeamsByTeamPrincipal()
        {
            Console.Clear();
            Console.WriteLine("Adja meg a csapatfőnök nevét: ");
            string principalName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(principalName))
            {
                var teams = _service.GetTeamsByPrincipal(principalName);
                if (teams.Any())
                {
                    _toConsole.Display(teams);
                }
                else
                {
                    Console.WriteLine("Nincs csapat a megadott csapatfőnök nevű vezetővel.");
                }
            }
            else
            {
                Console.WriteLine("Érvénytelen bemenet.");
            }
            Console.WriteLine("Nyomjon meg egy billentyűt a folytatáshoz...");
            Console.ReadKey();
        }


        private void GetTeamsByHeadquarters()
        {
            Console.Clear();
            Console.WriteLine("Adja meg a főhadiszállás nevét: ");
            string headquarters = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(headquarters))
            {
                var teams = _service.GetTeamsByHeadquarters(headquarters);
                if (teams.Any())
                {
                    _toConsole.Display(teams);
                }
                else
                {
                    Console.WriteLine("Nincs csapat a megadott főhadiszállással.");
                }
            }
            else
            {
                Console.WriteLine("Érvénytelen bemenet.");
            }
            Console.WriteLine("Nyomjon meg egy billentyűt a folytatáshoz...");
            Console.ReadKey();
        }


        private void GetTeamsByYear()
        {
            Console.Clear();
            Console.WriteLine("Év lekérdezése:");
            Console.WriteLine();
            Console.Write("Adja meg az évet: ");
            if (int.TryParse(Console.ReadLine(), out int year))
            {
                var yearentities = _service.GetTeamEntitiesByYear(year);
                if (yearentities.Count == 0)
                {
                    Console.WriteLine("Nincs adat a megadott évhez. Próbálja újra.");
                    Console.WriteLine("Nyomjon meg egy billentyűt a folytatáshoz...");
                    Console.ReadKey();
                }
                else
                {
                    _toConsole.Display(yearentities);
                    Console.WriteLine("Nyomjon meg egy billentyűt a folytatáshoz...");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Érvénytelen évformátum. Próbálja újra.");
                Console.WriteLine("Nyomjon meg egy billentyűt a folytatáshoz...");
                Console.ReadKey();
            }
        }

        private void GetTeamEntityByID()
        {
            Console.WriteLine();
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
        }

        private void GetTeamEntitiesByName()
        {
            Console.Clear();
            Console.WriteLine("2. Csapat lekérdezése csapat név alapján");
            Console.WriteLine("1.Pontos nevet ad meg");
            Console.WriteLine("2.Megközelítőleges nevet ad meg");
            Console.Write("Kérjük, válasszon egy opciót (1, 2, 3 vagy ESC): ");
            var key2 = Console.ReadKey(true);

            switch (key2.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    Console.WriteLine();
                    Console.WriteLine("Adja meg pontosan a csapat nevét: ");
                    string exactteamName = Console.ReadLine();
                    var exactteams = _service.GetTeamEntityByExactName(exactteamName);
                    _toConsole.Display(exactteams);
                    Console.WriteLine("Nyomjon meg egy billentyűt a folytatáshoz...");
                    Console.ReadKey();
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    Console.WriteLine();
                    Console.WriteLine("Adja meg a csapat nevét: ");
                    string teamName = Console.ReadLine();
                    var teams = _service.GetTeamEntityByName(teamName);
                    _toConsole.Display(teams);
                    Console.WriteLine("Nyomjon meg egy billentyűt a folytatáshoz...");
                    Console.ReadKey();
                    break;
                default:
                    Console.WriteLine("Érvénytelen választás. Kérjük, próbálja újra.");
                    break;
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
