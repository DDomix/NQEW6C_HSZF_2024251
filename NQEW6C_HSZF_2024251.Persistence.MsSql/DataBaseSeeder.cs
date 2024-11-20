using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NQEW6C_HSZF_2024251.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NQEW6C_HSZF_2024251.Persistence.MsSql
{
    public class DatabaseSeeder
    {
        private readonly IF1DataProvider _provider;

        public DatabaseSeeder(IF1DataProvider provider)
        {
            _provider = provider;
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
                // Ellenőrizzük, hogy a csapat már létezik-e
                var existingTeam = _provider.GetTeamEntities().FirstOrDefault(x => x.TeamName.Equals((team.TeamName)));

                if (existingTeam == null)
                {
                    // Új csapat hozzáadása
                    _provider.AddOrUpdateTeam(team);
                    Console.WriteLine($"Új csapat hozzáadva: {team.TeamName}");
                }
                else
                {
                    // Csapat frissítése
                    _provider.AddOrUpdateTeam(team);
                    Console.WriteLine($"Létező csapat frissítve: {team.TeamName}");
                }
            }

            Console.WriteLine("Az adatbázis frissítése sikeresen megtörtént.");
        }
    }
}
