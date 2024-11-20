using Microsoft.EntityFrameworkCore;
using NQEW6C_HSZF_2024251.Model;
using System.Collections.Generic;
using System.Linq;

namespace NQEW6C_HSZF_2024251.Persistence.MsSql
{
    public interface IF1DataProvider
    {
        TeamsEntity GetTeamsEntityById(int id);
        List<TeamsEntity> GetTeamEntities();

        void AddTeam(TeamsEntity team);
        void UpdateTeam(TeamsEntity team);
        void DeleteTeam(TeamsEntity team); // Új metódus a csapat törléséhez

        void AddOrUpdateTeam(TeamsEntity team);

        List<Budget> GetBudgetEntities();

        void DeleteExpense(Expense expense);

        void DeleteBudget(Budget budget);
        List<Expense> GetExpeseEntities();
    }

    public class F1DataProvider : IF1DataProvider
    {
        private readonly AppDBContext context;

        public F1DataProvider(AppDBContext context)
        {
            this.context = context;
        }

        public List<TeamsEntity> GetTeamEntities()
        {
            return context.Teams
                .Include(t => t.Budget)
                .ThenInclude(b => b.Expenses).ToList();
                
        }
        public TeamsEntity GetTeamsEntityById(int id)
        {
            return context.Teams
                .Include(t => t.Budget)
                .ThenInclude(b => b.Expenses)
                .FirstOrDefault(x => x.Id == id);
        }

        public void AddOrUpdateTeam(TeamsEntity team)
        {
            var existingTeam = GetTeamEntities()
                .FirstOrDefault(t => t.TeamName == team.TeamName && t.Year == team.Year);

            if (existingTeam == null)
            {
                AddTeam(team); // Új csapat hozzáadása
            }
            else
            {
                // Frissítjük a meglévő csapatot, például a költségeket
                foreach (var expense in team.Budget.Expenses)
                {
                    var existingExpense = existingTeam.Budget.Expenses
                        .FirstOrDefault(e => e.Category == expense.Category && e.ExpenseDate == expense.ExpenseDate);

                    if (existingExpense == null)
                    {
                        existingTeam.Budget.Expenses.Add(expense);
                    }
                }
                UpdateTeam(existingTeam); // Csapat frissítése
            }
        }

        public List<Budget> GetBudgetEntities()
        {
            return context.Budgets
                .Include(t => t.TotalBudget).ToList();

        }

        public List<Expense> GetExpeseEntities()
        {
            return context.Expenses.ToList();

        }

        public void AddTeam(TeamsEntity team)
        {
            context.Teams.Add(team);
            context.SaveChanges();
        }

        public void UpdateTeam(TeamsEntity team)
        {
            context.Teams.Update(team);
            context.SaveChanges();
        }

        public void DeleteTeam(TeamsEntity team)
        {
            context.Teams.Remove(team);
            context.SaveChanges();
        }
        public void DeleteBudget(Budget budget)
        {
            context.Budgets.Remove(budget);
            context.SaveChanges();
        }
        public void DeleteExpense(Expense expense)
        {
            context.Expenses.Remove(expense);
            context.SaveChanges();
        }
    }
}
