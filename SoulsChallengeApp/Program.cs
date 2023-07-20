namespace DSD_App
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

            ApplicationConfiguration.Initialize();
            Application.Run(new BossForm());

            settings.Save();
        }
    }
}