using SoulsChallengeApp.Models;
using System.Diagnostics;

namespace SoulsChallengeApp
{
    public partial class BossForm : Form
    {
        // Variables
        private static string baseFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Games");
        private GameData gameData;
        private List<Boss> bossList = new List<Boss>();
        private RunType? currentRunType;
        private int bossCount;
        private bool isDarkMode = false;

        public BossForm()
        {
            InitializeComponent();
            Text = "Souls Challenger";
            gameData = new GameData();

            InitializeRunTypes();
            FillAllBosses();
            FillGames();
        }

        // Initialization
        private void InitializeRunTypes()
        {
            rbCasual.Tag = RunType.Casual;
            rbChampion.Tag = RunType.Champion;
            rbLegend.Tag = RunType.Legend;
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

        // Runtypes
        private string FindRunTypes()
        {
            var types = new[]
            {
                "NG",           // MAX NG
                "+0",           // +0
                "No Roll",      // NO ROLL
                "Deathless",    // DEATHLESS
                "NoHit"         // NO HIT
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
        private void CheckRunType()
        {
            bool completedRun = bossCount == clbBosses.Items.Count;

            if (currentRunType != RunType.Casual && completedRun)
                btnSubmit.Enabled = true;
            else
                btnSubmit.Enabled = false;

            if (completedRun)
            {
                lblCompletion.Text = "COMPLETED";
                lblCompletion.ForeColor = Color.Green;
            }
            else
            {
                lblCompletion.Text = "UNCOMPLETED";
                lblCompletion.ForeColor = Color.Red;
            }
        }

        // Events
        private void cbxGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = cbxGames.Text;

            gameData.LoadGameData(name, baseFolderPath);

            var gameInfo = gameData.GetGameInfo(name);

            clbBosses.Items.Clear();
            cbxRestrictions.Items.Clear();
            int checkedCount = gameInfo.Bosses.Count(b => b.Completed);

            gameInfo.Bosses.ForEach(b => clbBosses.Items.Add(b.Name!));
            cbxRestrictions.Items.AddRange(gameInfo.Restrictions.Select(r => r.Name).ToArray());

            lblBosses.Text = $"{checkedCount}/{clbBosses.Items.Count}";
        }
        private void clbBosses_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            bossCount = clbBosses.CheckedItems.Count + (e.NewValue == CheckState.Checked ? 1 : -1);
            lblBosses.Text = $"{bossCount}/{clbBosses.Items.Count}";

            CheckRunType();
        }
        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                currentRunType = (RunType)radioButton.Tag;

                if (currentRunType == RunType.Legend)
                {
                    cbxRestrictions.Enabled = true;
                }
                else
                {
                    cbxRestrictions.Enabled = false;
                    cbxRestrictions.SelectedItem = null;
                }

                CheckRunType();
            }
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
        private void btnReset_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbBosses.Items.Count; i++)
                clbBosses.SetItemChecked(i, false);
        }
        private void btnRules_Click(object sender, EventArgs e)
        {
            string rulesURL = @"https://docs.google.com/document/d/1Hffx3O7SavIRUErIeLXMvRQ5yH6Lx1Xs9ZFuPqglvr4/edit";
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = rulesURL,
                UseShellExecute = true
            };

            Process.Start(psi);
        }

        private void btnMode_Click(object sender, EventArgs e)
        {
            isDarkMode = !isDarkMode;

            if (isDarkMode)
            {
                SetDarkMode();
                btnMode.Text = "Light Mode";
            }
            else
            {
                SetLightMode();
                btnMode.Text = "Dark Mode";
            }
        }

        // UI
        private void SetLightMode()
        {
            BackColor = ColorThemes.BackgroundLight;
            ForeColor = ColorThemes.ForegroundLight;
            clbBosses.BackColor = ColorThemes.BossesLight;
            clbBosses.ForeColor = ColorThemes.ForegroundLight;
        }
        private void SetDarkMode()
        {
            BackColor = ColorThemes.BackgroundDark;
            ForeColor = ColorThemes.ForegroundDark;
            clbBosses.BackColor = ColorThemes.BossesDark;
            clbBosses.ForeColor = ColorThemes.ForegroundDark;

            foreach (Control control in Controls)
            {
                if (control is Button button)
                {
                    button.ForeColor = ColorThemes.ForegroundLight;
                }
            }
        }
    }
}