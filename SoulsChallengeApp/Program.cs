using System.Configuration;

namespace SoulsChallengeApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Properties.Settings settings = Properties.Settings.Default;

            if (settings.UpgradeRequired)
            {
                settings.Upgrade();
                settings.UpgradeRequired = false;
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new BossForm());

            settings.Save();
        }
    }
}