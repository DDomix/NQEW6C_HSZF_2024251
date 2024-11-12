using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NQEW6C_HSZF_2024251.Model;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NQEW6C_HSZF_2024251.Persistence.MsSql
{
    public class DatabaseSeeder
    {
        private readonly AppDBContext _context;

        public DatabaseSeeder(AppDBContext context)
        {
            _context = context;
        }
        public async Task SeedDataAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("A JSON fájl nem található.");
                return;
            }

            var jsonData = await File.ReadAllTextAsync(filePath);
            var teams = JsonConvert.DeserializeObject<List<TeamsEntity>>(jsonData);

            if (teams == null || !teams.Any())
            {
                Console.WriteLine("Nincs új adat feltöltésre.");
                return;
            }

            foreach (var team in teams)
            {
                var existingTeam = _context.Teams
                    .Include(t => t.Budget)
                    .ThenInclude(b => b.Expenses)
                    .FirstOrDefault(t => t.TeamName == team.TeamName && t.Year == team.Year);

                if (existingTeam == null)
                {
                    _context.Teams.Add(team);
                }
                else
                {
                    existingTeam.TeamPrincipal = team.TeamPrincipal;
                    existingTeam.ConstructorsChampionshipWins = team.ConstructorsChampionshipWins;

                    if (existingTeam.Budget == null)
                    {
                        existingTeam.Budget = team.Budget;
                    }
                    else
                    {
                        existingTeam.Budget.TotalBudget = team.Budget.TotalBudget;

                        foreach (var expense in team.Budget.Expenses)
                        {
                            var existingExpense = existingTeam.Budget.Expenses
                                .FirstOrDefault(e => e.Category == expense.Category && e.ExpenseDate == expense.ExpenseDate);

                            if (existingExpense == null)
                            {
                                // Új kiadás hozzáadása, ha még nem létezik
                                expense.Amount = expense.SubCategory.Sum(sc => sc.Amount);

                                if (existingTeam.Budget.TotalBudget >= existingTeam.Budget.Expenses.Sum(e => e.Amount) + expense.Amount)
                                {
                                    existingTeam.Budget.Expenses.Add(expense);
                                }
                                else
                                {
                                    Console.WriteLine($"A(z) {expense.Category} kiadás összege meghaladná a költségvetést, ezért kimarad.");
                                }
                            }
                            else
                            {
                                existingExpense.Amount = existingExpense.SubCategory.Sum(sc => sc.Amount);

                                if (existingTeam.Budget.TotalBudget >= existingTeam.Budget.Expenses.Sum(e => e.Amount))
                                {
                                    foreach (var subCategory in expense.SubCategory)
                                    {
                                        var existingSubCategory = existingExpense.SubCategory
                                            .FirstOrDefault(sc => sc.Name == subCategory.Name);

                                        if (existingSubCategory == null)
                                        {
                                            existingExpense.SubCategory.Add(subCategory);
                                            existingExpense.Amount += subCategory.Amount;
                                        }
                                        else
                                        {
                                            existingSubCategory.Amount = subCategory.Amount;
                                        }
                                    }

                                    existingExpense.Amount = existingExpense.SubCategory.Sum(sc => sc.Amount);
                                }
                                else
                                {
                                    Console.WriteLine($"A(z) {expense.Category} kiadás frissítése meghaladná a költségvetést, ezért nem kerül frissítésre.");
                                }
                            }
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
            Console.WriteLine("Az adatbázis frissítése sikeresen megtörtént.");
        }




    }
}

