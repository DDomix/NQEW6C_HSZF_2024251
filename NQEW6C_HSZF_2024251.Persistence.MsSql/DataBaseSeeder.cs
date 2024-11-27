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

        List<TeamsEntity> teams;
        if (jsonData.TrimStart().StartsWith("["))
        {
            teams = JsonConvert.DeserializeObject<List<TeamsEntity>>(jsonData);
        }
        else
        {
            var singleTeam = JsonConvert.DeserializeObject<TeamsEntity>(jsonData);
            teams = singleTeam != null ? new List<TeamsEntity> { singleTeam } : new List<TeamsEntity>();
        }

        if (teams == null || !teams.Any())
        {
            OnDataSeeded("Nincs új adat feltöltésre.");
            return;
        }

        foreach (var team in teams)
        {
            var existingTeam = _provider.GetTeamEntities().FirstOrDefault(x => x.TeamName.Equals(team.TeamName) && x.Year == team.Year);

            if (existingTeam == null)
            {
                _provider.AddTeam(team);
                OnDataSeeded($"Új csapat hozzáadva: {team.TeamName} ({team.Year})");
            }
            else
            {
                try
                {
                    _provider.MergeTeamData(existingTeam, team);
                    _provider.UpdateTeamFromJson(existingTeam);
                    OnDataSeeded($"Létező csapat frissítve: {team.TeamName} ({team.Year})");
                }
                catch (Exception ex)
                {
                    OnDataSeeded($"Hiba történt: {ex.Message} - Inner Exception: {ex.InnerException?.Message}");
                }

            }
        }
        OnDataSeeded("Az adatbázis frissítése sikeresen megtörtént.");
    }

    protected virtual void OnDataSeeded(string message)
    {
        DataSeeded?.Invoke(this, message); 
    }
}
