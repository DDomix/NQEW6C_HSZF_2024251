﻿using Microsoft.EntityFrameworkCore;
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
        void DeleteTeam(TeamsEntity team);

        void UpdateTeamFromJson(TeamsEntity team);
        void AddOrUpdateBudget(Budget budget);

        List<Budget> GetBudgetEntities();

        void DeleteExpense(Expense expense);

        void DeleteBudget(Budget budget);

        void DeleteSubCategory(SubCategory sub);
        List<Expense> GetExpeseEntities();
        List<TeamsEntity> TeamDataForRiport(string teamName);

        void MergeTeamData(TeamsEntity existingTeam, TeamsEntity newTeam);
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

        public List<TeamsEntity> TeamDataForRiport(string teamName)
        {
            return context.Teams
            .Where(t => t.TeamName.Contains(teamName))
            .OrderByDescending(t => t.Year)
            .Take(2)
            .ToList();
        }

        public void UpdateTeamFromJson(TeamsEntity team)
        {
            var existingTeam = context.Teams
                               .Include(t => t.Budget)
                               .ThenInclude(b => b.Expenses)
                               .ThenInclude(e => e.SubCategory)
                               .FirstOrDefault(t => t.TeamName == team.TeamName && t.Year == team.Year);

            foreach (var expense in team.Budget.Expenses)
            {
                var existingExpense = existingTeam.Budget.Expenses
                    .FirstOrDefault(e => e.Category == expense.Category && e.ExpenseDate == expense.ExpenseDate);

                if (existingExpense == null)
                {
                    existingTeam.Budget.Expenses.Add(expense);
                    AddExpense(expense);

                    foreach (var subCategory in expense.SubCategory)
                    {
                        subCategory.Expense = expense;
                        AddSubCategory(subCategory);
                    }
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
                            subCategory.Expense = existingExpense;
                            AddSubCategory(subCategory);
                        }
                        else
                        {
                            existingSubCategory.Amount = subCategory.Amount;
                            UpdateSubCategory(existingSubCategory);
                        }

                        existingExpense.Amount += subCategory.Amount;
                    }

                    existingExpense.Amount = existingExpense.SubCategory.Sum(sc => sc.Amount);
                    UpdateExpense(existingExpense);
                }
            }

            UpdateTeam(existingTeam);
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

        public void MergeTeamData(TeamsEntity existingTeam, TeamsEntity newTeam)
        {
            if (newTeam.Budget?.Expenses != null)
            {
                foreach (var newExpense in newTeam.Budget.Expenses)
                {
                    var existingExpense = existingTeam.Budget.Expenses
                        .FirstOrDefault(e => e.Category == newExpense.Category);

                    if (existingExpense == null)
                    {
                        newExpense.BudgetId = existingTeam.Budget.Id;

                        if (string.IsNullOrEmpty(newExpense.ApprovalStatus))
                        {
                            newExpense.ApprovalStatus = "Pending";
                        }

                        if (newExpense.SubCategory != null)
                        {
                            newExpense.Amount = newExpense.SubCategory.Sum(sc => sc.Amount);
                        }

                        existingTeam.Budget.Expenses.Add(newExpense);
                    }
                    else
                    {
                        if (newExpense.SubCategory != null)
                        {
                            foreach (var newSubCategory in newExpense.SubCategory)
                            {
                                var existingSubCategory = existingExpense.SubCategory
                                    .FirstOrDefault(sc => sc.Name == newSubCategory.Name);

                                if (existingSubCategory == null)
                                {
                                    existingExpense.SubCategory.Add(newSubCategory);
                                    newSubCategory.ExpenseId = existingExpense.Id;
                                    existingExpense.Amount += newSubCategory.Amount;
                                }
                                else
                                {
                                    existingSubCategory.Amount = newSubCategory.Amount;
                                    UpdateSubCategory(existingSubCategory);
                                }
                            }
                        }
                        existingExpense.Amount = existingExpense.SubCategory.Sum(sc => sc.Amount);
                        UpdateExpense(existingExpense);
                    }
                }
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
