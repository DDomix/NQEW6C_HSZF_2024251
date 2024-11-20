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
        void AddOrUpdateTeam(TeamsEntity team);
        void DeleteTeam(int id); // Új metódus a csapat törléséhez

        TeamsEntity GetTeamEntityByExactName(string name);

        List<TeamsEntity> GetTeamEntitiesByYear(int year);

        List<TeamsEntity> GetTeamsByHeadquarters(string headquarters);

        List<TeamsEntity> GetTeamsByPrincipal(string principalName);

        List<TeamsEntity> GetTeamsByConstructorTitles(int titleCount);
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

        public void AddOrUpdateTeam(TeamsEntity team)
        {
            var existingTeam = dataProvider.GetTeamEntities()
                .FirstOrDefault(t => t.TeamName == team.TeamName && t.Year == team.Year);

            if (existingTeam == null)
            {
                dataProvider.AddTeam(team); // Új csapat hozzáadása
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
                dataProvider.UpdateTeam(existingTeam); // Csapat frissítése
            }
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
