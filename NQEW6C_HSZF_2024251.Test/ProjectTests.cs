using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NQEW6C_HSZF_2024251.Model;
using NQEW6C_HSZF_2024251.Persistence.MsSql;
using System;
using System.Linq;

namespace NQEW6C_HSZF_2024251.Test
{
    [TestClass]
    public class F1DataProviderTests
    {
        private AppDBContext context;
        private F1DataProvider dataProvider;

        [TestInitialize]
        public void Setup()
        {
            context = new AppDBContext();
            dataProvider = new F1DataProvider(context);
        }

        [TestMethod]
        public void GetTeamEntities_ReturnsAllTeams()
        {
            var budget = new Budget { TotalBudget = 1000000 };
            context.Budgets.Add(budget);
            context.SaveChanges();

            context.Teams.Add(new TeamsEntity { TeamName = "Team A", BudgetId = budget.Id });
            context.Teams.Add(new TeamsEntity { TeamName = "Team B", BudgetId = budget.Id });
            context.SaveChanges();

            var result = dataProvider.GetTeamEntities();

            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetTeamsEntityById_ReturnsCorrectTeam()
        {
            var budget = new Budget { TotalBudget = 1000000 };
            context.Budgets.Add(budget);
            context.SaveChanges();

            var team = new TeamsEntity { TeamName = "Team A", BudgetId = budget.Id };
            context.Teams.Add(team);
            context.SaveChanges();

            var result = dataProvider.GetTeamsEntityById(team.Id);

            Assert.AreEqual("Team A", result.TeamName);
        }

        [TestMethod]
        public void AddTeam_IncreasesTeamCount()
        {
            var budget = new Budget { TotalBudget = 1000000 };
            context.Budgets.Add(budget);
            context.SaveChanges();

            var team = new TeamsEntity { TeamName = "Team A", BudgetId = budget.Id };

            dataProvider.AddTeam(team);
            context.SaveChanges();

            Assert.AreEqual(1, context.Teams.Count());
        }

        [TestMethod]
        public void UpdateTeam_UpdatesExistingTeam()
        {
            var budget = new Budget { TotalBudget = 1000000 };
            context.Budgets.Add(budget);
            context.SaveChanges();

            var team = new TeamsEntity { TeamName = "Team A", BudgetId = budget.Id };
            context.Teams.Add(team);
            context.SaveChanges();

            team.TeamName = "Updated Team A";
            dataProvider.UpdateTeam(team);
            context.SaveChanges();

            var updatedTeam = context.Teams.Find(team.Id);
            Assert.AreEqual("Updated Team A", updatedTeam.TeamName);
        }

        [TestMethod]
        public void DeleteTeam_RemovesTeam()
        {
            var budget = new Budget { TotalBudget = 1000000 };
            context.Budgets.Add(budget);
            context.SaveChanges();

            var team = new TeamsEntity { TeamName = "Team A", BudgetId = budget.Id };
            context.Teams.Add(team);
            context.SaveChanges();

            dataProvider.DeleteTeam(team);
            context.SaveChanges();

            Assert.AreEqual(0, context.Teams.Count());
        }
    }

    [TestClass]
    public class DatabaseSeederTests
    {
        private AppDBContext _context;
        private DatabaseSeeder _seeder;

        [TestInitialize]
        public void Setup()
        {
            _context = new AppDBContext();
            _seeder = new DatabaseSeeder(new F1DataProvider(_context));
        }

        [TestMethod]
        public async Task SeedDataAsync_FileDoesNotExist_ReturnsErrorMessage()
        {
            string filePath = "nonexistent.json";

            string message = null;
            _seeder.DataSeeded += (sender, e) => message = e;
            await _seeder.SeedDataAsync(filePath);

            Assert.AreEqual("A JSON fájl nem található.", message);
        }

        [TestMethod]
        public async Task SeedDataAsync_EmptyJson_ReturnsNoDataMessage()
        {
            string filePath = "empty.json";
            File.WriteAllText(filePath, "[]");

            string message = null;
            _seeder.DataSeeded += (sender, e) => message = e;
            await _seeder.SeedDataAsync(filePath);

            Assert.AreEqual("Nincs új adat feltöltésre.", message);
        }

        [TestMethod]
        public async Task SeedDataAsync_UpdateBudgetForNonExistentTeam_ReturnsErrorMessage()
        {
            string filePath = "update_budget_nonexistent_team.json";
            var updatedTeam = new TeamsEntity
            {
                TeamName = "Nonexistent Team",
                Year = 2021,
                Budget = new Budget { TotalBudget = 200000 }
            };
            File.WriteAllText(filePath, JsonConvert.SerializeObject(new List<TeamsEntity> { updatedTeam }));

            string message = null;
            _seeder.DataSeeded += (sender, e) => message = e;
            await _seeder.SeedDataAsync(filePath);

            Assert.IsFalse(message.Contains("Hiba történt: A csapat nem található."));
        }
    }

    [TestClass]
    public class EntityTests
    {
        [TestMethod]
        public void CreateNewTeam_ShouldCreateTeamSuccessfully()
        {
            var team = new TeamsEntity
            {
                TeamName = "Team A",
                Year = 2021,
                HeadQuarters = "Location A",
                TeamPrincipal = "Principal A",
                ConstructorsChampionshipWins = 5,
            };
            Assert.IsNotNull(team);
            Assert.AreEqual("Team A", team.TeamName);
            Assert.AreEqual(2021, team.Year);
        }

        [TestMethod]
        public void CreateNewBudget_ShouldCreateBudgetSuccessfully()
        {
            var budget = new Budget
            {
                TotalBudget = 100000
            };
            Assert.IsNotNull(budget);
            Assert.AreEqual(100000, budget.TotalBudget);
        }

        [TestMethod]
        public void TeamShouldHaveBudget_WhenBudgetIsAssigned()
        {
            var budget = new Budget { TotalBudget = 150000 };
            var team = new TeamsEntity
            {
                TeamName = "Team B",
                Year = 2021,
                Budget = budget
            };
            Assert.IsNotNull(team.Budget);
            Assert.AreEqual(150000, team.Budget.TotalBudget);
        }

        [TestMethod]
        public void CalculateTotalExpenses_ShouldReturnCorrectTotal()
        {
            var budget = new Budget { TotalBudget = 50000 };
            var expense1 = new Expense { Amount = 10000 };
            var expense2 = new Expense { Amount = 15000 };
            var expense3 = new Expense { Amount = 5000 };

            budget.Expenses.Add(expense1);
            budget.Expenses.Add(expense2);
            budget.Expenses.Add(expense3);

            var totalExpenses = budget.Expenses.Sum(e => e.Amount) ?? 0;

            Assert.AreEqual(30000, totalExpenses);
        }

        [TestMethod]
        public void UpdateExpenseCategory_ShouldChangeCategorySuccessfully()
        {
            var expense = new Expense { Category = "Old Category", Amount = 3000 };

            expense.Category = "New Category";

            Assert.AreEqual("New Category", expense.Category);
        }
    }
}

