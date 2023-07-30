using Newtonsoft.Json;
using Octokit;
using Semver;
using DSD_App.Models;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Reflection;
using DSD_App.Properties;

namespace DSD_App
{
    public partial class BossForm : Form
    {
        // Constants
        #region Constants
        private const string RulesURL = @"https://docs.google.com/document/d/1Hffx3O7SavIRUErIeLXMvRQ5yH6Lx1Xs9ZFuPqglvr4/edit";
        private const string DiscordURL = @"https://discord.gg/invite/darksouls3";
        private const string GitHubURL = @"https://github.com/DevoWarr/DSDChallengeRunning";
        #endregion

        // Variables
        #region Variables
        private Version assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version!;
        private GameManager gameManager;
        private string currentGame = Settings.Default.CurrentGame;
        public List<Boss> bossList = new List<Boss>();
        private RunType currentRunType;
        private int bossCount;
        private bool isDarkMode = false;
        #endregion

        // Constructor
        public BossForm()
        {
            InitializeComponent();
            gameManager = new GameManager();

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
            var bossNames = gameManager.LoadGameData()
                .SelectMany(g => g.Bosses!).Select(b => b.Name!).ToArray();

            bossList.AddRange(bossNames.Select(b => new Boss { Name = b, Completed = false }));

            clbBosses.Items.AddRange(bossNames);
        }
        private void FillGames()
        {
            var games = gameManager.LoadGameData()
                .Select(g => g.GameName!).ToArray();

            cbxGames.Items.AddRange(games);

            if (string.IsNullOrEmpty(currentGame))
                cbxGames.SelectedIndex = 0;
            else
                cbxGames.SelectedItem = currentGame;
        }

        // Events
        #region Events
        private void cbxGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentGame = cbxGames.Text;

            var gameData = gameManager.LoadGameData();
            var gameInfo = gameData.Where(g => g.GameName == currentGame).FirstOrDefault();

            clbBosses.Items.Clear();
            cbxRestrictions.Items.Clear();
            int completed = gameInfo!.Bosses!.Count(b => b.Completed);

            gameInfo!.Bosses!.ForEach(b => clbBosses.Items.Add(b.Name!));
            cbxRestrictions.Items.AddRange(gameInfo.Restrictions!.Select(r => r.Name).ToArray());

            lblBosses.Text = $"{completed}/{clbBosses.Items.Count}";

            TickCompletedBosses();
            CheckCompletedRun();
        }
        private void clbBosses_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            bossCount = clbBosses.CheckedItems.Count + (e.NewValue == CheckState.Checked ? 1 : -1);
            lblBosses.Text = $"{bossCount}/{clbBosses.Items.Count}";

            string bossName = clbBosses.Items[e.Index].ToString()!;
            var selectedBoss = bossList.FirstOrDefault(b => b.Name == bossName)!;

            selectedBoss.Completed = e.NewValue == CheckState.Checked;

            var completedBosses = bossList.Where(b => b.Completed).ToList();
            gameManager.UpdateCompletedBosses(currentGame, completedBosses);

            CheckCompletedRun();
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
        #endregion

        // Button Events
        #region Button Events
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

            string submissionTemplate = gameManager.LoadGameData()
                .Where(g => g.GameName == currentGame)
                .Select(g => g.Submission).FirstOrDefault()!;

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
            Settings.Default.UserSelectedMode = isDarkMode;
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
        #endregion

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
                btnMode.Image = new Bitmap(Resources.LightMode, new Size(32, 32));
            }
            else
            {
                SetLightMode();
                btnMode.Image = new Bitmap(Resources.DarkMode, new Size(32, 32));
            }

            btnSubmit.ForeColor = ColorThemes.ForegroundDark;
        }

        // BossForm
        private async void BossForm_Load(object sender, EventArgs e)
        {
            isDarkMode = Settings.Default.UserSelectedMode;
            SetMode(isDarkMode);

            var completedBosses = Settings.Default.CompletedBosses;

            if (!string.IsNullOrEmpty(completedBosses))
                gameManager?.DeserializeGameDataFromJson(completedBosses);

            TickRunType();

            if (currentRunType == RunType.Legend)
                cbxRestrictions.SelectedIndex = Settings.Default.CurrentRestriction;

            // GitHub Updates
            await CheckGitHubUpdates();
        }
        private void BossForm_FormClosing(object sender, FormClosingEventArgs e) =>
            gameManager.SaveGameData(currentGame, currentRunType, cbxRestrictions.SelectedIndex);

        // Methods
        #region Methods
        private void TickCompletedBosses()
        {
            string completedBossesJson = Settings.Default.CompletedBosses;
            if (!string.IsNullOrEmpty(completedBossesJson))
            {
                var completedBosses = JsonConvert.DeserializeObject<Dictionary<string, List<Boss>>>(completedBossesJson);

                if (completedBosses!.TryGetValue(currentGame, out var gameData))
                {
                    var completedBossNames = gameData!.Where(boss => boss.Completed)
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
            string currentRunType = Settings.Default.CurrentRunType;

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

            if (currentGame == "Sekiro")
            {
                restrictions.Add("Base Vit");

                if (restrictions.Count == 2)
                    restrictions.Add("Other");
            }

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
            string assemblySemverString = string.Empty;

            try
            {
                Release release = await gitHubClient.Repository.Release.GetLatest("DevoWarr", "DSDChallengeRunning");
                
                var latestVersion = SemVersion.Parse(release.TagName);
                assemblySemverString = new Version(assemblyVersion.Major, assemblyVersion.Minor, assemblyVersion.Build).ToString();
                var assemblySemver = SemVersion.Parse(assemblySemverString);

                if (latestVersion > assemblySemver)
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

            Text = $"DSD Challenge Running {assemblySemverString}";
        }
        #endregion
    }
}