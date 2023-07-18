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

            GameInfo newGameInfo = new GameInfo
            {
                Bosses = bossList,
                Restrictions = restrictionList
            };

            gamesData.Add(name, newGameInfo);
        }

        public GameInfo GetGameInfo(string name)
        {
            return gamesData[name];
        }
    }
}