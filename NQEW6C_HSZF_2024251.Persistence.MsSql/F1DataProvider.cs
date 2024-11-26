using Microsoft.EntityFrameworkCore;
using NQEW6C_HSZF_2024251.Model;
using System.Collections.Generic;
using System.Linq;

namespace NQEW6C_HSZF_2024251.Persistence.MsSql
{
    public interface IF1DataProvider
    {
        TeamsEntity GetTeamsEntityById(int id);
        Budget GetBudgetEntityById(int id);
        List<TeamsEntity> GetTeamEntities();

        void AddTeam(TeamsEntity team);

        void AddBudget(Budget budget);
        void AddExpense(Expense expense);
        void AddSubCategory(SubCategory subCategory);


        void UpdateTeam(TeamsEntity team);
        void UpdateBudget(Budget budget);
        void UpdateExpense(Expense expense);
        void UpdateSubCategory(SubCategory subCategory);
        void DeleteTeam(TeamsEntity team); // Új metódus a csapat törléséhez

        void AddOrUpdateTeam(TeamsEntity team);
        void AddOrUpdateBudget(Budget budget);

        List<Budget> GetBudgetEntities();

        void DeleteExpense(Expense expense);

        void DeleteBudget(Budget budget);

        void DeleteSubCategory(SubCategory sub);
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

        public Budget GetBudgetEntityById(int id)
        {
            return context.Budgets.FirstOrDefault(x => x.Id == id);
        }

        public void AddOrUpdateTeam(TeamsEntity team)
        {
            var existingTeam = context.Teams
                .Include(t => t.Budget)
                .ThenInclude(b => b.Expenses)
                .ThenInclude(e => e.SubCategory)
                .FirstOrDefault(t => t.TeamName == team.TeamName && t.Year == team.Year);

            if (existingTeam == null)
            {
                AddTeam(team);
            }
            else
            {
                foreach (var expense in team.Budget.Expenses)
                {
                    var existingExpense = existingTeam.Budget.Expenses
                        .FirstOrDefault(e => e.Category == expense.Category && e.ExpenseDate == expense.ExpenseDate);

                    if (existingExpense == null)
                    {
                        existingTeam.Budget.Expenses.Add(expense);
                        context.Entry(expense).State = EntityState.Added;
                    }
                    else
                    {
                        foreach (var subCategory in expense.SubCategory)
                        {
                            var existingSubCategory = existingExpense.SubCategory
                                .FirstOrDefault(sc => sc.Name == subCategory.Name);

                            if (existingSubCategory == null)
                            {
                                existingExpense.SubCategory.Add(subCategory);
                                context.Entry(subCategory).State = EntityState.Added;
                            }
                            else
                            {
                                existingSubCategory.Amount = subCategory.Amount;
                            }
                        }
                    }
                }
                UpdateTeam(existingTeam);
            }
        }

        public void AddOrUpdateBudget(Budget budget)
        {
            if (budget == null)
                throw new ArgumentNullException(nameof(budget));

            var existingBudget = context.Budgets.FirstOrDefault(b => b.Id == budget.Id);

            if (existingBudget == null)
            {
                AddBudget(budget);
            }
            else
            {
                existingBudget.TotalBudget = budget.TotalBudget;

                UpdateBudget(existingBudget);
            }
        }


        public List<Budget> GetBudgetEntities()
        {
            return context.Budgets.ToList();
        }

        public List<Expense> GetExpeseEntities()
        {
            return context.Expense.ToList();
        }

        public void AddTeam(TeamsEntity team)
        {
            context.Teams.Add(team);
            context.SaveChanges();
        }

        public void AddBudget(Budget budget)
        {
            context.Budgets.Add(budget);
            context.SaveChanges();
        }

        public void AddExpense(Expense expense)
        {
            context.Expense.Add(expense);
            context.SaveChanges();
        }

        public void AddSubCategory(SubCategory subCategory)
        {
            context.SubCategory.Add(subCategory);
            context.SaveChanges();
        }

        public void UpdateTeam(TeamsEntity team)
        {
            context.Teams.Update(team);
            context.SaveChanges();
        }
        public void UpdateBudget(Budget budget)
        {
            context.Budgets.Update(budget);
            context.SaveChanges();
        }
        public void UpdateExpense(Expense expense)
        {
            context.Expense.Update(expense);
            context.SaveChanges();
        }
        public void UpdateSubCategory(SubCategory subCategory)
        {
            context.SubCategory.Update(subCategory);
            context.SaveChanges();
        }

        public void DeleteTeam(TeamsEntity team)
        {
            context.Entry(team).Reload();
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
            context.Expense.Remove(expense);
            context.SaveChanges();
        }
        public void DeleteSubCategory(SubCategory sub)
        {
            context.SubCategory.Remove(sub);
            context.SaveChanges();
        }
    }
}
