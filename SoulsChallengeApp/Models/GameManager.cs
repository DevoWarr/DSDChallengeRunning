using Newtonsoft.Json;
using DSD_App.Properties;

namespace DSD_App.Models
{
    public class GameManager
    {
        private static Dictionary<string, List<Boss>> gamesData = new Dictionary<string, List<Boss>>();

        // Constructor
        public GameManager()
        {
            var gameData = Resources.GameData;
            var gamesList = JsonConvert.DeserializeObject<List<Game>>(gameData)!;

            gamesData = gamesList.ToDictionary(
                game => game.GameName,
                game => game.Bosses
            )!;
        }

        // Methods
        public List<Game> LoadGameData()
        {
            var gameData = Resources.GameData;
            return JsonConvert.DeserializeObject<List<Game>>(gameData)!;
        }
        public void SaveGameData(string currentGame, RunType currentRunType, int currentRestriction)
        {
            var gameDataJson = SerializeGameDataToJson();
            Settings.Default.CurrentGame = currentGame;
            Settings.Default.CurrentRunType = currentRunType.ToString();
            Settings.Default.CurrentRestriction = currentRestriction;
            Settings.Default.CompletedBosses = gameDataJson;
        }
        public void UpdateCompletedBosses(string gameName, List<Boss> completedBosses)
        {
            if (gamesData.TryGetValue(gameName, out var gameData))
                gameData.ForEach(b => b.Completed = completedBosses.Any
                (cb => cb.Name == b.Name && cb.Completed));
        }

        // Serialization & Deserialization
        public string SerializeGameDataToJson() => JsonConvert.SerializeObject(gamesData);
        public void DeserializeGameDataFromJson(string jsonData) =>
            gamesData = JsonConvert.DeserializeObject<Dictionary<string, List<Boss>>>(jsonData)!;
    }
}