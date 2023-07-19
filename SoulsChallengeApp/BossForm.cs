using Newtonsoft.Json;
using SoulsChallengeApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SoulsChallengeApp
{
    public partial class BossForm : Form
    {
        // Constants
        private const string SubmissionTemplate = "{Category}/{Restriction}";
        private const string RulesURL = @"https://docs.google.com/document/d/1Hffx3O7SavIRUErIeLXMvRQ5yH6Lx1Xs9ZFuPqglvr4/edit";

        // Variables
        private static string baseFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Games");
        private GameData gameData;
        private string currentGame;
        public List<Boss> bossList = new List<Boss>();
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
            var types = new Dictionary<string, string>
            {
                { "NG", "Max+Ng" },             // MAX NG
                { "+0", "%2B0" },               // +0 WEAPON
                { "No Roll", "No+Roll" },       // NO ROLL
                { "Deathless", "Deathless" },   // DEATHLESS
                { "NoHit", "No+Hit" }           // NO HIT
            };

            string selectedRestriction = cbxRestrictions.Text;

            foreach (var type in types)
            {
                if (selectedRestriction.Contains(type.Key))
                {
                    return type.Value;
                }
            }

            return "Other";
        }
        private void CheckRunType()
        {
            bool completedRun = bossCount == clbBosses.Items.Count;

            btnSubmit.Enabled = currentRunType != RunType.Casual && completedRun;

            lblCompletion.Text = completedRun ? "COMPLETED" : "UNCOMPLETED";
            lblCompletion.ForeColor = completedRun ? Color.Green : Color.Red;
        }

        // Events
        private async void cbxGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentGame = cbxGames.Text;

            await LoadGameDataAsync(currentGame, baseFolderPath);
            var gameInfo = gameData.GetGameInfo(currentGame);

            clbBosses.Items.Clear();
            cbxRestrictions.Items.Clear();
            int completed = gameInfo.Bosses!.Count(b => b.Completed);

            gameInfo.Bosses!.ForEach(b => clbBosses.Items.Add(b.Name!));
            cbxRestrictions.Items.AddRange(gameInfo.Restrictions!.Select(r => r.Name).ToArray());

            lblBosses.Text = $"{completed}/{clbBosses.Items.Count}";

            CheckCompletedBosses();
        }

        private async Task LoadGameDataAsync(string gameName, string baseFolderPath) =>
            await Task.Run(() => gameData.LoadGameData(gameName, baseFolderPath));

        private void clbBosses_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            bossCount = clbBosses.CheckedItems.Count + (e.NewValue == CheckState.Checked ? 1 : -1);
            lblBosses.Text = $"{bossCount}/{clbBosses.Items.Count}";

            string bossName = clbBosses.Items[e.Index].ToString()!;
            var selectedBoss = bossList.FirstOrDefault(b => b.Name == bossName)!;

            selectedBoss.Completed = e.NewValue == CheckState.Checked;

            var completedBosses = bossList.Where(b => b.Completed).ToList();
            gameData.UpdateCompletedBosses(currentGame, completedBosses);

            CheckRunType();
        }

        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                currentRunType = (RunType)radioButton.Tag;

                cbxRestrictions.Enabled = currentRunType == RunType.Legend;
                cbxRestrictions.SelectedItem = null;

                CheckRunType();
            }
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            string submissionURL = SubmissionTemplate
                .Replace("{Category}", currentRunType.ToString())
                .Replace("{Restriction}", FindRunTypes());

            await OpenSubmissionURLAsync(submissionURL);
        }

        private async Task OpenSubmissionURLAsync(string url)
        {
            await Task.Run(() =>
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };

                using (Process.Start(psi)) { }
            });
        }


        private void btnReset_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbBosses.Items.Count; i++)
                clbBosses.SetItemChecked(i, false);

            bossCount = 0;
            lblBosses.Text = $"{bossCount}/{clbBosses.Items.Count}";

            CheckRunType();
        }

        private void btnRules_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = RulesURL,
                UseShellExecute = true
            };

            using (Process.Start(psi)) { }
        }

        private void btnMode_Click(object sender, EventArgs e)
        {
            isDarkMode = !isDarkMode;
            Properties.Settings.Default.UserSelectedMode = isDarkMode;
            SetMode(isDarkMode);
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

        private void SetMode(bool isDarkMode)
        {
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
        private void BossForm_Load(object sender, EventArgs e)
        {
            isDarkMode = Properties.Settings.Default.UserSelectedMode;
            SetMode(isDarkMode);

            string gameDataJson = Properties.Settings.Default.CompletedBosses;
            gameData.DeserializeGameDataFromJson(gameDataJson);
        }

        private void BossForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var gameDataJson = gameData.SerializeGameDataToJson();
            Properties.Settings.Default.CompletedBosses = gameDataJson;
        }

        private void CheckCompletedBosses()
        {
            string completedBossesJson = Properties.Settings.Default.CompletedBosses;
            if (!string.IsNullOrEmpty(completedBossesJson))
            {
                var completedBosses = JsonConvert.DeserializeObject<Dictionary<string, GameInfo>>(completedBossesJson);

                if (completedBosses!.TryGetValue(currentGame, out var gameData))
                {
                    var completedBossNames = gameData.Bosses!.Where(boss => boss.Completed)
                        .Select(boss => boss.Name).ToList();

                    for (int i = 0; i < clbBosses.Items.Count; i++)
                    {
                        string bossName = clbBosses.Items[i].ToString()!;
                        clbBosses.SetItemChecked(i, completedBossNames.Contains(bossName));
                    }
                }
            }
        }
    }
}