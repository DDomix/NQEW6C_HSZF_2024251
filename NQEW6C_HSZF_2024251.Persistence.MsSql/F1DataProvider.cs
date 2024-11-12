using Microsoft.EntityFrameworkCore;
using NQEW6C_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NQEW6C_HSZF_2024251.Persistence.MsSql
{
    public interface IF1DataProvider
    {
        TeamsEntity GetTeamsEntityById(int id);

        List<TeamsEntity> GetTeamEntities();
        
        void AddTeam(TeamsEntity team);      // Új metódus a csapat hozzáadásához
        void UpdateTeam(TeamsEntity team);

        //TODO
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
            return context.Teams.Include(t => t.Budget).ThenInclude(b => b.Expenses).ToList();
        }

        public TeamsEntity GetTeamsEntityById(int id)
        {
            return context.Teams.Include(t => t.Budget).ThenInclude(b => b.Expenses).FirstOrDefault(x => x.Id == id);
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
    }
}
