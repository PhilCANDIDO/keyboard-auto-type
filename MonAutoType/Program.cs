namespace MonAutoType
{
    internal static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Activer les styles visuels modernes de Windows
            ApplicationConfiguration.Initialize();
            
            // Lancer le formulaire principal
            Application.Run(new MainForm());
        }
    }
}