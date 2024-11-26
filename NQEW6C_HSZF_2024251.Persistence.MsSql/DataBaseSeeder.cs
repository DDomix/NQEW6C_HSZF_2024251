using Newtonsoft.Json;
using NQEW6C_HSZF_2024251.Model;
using NQEW6C_HSZF_2024251.Persistence.MsSql;

public class DatabaseSeeder
{
    private readonly IF1DataProvider _provider;

    public event EventHandler<string> DataSeeded;

    public DatabaseSeeder(IF1DataProvider provider)
    {
        _provider = provider;
    }

    public async Task SeedDataAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            OnDataSeeded("A JSON fájl nem található.");
            return;
        }

        var jsonData = await File.ReadAllTextAsync(filePath);
        var teams = JsonConvert.DeserializeObject<List<TeamsEntity>>(jsonData);

        if (teams == null || !teams.Any())
        {
            OnDataSeeded("Nincs új adat feltöltésre.");
            return;
        }

        foreach (var team in teams)
        {
            var existingTeam = _provider.GetTeamEntities().FirstOrDefault(x => x.TeamName.Equals(team.TeamName));

            if (existingTeam == null)
            {
                _provider.AddOrUpdateTeam(team);
                OnDataSeeded($"Új csapat hozzáadva: {team.TeamName}");
            }
            else
            {
                _provider.AddOrUpdateTeam(team);
                OnDataSeeded($"Létező csapat frissítve: {team.TeamName}");
            }
        }

        OnDataSeeded("Az adatbázis frissítése sikeresen megtörtént.");
    }

    protected virtual void OnDataSeeded(string message)
    {
        DataSeeded?.Invoke(this, message); 
    }
}
