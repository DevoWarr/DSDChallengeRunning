using Newtonsoft.Json;
using Octokit;
using Semver;
using SoulsChallengeApp.Models;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using System.Web;

namespace SoulsChallengeApp
{
    public partial class BossForm : Form
    {
        // Constants
        private const string RulesURL = @"https://docs.google.com/document/d/1Hffx3O7SavIRUErIeLXMvRQ5yH6Lx1Xs9ZFuPqglvr4/edit";

        // Variables
        private static string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Games");
        private GameData gameData;
        private string currentGame = string.Empty;
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
            cbxGames.SelectedIndex = 0;
        }

        // Runtypes
        private List<string> FindRunTypes()
        {
            var types = new List<string>
            {
                "NG", "CoC", "Broken Thief Sword", "Broken Straight Sword", "+0",
                "No Roll", "Deathless", "NoHit", "NG+ 7 No Aux", "No Roll/Block/Parry", "+0 Weapons No Aux",
                "No Deflect", "No Items", "Sword Only", "Base Att", "Hardmode"
            };

            string selectedRestriction = cbxRestrictions.Text;

            var restrictions = types
                .Where(type => selectedRestriction.Contains(type))
                .Select(type => HttpUtility.UrlEncode(type))
                .ToList();

            if (restrictions.Count == 0)
                restrictions.Add("Other");

            return restrictions;
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

            await LoadGameDataAsync(currentGame, basePath);
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
            // Entry IDS
            string entryRadio = "entry.1804428826=";
            string entryCheck = "&entry.805188992=";
            string entryOther = "&entry.1655991278=";

            string gamePath = Path.Combine(basePath, currentGame, "Submission.txt");
            string submissionTemplate = File.ReadAllText(gamePath);

            string category = $"{entryRadio}{currentRunType}";

            var restrictions = FindRunTypes();
            string restrictionsParameters = string.Join("&", restrictions.Select((r, i) => $"{entryCheck}{r}"));

            string submissionURL = submissionTemplate
                .Replace("{Category}", category)
                .Replace("{Restrictions}", restrictionsParameters);

            string restriction = cbxRestrictions.SelectedItem.ToString()!;
            string otherRestriction = HttpUtility.UrlEncode(restriction);

            submissionURL += $"{entryOther}{otherRestriction}";

            DialogResult result = MessageBox.Show($"Congratulations on completing your run for {currentGame}: \n{restriction}!\n" +
                $"You will be redirected to the Google Submission Form.\n" +
                "Would you like to continue?", "Submission Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
                await OpenURLAsync(submissionURL);
            else return;
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


        private void btnReset_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbBosses.Items.Count; i++)
                clbBosses.SetItemChecked(i, false);

            bossCount = 0;
            lblBosses.Text = $"{bossCount}/{clbBosses.Items.Count}";

            CheckRunType();
        }

        private async void btnRules_Click(object sender, EventArgs e)
        {
            await OpenURLAsync(RulesURL);
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
        private async void BossForm_Load(object sender, EventArgs e)
        {
            isDarkMode = Properties.Settings.Default.UserSelectedMode;
            SetMode(isDarkMode);

            string gameDataJson = Properties.Settings.Default.CompletedBosses;
            gameData?.DeserializeGameDataFromJson(gameDataJson);

            // GitHub
            GitHubClient gitHubClient = new GitHubClient(new ProductHeaderValue("SoulsChallenge"));
            try
            {
                Release release = await gitHubClient.Repository.Release.GetLatest("DevoWarr", "SoulsChallenge");

                if (SemVersion.Parse(release.TagName) > System.Windows.Forms.Application.ProductVersion)
                {
                    DialogResult result = MessageBox.Show("New Update Available\n" +
                        "Would you like to update to the latest version?",
                        "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                        await OpenURLAsync(release.HtmlUrl);
                    else return;
                }
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is ApiException || ex is ArgumentException)
            {
                MessageBox.Show("App Version Unknown!", "Unknown App Version");
            }
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