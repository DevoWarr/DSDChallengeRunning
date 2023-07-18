using SoulsChallengeApp.Models;
using System.Diagnostics;
using System.Security.Policy;
using System.Xml.Linq;

namespace SoulsChallengeApp
{
    public partial class BossForm : Form
    {
        private static string baseFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SoulsChallenge", "SoulsChallenge", "SoulsChallengeApp", "Games");
        private List<Boss> bossList = new List<Boss>();
        private List<Restriction> restrictionList = new List<Restriction>();

        public BossForm()
        {
            InitializeComponent();
            FillAllBosses();
            FillGames();
        }

        // Fills
        private void FillAllBosses()
        {
            string[] folders = Directory.GetDirectories(baseFolderPath);

            foreach (string folder in folders)
            {
                string bossPath = Path.Combine(folder, "Bosses.txt");
                string[] names = File.ReadAllLines(bossPath);

                bossList.AddRange(names.Select(b => new Boss { Name = b, Completed = false }));
            }

            clbBosses.Items.AddRange(bossList.Select(b => b.Name!).ToArray());
        }
        private void FillGames()
        {
            var games = new[]
            {
                "Demon's Souls",
                "Dark Souls I",
                "Dark Souls II",
                "Bloodborne",
                "Dark Souls III",
                "Sekiro",
                "Elden Ring"
            };

            cbxGames.Items.AddRange(games);
            cbxGames.SelectedIndex = 0;
        }

        private Dictionary<string, GameInfo> gamesData = new Dictionary<string, GameInfo>();

        private void cbxGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = cbxGames.Text;

            if (gamesData.TryGetValue(name, out var gameInfo))
            {
                // Game data already loaded, update the UI
                clbBosses.Items.Clear();
                cbxRestrictions.Items.Clear();
                int checkedCount = gameInfo.Bosses.Count(b => b.Completed);

                gameInfo.Bosses.ForEach(b => clbBosses.Items.Add(b.Name!));

                cbxRestrictions.Items.AddRange(gameInfo.Restrictions.Select(r => r.Name!).ToArray());

                lblBosses.Text = $"{checkedCount}/{clbBosses.Items.Count}";
            }
            else
            {
                // Game data not loaded, read the files and populate the data model
                string gameFolderPath = Path.Combine(baseFolderPath, name);
                string bossPath = Path.Combine(gameFolderPath, "Bosses.txt");
                string restrictionsPath = Path.Combine(gameFolderPath, "Restrictions.txt");

                string[] bossNames = File.ReadAllLines(bossPath);
                string[] restrictionNames = File.ReadAllLines(restrictionsPath);

                bossList = bossNames.Select(b => new Boss { Name = b, Completed = false }).ToList();
                restrictionList = restrictionNames.Select(r => new Restriction { Name = r }).ToList();

                GameInfo newGameInfo = new GameInfo
                {
                    Bosses = bossList,
                    Restrictions = restrictionList
                };

                gamesData.Add(name, newGameInfo);

                // Update the UI with the new game data
                cbxGames_SelectedIndexChanged(sender, e);
            }
        }

        private RunType? currentRunType;
        private int bossCount;
        private void clbBosses_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            bossCount = clbBosses.CheckedItems.Count + (e.NewValue == CheckState.Checked ? 1 : -1);
            lblBosses.Text = $"{bossCount}/{clbBosses.Items.Count}";

            CheckRunType();
        }

        private void CheckRunType()
        {
            if (currentRunType != RunType.Casual && bossCount == clbBosses.Items.Count)
                btnSubmit.Enabled = true;
            else
                btnSubmit.Enabled = false;
        }

        private void rbCasual_CheckedChanged(object sender, EventArgs e)
        {
            currentRunType = RunType.Casual;

            cbxRestrictions.Enabled = false;
            cbxRestrictions.SelectedItem = null;

            CheckRunType();
        }

        private void rbChampion_CheckedChanged(object sender, EventArgs e)
        {
            currentRunType = RunType.Champion;

            cbxRestrictions.Enabled = false;
            cbxRestrictions.SelectedItem = null;

            CheckRunType();
        }

        private void rbLegend_CheckedChanged(object sender, EventArgs e)
        {
            currentRunType = RunType.Legend;

            cbxRestrictions.Enabled = true;

            CheckRunType();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string gameName = cbxGames.Text;
            string gameFolderPath = Path.Combine(baseFolderPath, gameName);
            string submissionPath = Path.Combine(gameFolderPath, "Submission.txt");

            string submissionTemplate = File.ReadAllText(submissionPath);

            string category = currentRunType.ToString()!;
            string restriction = FindRunTypes();
            string submissionURL = submissionTemplate
                .Replace("{Category}", category)
                .Replace("{Restriction}", restriction);

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = submissionURL,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private string FindRunTypes()
        {
            var types = new[]
            {
                "NG",           // MAX NG
                "+0",           // +0
                "No Roll",      // NO ROLL
                "Deathless",    // DEATHLESS
                "NoHit"        // NO HIT
            };

            string selectedRestriction = cbxRestrictions.Text;

            string foundType = "Other";
            foreach (string type in types)
            {
                if (selectedRestriction.Contains(type))
                {
                    switch (type)
                    {
                        case "NG":
                            foundType = "Max+Ng";
                            break;
                        case "+0":
                            foundType = "%2B0";
                            break;
                        case "No Roll":
                            foundType = "No+Roll";
                            break;
                        case "Deathless":
                            foundType = "Deathless";
                            break;
                        case "NoHit":
                            foundType = "No+Hit";
                            break;
                    }
                    break;
                }
            }

            return foundType;
        }
    }
}