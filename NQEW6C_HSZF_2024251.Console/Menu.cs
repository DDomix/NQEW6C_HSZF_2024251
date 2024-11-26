using NQEW6C_HSZF_2024251.Model;
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
                        TeamQueries(false); 
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        Console.WriteLine("Költségvetés lekérdezések (TODO: implementáció)");
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
        private async Task ShowAdminMenuAsync()
        {
            bool backToMainMenu = false;
        
            while (!backToMainMenu)
            {
                Console.Clear();
                Console.WriteLine("Adminisztrációs menü:");
                Console.WriteLine("1. Csapat hozzáadása");
                Console.WriteLine("2. Csapatok frissítése");
                Console.WriteLine("3. Csapatok törlése");
                Console.WriteLine("4. Budget hozzáadása");
                Console.WriteLine("5. Budget frissítése");
                Console.WriteLine("ESC: Vissza a főmenübe");
                Console.Write("Kérjük, válasszon egy opciót (1, 2 vagy ESC): ");
        
                var key = Console.ReadKey(true);
        
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        await AddTeamAsync();
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        await UpdateTeamAsync();
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        DeleteTeamAsync();
                        break;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        AddBudgetAsync();
                        break;
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        UpdateBudgetAsync();
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

        private async Task AddTeamAsync()
        {
            Console.Clear();
            Console.WriteLine("Új csapat hozzáadása:");

            try
            {
                // Csapat adatok bekérése a felhasználótól
                Console.Write("Csapat neve: ");
                string teamName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(teamName))
                {
                    Console.WriteLine("A csapat neve nem lehet üres. Nyomjon meg egy gombot a folytatáshoz...");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Alapítás éve: ");
                if (!int.TryParse(Console.ReadLine(), out int year) || year <= 0)
                {
                    Console.WriteLine("Érvénytelen év. Nyomjon meg egy gombot a folytatáshoz...");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Székhely: ");
                string headquarters = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(headquarters))
                {
                    Console.WriteLine("A székhely nem lehet üres. Nyomjon meg egy gombot a folytatáshoz...");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Csapatfőnök neve: ");
                string teamPrincipal = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(teamPrincipal))
                {
                    Console.WriteLine("A csapatfőnök neve nem lehet üres. Nyomjon meg egy gombot a folytatáshoz...");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Konstruktor bajnoki címek száma: ");
                if (!int.TryParse(Console.ReadLine(), out int constructorsChampionshipWins) || constructorsChampionshipWins < 0)
                {
                    Console.WriteLine("Érvénytelen bajnoki cím szám. Nyomjon meg egy gombot a folytatáshoz...");
                    Console.ReadKey();
                    return;
                }

                // Költségvetési ID bekérése vagy létrehozása
                Console.Write("Költségvetési ID: ");
                int budgetId;
                
                Console.WriteLine("Érvénytelen költségvetési ID. Új költségvetés létrehozása...");
                var newBudget = await AddBudgetAsync();
                if (newBudget == null)
                {
                    Console.WriteLine("Költségvetés létrehozása sikertelen. Nyomjon meg egy gombot a folytatáshoz...");
                    Console.ReadKey();
                    return;
                }
                budgetId = newBudget.Id;
                

                // Új csapat létrehozása
                var newTeam = new TeamsEntity
                {
                    TeamName = teamName,
                    Year = year,
                    HeadQuarters = headquarters,
                    TeamPrincipal = teamPrincipal,
                    ConstructorsChampionshipWins = constructorsChampionshipWins,
                    BudgetId = budgetId
                };

                // Csapat mentése az adatbázisba
                _service.AddOrUpdateTeam(newTeam);

                Console.WriteLine("Az új csapat sikeresen hozzáadva.");
                Console.WriteLine("Nyomjon meg egy gombot a folytatáshoz...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba történt a csapat hozzáadása során: {ex.Message}");
                Console.WriteLine("Nyomjon meg egy gombot a folytatáshoz...");
                Console.ReadKey();
            }
        }

        private async Task<Budget> AddBudgetAsync()
        {
            Console.Clear();
            Console.WriteLine("Új költségvetés létrehozása:");

            try
            {
                Console.Write("Teljes költségvetés (összeg): ");
                if (!int.TryParse(Console.ReadLine(), out int totalBudget) || totalBudget <= 0)
                {
                    Console.WriteLine("Érvénytelen összeg. Nyomjon meg egy gombot a folytatáshoz...");
                    Console.ReadKey();
                    return null;
                }

                var newBudget = new Budget
                {
                    TotalBudget = totalBudget
                };

                
                _service.AddOrUpdateBudget(newBudget);

                Console.WriteLine("Az új költségvetés sikeresen létrehozva.");
                Console.WriteLine("Nyomjon meg egy gombot a folytatáshoz...");
                Console.ReadKey();

                return newBudget;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba történt a költségvetés létrehozása során: {ex.Message}");
                Console.WriteLine("Nyomjon meg egy gombot a folytatáshoz...");
                Console.ReadKey();
                return null;
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


        private async Task UpdateBudgetAsync()
        {
            Console.Clear();
            Console.WriteLine("Budget frissítése:");

            try
            {
                // A meglévő Budget entitások listázása
                var budgets = _service.GetBudgetEntities();
                if (budgets == null || !budgets.Any())
                {
                    Console.WriteLine("Nincs elérhető budget az adatbázisban.");
                    Console.WriteLine("Nyomjon meg egy gombot a folytatáshoz...");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Elérhető budgetek:");
                foreach (var budget in budgets)
                {
                    Console.WriteLine($"ID: {budget.Id}, Csapat név: {_service.GetTeamsEntityByBudgetId(budget)}  Összeg: {budget.TotalBudget}");
                }

                Console.Write("Adja meg a frissíteni kívánt Budget ID-ját: ");
                if (!int.TryParse(Console.ReadLine(), out int budgetId))
                {
                    Console.WriteLine("Érvénytelen ID. Nyomjon meg egy gombot a folytatáshoz...");
                    Console.ReadKey();
                    return;
                }

                var selectedBudget = budgets.FirstOrDefault(b => b.Id == budgetId);
                if (selectedBudget == null)
                {
                    Console.WriteLine("Nem található a megadott ID-val Budget. Nyomjon meg egy gombot a folytatáshoz...");
                    Console.ReadKey();
                    return;
                }

                // Új összeg megadása
                Console.Write($"Adja meg az új összeget (jelenlegi: {selectedBudget.TotalBudget}): ");
                if (!int.TryParse(Console.ReadLine(), out int newTotalBudget) || newTotalBudget <= 0)
                {
                    Console.WriteLine("Érvénytelen összeg. Nyomjon meg egy gombot a folytatáshoz...");
                    Console.ReadKey();
                    return;
                }

                selectedBudget.TotalBudget = newTotalBudget;
                _service.UpdateBudget(selectedBudget.Id);

                Console.WriteLine("A budget sikeresen frissítve.");
                Console.WriteLine("Nyomjon meg egy gombot a folytatáshoz...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba történt a budget frissítése során: {ex.Message}");
                Console.WriteLine("Nyomjon meg egy gombot a folytatáshoz...");
                Console.ReadKey();
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

        private async Task DeleteTeamAsync()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Csapat törlése:");

                var teams = _service.GetTeamEntities();
                if (teams == null || !teams.Any())
                {
                    Console.WriteLine("Nincs elérhető csapat az adatbázisban.");
                    Console.WriteLine("Nyomjon meg egy gombot a folytatáshoz...");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Elérhető csapatok:");
                foreach (var team in teams)
                {
                    Console.WriteLine($"ID: {team.Id}, Csapat név: {team.TeamName}");
                }

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
                            _service.DeleteTeamsAndConnections(teamId);
                            Console.WriteLine("Csapat és kapcsolódó adatok sikeresen törölve.");
                        }
                        else
                        {
                            Console.WriteLine("A csapat törlése megszakítva.");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Nyomjon meg egy gombot a folytatáshoz...");
                Console.ReadKey();
            }
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

            _seeder.DataSeeded += (sender, message) =>
            {
                Console.WriteLine(message);
            };

            await _seeder.SeedDataAsync("F1TeamsData.json");

            Console.WriteLine("Nyomjon meg egy billentyűt a folytatáshoz...");
            Console.ReadKey();
        }

        private async Task FillDatabaseFromJsonFileAsync()
        {
            Console.Write("Adja meg a JSON fájl elérési útját (üres ENTER a kilépéshez): ");
            string filePath = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath))
            {
                _seeder.DataSeeded += (sender, message) =>
                {
                    Console.WriteLine(message);
                };

                await _seeder.SeedDataAsync(filePath);
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
