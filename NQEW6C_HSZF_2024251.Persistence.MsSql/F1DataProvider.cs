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

        //TODO
    }

    public class F1DataProvider : IF1DataProvider
    {
        private readonly AppDBContext context;
        public F1DataProvider(AppDBContext context)
        {
            this.context=context;
        }
        public List<TeamsEntity> GetTeamEntities()
        {
            return context.Teams.ToList();
        }

        public TeamsEntity GetTeamsEntityById(int id)
        {
            return context.Teams.First(x => x.Id == id);
            
        }
    }
}
