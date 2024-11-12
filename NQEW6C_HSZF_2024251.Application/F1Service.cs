using NQEW6C_HSZF_2024251.Model;
using NQEW6C_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NQEW6C_HSZF_2024251.Application
{
    public interface IF1Service
    {
        TeamsEntity GetF1EntityById(int id);

        List<TeamsEntity> GetF1Entities();

        void AddOrUpdateTeam(TeamsEntity team);
    }
    public class F1Service : IF1Service
    {
        private readonly IF1DataProvider dataProvider;

        public F1Service(IF1DataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        public List<TeamsEntity> GetF1Entities()
        {
            return dataProvider.GetTeamEntities();
        }

        public TeamsEntity GetF1EntityById(int id)
        {
            return dataProvider.GetTeamsEntityById(id);
        }

        public void AddOrUpdateTeam(TeamsEntity team)
        {
            var existingTeam = dataProvider.GetTeamEntities()
                .FirstOrDefault(t => t.TeamName == team.TeamName && t.Year == team.Year);

            if (existingTeam == null)
            {
                dataProvider.AddTeam(team);  // új csapat hozzáadása
            }
            else
            {
                // Csak a költségek frissítése, ha már létezik
                foreach (var expense in team.Budget.Expenses)
                {
                    var existingExpense = existingTeam.Budget.Expenses
                        .FirstOrDefault(e => e.Category == expense.Category && e.ExpenseDate == expense.ExpenseDate);

                    if (existingExpense == null)
                    {
                        existingTeam.Budget.Expenses.Add(expense);
                    }
                }
                dataProvider.UpdateTeam(existingTeam);  // frissítjük a meglévő csapatot
            }
        }
    }
}
