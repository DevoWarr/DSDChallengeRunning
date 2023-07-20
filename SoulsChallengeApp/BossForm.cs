using Newtonsoft.Json;
using Octokit;
using Semver;
using SoulsChallengeApp.Models;
using System.Diagnostics;
using System.Text;
using System.Web;

namespace SoulsChallengeApp
{
    public partial class BossForm : Form
    {
        // Constants
        private const string RulesURL = @"https://docs.google.com/document/d/1Hffx3O7SavIRUErIeLXMvRQ5yH6Lx1Xs9ZFuPqglvr4/edit";
        private const string DiscordURL = @"https://discord.gg/invite/darksouls3";
        private const string GitHubURL = @"https://github.com/DevoWarr/DSDChallengeRunning";

        // Variables
        private static string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Games");
        private GameData gameData;
        private string currentGame = Properties.Settings.Default.CurrentGame;
        public List<Boss> bossList = new List<Boss>();
        private RunType? currentRunType;
        private int bossCount;
        private bool isDarkMode = false;

        public BossForm()
        {
            InitializeComponent();
            Text = $"DSD Challenge Running {System.Windows.Forms.Application.ProductVersion}";
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
            string[] folders = Directory.GetDirectories(basePath);

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

            if (string.IsNullOrEmpty(currentGame))
                cbxGames.SelectedIndex = 0;
            else
                cbxGames.SelectedItem = currentGame;
        }

        // Events
        private async void cbxGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentGame = cbxGames.Text;

            await LoadGameDataAsync(currentGame, basePath);
            var gameInfo = gameData.GetGameInfo(currentGame);

            clbBosses.Items.Clear();
            cbxRestrictions.Items.Clear();
            int completed = gameInfo.Bosses!.Count(b => b.Completed);

            gameInfo.Bosses!.ForEach(b => clbBosses.Items.Add(b.Name!));
            cbxRestrictions.Items.AddRange(gameInfo.Restrictions!.Select(r => r.Name).ToArray());

            lblBosses.Text = $"{completed}/{clbBosses.Items.Count}";

            TickCompletedBosses();
            CheckCompletedRun();

            SaveGameData();
        }
        private void clbBosses_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            bossCount = clbBosses.CheckedItems.Count + (e.NewValue == CheckState.Checked ? 1 : -1);
            lblBosses.Text = $"{bossCount}/{clbBosses.Items.Count}";

            string bossName = clbBosses.Items[e.Index].ToString()!;
            var selectedBoss = bossList.FirstOrDefault(b => b.Name == bossName)!;

            selectedBoss.Completed = e.NewValue == CheckState.Checked;

            var completedBosses = bossList.Where(b => b.Completed).ToList();
            gameData.UpdateCompletedBosses(currentGame, completedBosses);

            CheckCompletedRun();
            SaveGameData();
        }
        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                currentRunType = (RunType)radioButton.Tag;

                cbxRestrictions.Enabled = currentRunType == RunType.Legend;
                cbxRestrictions.SelectedItem = null;

                CheckCompletedRun();
            }
        }

        // Button Events
        private void btnReset_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show($"Would you like to reset progress for {currentGame}?", $"Reset {currentGame} Bosslist",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                for (int i = 0; i < clbBosses.Items.Count; i++)
                    clbBosses.SetItemChecked(i, false);

                bossCount = 0;
                lblBosses.Text = $"{bossCount}/{clbBosses.Items.Count}";

                CheckCompletedRun();
            }
        }
        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            // Checks
            if (currentRunType == null)
            {
                MessageBox.Show("Please select a Run Type", "Run Type Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (currentRunType == RunType.Legend && cbxRestrictions.SelectedItem == null)
            {
                MessageBox.Show("Please select a restriction for your Legend Run!", "Restriction Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string entryRadio = "entry.1804428826=";
            string entryCheck = "&entry.805188992=";
            string entryOther = "&entry.1655991278=";

            string gamePath = Path.Combine(basePath, currentGame, "Submission.txt");
            string submissionTemplate = File.ReadAllText(gamePath);

            string category = $"{entryRadio}{currentRunType}";
            string submissionURL;
            string restrictionsParameters = string.Empty;
            string restriction = cbxRestrictions.SelectedItem?.ToString()!;
            string challengeRun = currentRunType == RunType.Champion ? $"{currentGame} - {(currentGame == "Sekiro" ? "Base Vit" : "Champion")}" : restriction;

            if (currentRunType == RunType.Champion)
            {
                string champTypeEntry = $"{entryCheck}{(currentGame == "Sekiro" ? "Base Vit" : "Champion")}";
                submissionURL = submissionTemplate.Replace("{Category}", category)
                                                 .Replace("{Restrictions}", champTypeEntry);
            }
            else // Legend Run
            {
                var restrictions = FindRunTypes();
                restrictionsParameters = string.Join("&", restrictions.Select(r => $"{entryCheck}{r}"));

                string otherRestriction = HttpUtility.UrlEncode(restriction);
                submissionURL = submissionTemplate.Replace("{Category}", category)
                                                 .Replace("{Restrictions}", restrictionsParameters + entryOther + otherRestriction);
            }

            DialogResult result = MessageBox.Show($"Congratulations on completing your run for {currentGame}: \n{challengeRun}\n" +
                $"You will be redirected to the Google Submission Form.\n" +
                "Would you like to continue?", "Submission Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes) await OpenURLAsync(submissionURL);
        }
        private async void btnRules_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Go to DSD Challenge Run Rules Document?",
                "Challenge Run Rules", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes) await OpenURLAsync(RulesURL);
        }
        private void btnMode_Click(object sender, EventArgs e)
        {
            isDarkMode = !isDarkMode;
            Properties.Settings.Default.UserSelectedMode = isDarkMode;
            SetMode(isDarkMode);
        }
        private void btnInfo_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Easy tool for tracking bosses in FromSoftware games.");
            sb.AppendLine("Submit your challenge runs to the DSD Server to obtain roles!");
            sb.AppendLine("Discord: @devowarr or DSD Discord Server");

            MessageBox.Show(sb.ToString(), "Information");
        }
        private async void btnDiscord_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Would you like to join the DSD Discord Server?", "DSD Discord",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes) await OpenURLAsync(DiscordURL);
        }
        private async void btnGithub_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("View GitHub repository?", "DSDChallengeRunning Repository",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes) await OpenURLAsync(GitHubURL);
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

            btnSubmit.ForeColor = ColorThemes.ForegroundDark;
        }
        private void SetMode(bool isDarkMode)
        {
            if (isDarkMode)
            {
                SetDarkMode();
                btnMode.Image = new Bitmap(Properties.Resources.LightMode, new Size(32, 32));
            }
            else
            {
                SetLightMode();
                btnMode.Image = new Bitmap(Properties.Resources.DarkMode, new Size(32, 32));
            }

            btnSubmit.ForeColor = ColorThemes.ForegroundDark;
        }

        // BossForm
        private async void BossForm_Load(object sender, EventArgs e)
        {
            isDarkMode = Properties.Settings.Default.UserSelectedMode;
            SetMode(isDarkMode);

            if (!string.IsNullOrEmpty(Properties.Settings.Default.CompletedBosses))
                gameData?.DeserializeGameDataFromJson(Properties.Settings.Default.CompletedBosses);

            TickRunType();

            // GitHub Updates
            await CheckGitHubUpdates();
        }
        private void BossForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveGameData();
        }

        // Methods
        private void TickCompletedBosses()
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
        private void TickRunType()
        {
            string currentRunType = Properties.Settings.Default.CurrentRunType;

            if (Enum.TryParse(currentRunType, out RunType selectedRunType))
            {
                switch (selectedRunType)
                {
                    case RunType.Casual:
                        rbCasual.Checked = true;
                        break;
                    case RunType.Champion:
                        rbChampion.Checked = true;
                        break;
                    case RunType.Legend:
                        rbLegend.Checked = true;
                        break;
                    default:
                        rbCasual.Checked = true;
                        break;
                }
            }

        }
        private List<string> FindRunTypes()
        {
            var restrictions = new List<string>();

            var types = new List<string>
            {
                "NG", "CoC", "Broken Thief Sword", "Broken Straight Sword", "+0",
                "No Roll", "Deathless", "No Hit", "NG+ 7 No Aux", "No Roll/Block/Parry", "+0 Weapons No Aux",
                "No Deflect", "No Items", "Sword Only", "Base Att", "Hardmode"
            };

            string selectedRestriction = cbxRestrictions.Text;

            if (selectedRestriction.Contains(types[0]))
                restrictions.Add("Max Ng");

            if (selectedRestriction.Contains(types[1]))
                restrictions.Add("Company of Champion");

            restrictions.AddRange(types
                .Where(type => selectedRestriction.Contains(type))
                .Select(type => HttpUtility.UrlEncode(type))
                .ToList());

            if (restrictions.Count == 0 || restrictions.Contains(HttpUtility.UrlEncode(types[3]))) // None or DS1 BSS
                restrictions.Add("Other");

            if (selectedRestriction.Contains("CoC") && restrictions.Count == 2) restrictions.Add("Other");

            if (selectedRestriction == "SL1 NG+ Broken Weapons No Auxiliary (Bleed/Toxic/Poison/Frost)")
                restrictions.Remove("Max Ng");
                
            return restrictions;
        }
        private void CheckCompletedRun()
        {
            bool completedRun = bossCount == clbBosses.Items.Count;

            btnSubmit.Enabled = currentRunType != RunType.Casual && completedRun;

            lblCompletion.Text = completedRun ? "COMPLETED" : "UNCOMPLETED";
            lblCompletion.ForeColor = completedRun ? Color.Green : Color.Red;

            if (currentRunType != RunType.Casual)
                btnSubmit.BackColor = completedRun ? Color.Green : Color.Red;
            else
                btnSubmit.BackColor = Color.Red;
        }
        private async Task OpenURLAsync(string url)
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
        private async Task CheckGitHubUpdates()
        {
            GitHubClient gitHubClient = new GitHubClient(new ProductHeaderValue("DSDChallengeRunning"));
            try
            {
                Release release = await gitHubClient.Repository.Release.GetLatest("DevoWarr", "DSDChallengeRunning");
                var latestVersion = SemVersion.Parse(release.TagName);

                if (latestVersion > System.Windows.Forms.Application.ProductVersion)
                {
                    DialogResult result = MessageBox.Show("New Update Available\n" +
                        "Would you like to update to the latest version?",
                        "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes) await OpenURLAsync(release.HtmlUrl);
                }
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is ApiException || ex is ArgumentException)
            {
                MessageBox.Show("App Version Unknown!", "Unknown App Version");
            }
        }
        private void SaveGameData()
        {
            var gameDataJson = gameData.SerializeGameDataToJson();
            Properties.Settings.Default.CurrentGame = currentGame;
            Properties.Settings.Default.CurrentRunType = currentRunType.ToString();
            Properties.Settings.Default.CompletedBosses = gameDataJson;
        }
        private async Task LoadGameDataAsync(string gameName, string basePath) =>
            await Task.Run(() => gameData.LoadGameData(gameName, basePath));
    }
}