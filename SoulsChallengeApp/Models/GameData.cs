﻿using Newtonsoft.Json;

namespace DSD_App.Models
{
    public class GameData
    {
        private static Dictionary<string, GameInfo> gamesData = new Dictionary<string, GameInfo>();

        public void LoadGameData(string name, string basePath)
        {
            string gamePath = Path.Combine(basePath, name);
            string bossPath = Path.Combine(gamePath, "Bosses.txt");
            string restrictionsPath = Path.Combine(gamePath, "Restrictions.txt");

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