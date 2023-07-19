using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoulsChallengeApp.Models
{
    public class GameData
    {
        private Dictionary<string, GameInfo> gamesData = new Dictionary<string, GameInfo>();

        public void LoadGameData(string name, string baseFolderPath)
        {
            string gameFolderPath = Path.Combine(baseFolderPath, name);
            string bossPath = Path.Combine(gameFolderPath, "Bosses.txt");
            string restrictionsPath = Path.Combine(gameFolderPath, "Restrictions.txt");

            string[] bossNames = File.ReadAllLines(bossPath);
            string[] restrictionNames = File.ReadAllLines(restrictionsPath);

            var bossList = bossNames.Select(b => new Boss { Name = b, Completed = false }).ToList();
            var restrictionList = restrictionNames.Select(r => new Restriction { Name = r }).ToList();

            if (gamesData.ContainsKey(name))
            {
                gamesData[name].Bosses = bossList;
                gamesData[name].Restrictions = restrictionList;
            }
            else
            {
                GameInfo newGameInfo = new GameInfo
                {
                    Bosses = bossList,
                    Restrictions = restrictionList
                };
                gamesData.Add(name, newGameInfo);
            }
        }


        public GameInfo GetGameInfo(string name) => gamesData[name];
        public void UpdateCompletedBosses(string gameName, List<Boss> completedBosses)
        {
            if (gamesData.TryGetValue(gameName, out var gameInfo))
                gameInfo.Bosses!.ForEach(b => b.Completed = completedBosses.Any
                (cb => cb.Name == b.Name && cb.Completed));
        }

        // Serialization & Deserialization
        public string SerializeGameDataToJson() => JsonConvert.SerializeObject(gamesData);

        public void DeserializeGameDataFromJson(string jsonData) =>
            gamesData = JsonConvert.DeserializeObject<Dictionary<string, GameInfo>>(jsonData)!;
    }
}