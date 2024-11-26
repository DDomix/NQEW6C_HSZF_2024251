using Microsoft.EntityFrameworkCore;
using NQEW6C_HSZF_2024251.Model;
using NQEW6C_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NQEW6C_HSZF_2024251.Application
{
    public interface IF1Service
    {
        TeamsEntity GetF1EntityById(int id);
        
        List<TeamsEntity> GetTeamsEntity();
        TeamsEntity GetTeamEntityByName(string name);
        string GetTeamsEntityByBudgetId(Budget budget);
        void DeleteTeam(int id);
        void UpdateTeam(int id);
        void AddOrUpdateTeam(TeamsEntity team);
        void AddOrUpdateBudget(Budget budget);

        TeamsEntity GetTeamEntityByExactName(string name);

        List<TeamsEntity> GetTeamEntitiesByYear(int year);

        List<TeamsEntity> GetTeamsByHeadquarters(string headquarters);

        List<TeamsEntity> GetTeamsByPrincipal(string principalName);

        List<TeamsEntity> GetTeamsByConstructorTitles(int titleCount);

        List<Expense> GetExpensesEntities();

        List<Budget> GetBudgetEntities();

        void UpdateBudget(int id);
    }

    public class F1Service : IF1Service
    {
        private readonly IF1DataProvider dataProvider;

        public F1Service(IF1DataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }
        
        public TeamsEntity GetF1EntityById(int id)
        {
            return dataProvider.GetTeamsEntityById(id);
        }

        public List<TeamsEntity> GetTeamsEntity()
        {
            return dataProvider.GetTeamEntities();
        }

        public string GetTeamsEntityByBudgetId(Budget budget)
        {
            var team = dataProvider.GetTeamEntities()
                .Where(x => x.BudgetId == budget.Id)
                .FirstOrDefault();
            return team?.TeamName;
        }

        public TeamsEntity GetTeamEntityByName(string name)
        {
            return dataProvider.GetTeamEntities().FirstOrDefault(x => x.TeamName.Contains(name));

        }

        public List<TeamsEntity>GetTeamEntitiesByYear(int year)
        {
            return dataProvider.GetTeamEntities().Where(x => x.Year.Equals(year)).ToList();

        }

        public TeamsEntity GetTeamEntityByExactName(string name)
        {
            return dataProvider.GetTeamEntities().FirstOrDefault(x => x.TeamName.Equals(name));

        }
        public List<TeamsEntity> GetTeamsByHeadquarters(string headquarters)
        {
            return dataProvider.GetTeamEntities()
                .Where(t => t.HeadQuarters.Contains(headquarters, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public void AddOrUpdateTeam(TeamsEntity team)
        {
            dataProvider.AddOrUpdateTeam(team);
        }
        public void AddOrUpdateBudget(Budget budget)
        {
            dataProvider.AddOrUpdateBudget(budget);
        }

        public List<TeamsEntity> GetTeamsByPrincipal(string principalName)
        {
            return dataProvider.GetTeamEntities()
                .Where(t => t.TeamPrincipal.Contains(principalName, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<TeamsEntity> GetTeamsByConstructorTitles(int titleCount)
        {
            return dataProvider.GetTeamEntities()
                .Where(t => t.ConstructorsChampionshipWins == titleCount)
                .ToList();
        }

        public List<Budget> GetBudgetEntities()
        {
            return dataProvider.GetBudgetEntities();
        }

        public void UpdateBudget(int id)
        {
            var bud = dataProvider.GetBudgetEntityById(id);
            if (bud!=null)
            {
                dataProvider.UpdateBudget(bud);
            }
        }
        public List<Expense> GetExpensesEntities()
        {
            return dataProvider.GetExpeseEntities();
        }
        public void DeleteTeam(int id)
        {
            var team = dataProvider.GetTeamsEntityById(id);
            if (team != null)
            {
                dataProvider.DeleteTeam(team);
            }
        }
        public void UpdateTeam(int id)
        {
            var team = dataProvider.GetTeamsEntityById(id);
            if (team != null)
            {
                dataProvider.UpdateTeam(team);
            }
        }
    }
}