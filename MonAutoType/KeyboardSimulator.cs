using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace MonAutoType
{
    /// <summary>
    /// Classe pour simuler la saisie clavier via l'API Windows SendInput
    /// Utilise l'approche Unicode pour gérer tous les caractères spéciaux
    /// </summary>
    public static class KeyboardSimulator
    {
        #region Structures Win32
        
        /// <summary>
        /// Structure INPUT pour SendInput
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public uint type;
            public InputUnion U;
        }
        
        /// <summary>
        /// Union pour les différents types d'input
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        private struct InputUnion
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;
            [FieldOffset(0)]
            public KEYBDINPUT ki;
            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }
        
        /// <summary>
        /// Structure pour les entrées clavier
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public ushort wVk;        // Code de touche virtuelle (0 pour Unicode)
            public ushort wScan;      // Code de scan hardware ou caractère Unicode
            public uint dwFlags;      // Flags pour le type d'événement
            public uint time;         // Timestamp (0 = système génère)
            public IntPtr dwExtraInfo; // Info supplémentaire
        }
        
        /// <summary>
        /// Structures vides pour MOUSEINPUT et HARDWAREINPUT (nécessaires pour la compilation)
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        private struct HARDWAREINPUT
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }
        
        #endregion
        
        #region Constantes Win32
        
        private const uint INPUT_KEYBOARD = 1;
        private const uint KEYEVENTF_UNICODE = 0x0004;
        private const uint KEYEVENTF_KEYUP = 0x0002;
        
        #endregion
        
        #region Import Win32 API
        
        /// <summary>
        /// Fonction SendInput de l'API Windows
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
        
        /// <summary>
        /// Obtenir des informations supplémentaires pour les événements
        /// </summary>
        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();
        
        #endregion
        
        #region Méthodes publiques
        
        /// <summary>
        /// Simule la saisie d'un texte complet
        /// </summary>
        /// <param name="texte">Le texte à taper automatiquement</param>
        /// <param name="delaiEntreCaracteres">Délai en millisecondes entre chaque caractère (défaut: 10ms)</param>
        public static void TaperTexte(string texte, int delaiEntreCaracteres = 10)
        {
            if (string.IsNullOrEmpty(texte))
                return;
            
            foreach (char caractere in texte)
            {
                TaperCaractereUnicode(caractere);
                
                // Petit délai pour simuler une frappe humaine
                if (delaiEntreCaracteres > 0)
                {
                    Thread.Sleep(delaiEntreCaracteres);
                }
            }
        }
        
        /// <summary>
        /// Simule la saisie d'un seul caractère Unicode
        /// </summary>
        /// <param name="caractere">Le caractère à taper</param>
        private static void TaperCaractereUnicode(char caractere)
        {
            // Créer deux événements : KeyDown et KeyUp
            INPUT[] inputs = new INPUT[2];
            
            // Événement KeyDown
            inputs[0] = new INPUT
            {
                type = INPUT_KEYBOARD,
                U = new InputUnion
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = 0,                        // 0 car on utilise Unicode
                        wScan = caractere,              // Le caractère Unicode
                        dwFlags = KEYEVENTF_UNICODE,   // Flag pour indiquer Unicode
                        time = 0,                       // Laisser le système gérer
                        dwExtraInfo = GetMessageExtraInfo()
                    }
                }
            };
            
            // Événement KeyUp
            inputs[1] = new INPUT
            {
                type = INPUT_KEYBOARD,
                U = new InputUnion
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = 0,                                          // 0 car on utilise Unicode
                        wScan = caractere,                                // Le caractère Unicode
                        dwFlags = KEYEVENTF_UNICODE | KEYEVENTF_KEYUP,  // Unicode + KeyUp
                        time = 0,                                        // Laisser le système gérer
                        dwExtraInfo = GetMessageExtraInfo()
                    }
                }
            };
            
            // Envoyer les événements
            uint result = SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT)));
            
            // Vérifier que les événements ont été envoyés
            if (result != 2)
            {
                int erreur = Marshal.GetLastWin32Error();
                throw new Exception($"Erreur lors de l'envoi du caractère '{caractere}'. Code d'erreur Win32: {erreur}");
            }
        }
        
        /// <summary>
        /// Simule la saisie d'un texte avec gestion des retours à la ligne
        /// </summary>
        /// <param name="texte">Le texte multiligne à taper</param>
        /// <param name="delaiEntreCaracteres">Délai entre chaque caractère en ms</param>
        /// <param name="delaiEntreLignes">Délai supplémentaire entre les lignes en ms</param>
        public static void TaperTexteMultiligne(string texte, int delaiEntreCaracteres = 10, int delaiEntreLignes = 50)
        {
            if (string.IsNullOrEmpty(texte))
                return;
            
            // Diviser le texte en lignes
            string[] lignes = texte.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            
            for (int i = 0; i < lignes.Length; i++)
            {
                // Taper la ligne
                TaperTexte(lignes[i], delaiEntreCaracteres);
                
                // Si ce n'est pas la dernière ligne, ajouter un retour à la ligne
                if (i < lignes.Length - 1)
                {
                    TaperCaractereUnicode('\r');  // Retour chariot
                    TaperCaractereUnicode('\n');  // Nouvelle ligne
                    
                    if (delaiEntreLignes > 0)
                    {
                        Thread.Sleep(delaiEntreLignes);
                    }
                }
            }
        }
        
        #endregion
    }
}