using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;

namespace MonAutoType
{
    public partial class MainForm : Form
    {
        // Zone de texte pour le texte à taper automatiquement
        private TextBox textBoxContenu;
        // Bouton pour déclencher la saisie automatique
        private Button boutonAutoType;
        // Label pour afficher le compte à rebours
        private Label labelStatut;
        // Champ pour le délai avant la saisie
        private NumericUpDown numericDelai;
        private Label labelDelai;

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialise les composants de l'interface graphique
        /// </summary>
        private void InitializeComponent()
        {
            // Configuration de la fenêtre principale
            this.Text = "Auto-Type";
            this.Size = new System.Drawing.Size(600, 520);
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Création de la zone de texte multiligne
            textBoxContenu = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(560, 340),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Font = new System.Drawing.Font("Consolas", 10F)
            };

            // Création du label pour le délai
            labelDelai = new Label
            {
                Text = "Delay (sec):",
                Location = new System.Drawing.Point(10, 365),
                Size = new System.Drawing.Size(80, 25),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                Font = new System.Drawing.Font("Segoe UI", 9F),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Création du champ numérique pour le délai
            numericDelai = new NumericUpDown
            {
                Location = new System.Drawing.Point(95, 363),
                Size = new System.Drawing.Size(60, 25),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                Font = new System.Drawing.Font("Segoe UI", 9F),
                Minimum = 1,
                Maximum = 30,
                Value = 5,
                Increment = 1
            };

            // Création du label de statut
            labelStatut = new Label
            {
                Text = "Ready",
                Location = new System.Drawing.Point(10, 435),
                Size = new System.Drawing.Size(200, 25),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                Font = new System.Drawing.Font("Segoe UI", 9F)
            };

            // Création du bouton Auto-Type avec coins arrondis
            boutonAutoType = new RoundedButton
            {
                Text = "Auto-Type",
                Location = new System.Drawing.Point(230, 400),
                Size = new System.Drawing.Size(140, 45),
                Anchor = AnchorStyles.Bottom,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.FromArgb(0, 122, 204),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            boutonAutoType.FlatAppearance.BorderSize = 0;

            // Ajout de l'événement de clic sur le bouton
            boutonAutoType.Click += BoutonAutoType_Click;

            // Ajout des contrôles à la fenêtre
            this.Controls.Add(textBoxContenu);
            this.Controls.Add(labelDelai);
            this.Controls.Add(numericDelai);
            this.Controls.Add(boutonAutoType);
            this.Controls.Add(labelStatut);
        }
        
        /// <summary>
        /// Gestion du clic sur le bouton Auto-Type
        /// </summary>
        private async void BoutonAutoType_Click(object? sender, EventArgs e)
        {
            // Vérifier qu'il y a du texte à taper
            if (string.IsNullOrEmpty(textBoxContenu.Text))
            {
                MessageBox.Show("Please enter some text to type automatically.",
                               "No text",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);
                return;
            }

            // Désactiver le bouton pendant la saisie
            boutonAutoType.Enabled = false;

            // Minimiser la fenêtre principale
            this.WindowState = FormWindowState.Minimized;

            // Afficher le compte à rebours dans une fenêtre popup
            int delai = (int)numericDelai.Value;
            bool cancelled = await AfficherCompteARebours(delai);

            if (cancelled)
            {
                // L'utilisateur a annulé
                this.WindowState = FormWindowState.Normal;
                boutonAutoType.Enabled = true;
                labelStatut.Text = "Cancelled";
                await Task.Delay(2000);
                labelStatut.Text = "Ready";
                return;
            }

            labelStatut.Text = "Typing...";

            try
            {
                // Appeler le simulateur de clavier
                await Task.Run(() =>
                {
                    KeyboardSimulator.TaperTexteMultiligne(textBoxContenu.Text,
                                                           delaiEntreCaracteres: 15,
                                                           delaiEntreLignes: 100);
                });

                labelStatut.Text = "Typing completed successfully";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during auto-typing: {ex.Message}",
                               "Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
                labelStatut.Text = "Error during typing";
            }
            finally
            {
                // Restaurer la fenêtre et réactiver le bouton
                this.WindowState = FormWindowState.Normal;
                boutonAutoType.Enabled = true;

                // Remettre le statut par défaut après 3 secondes
                await Task.Delay(3000);
                labelStatut.Text = "Ready";
            }
        }

        /// <summary>
        /// Affiche une fenêtre popup avec le compte à rebours
        /// </summary>
        /// <returns>True si annulé, False sinon</returns>
        private async Task<bool> AfficherCompteARebours(int secondes)
        {
            bool cancelled = false;
            using var countdownForm = new Form
            {
                Text = "Auto-Type",
                Size = new System.Drawing.Size(500, 500),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false,
                TopMost = true,
                BackColor = System.Drawing.Color.FromArgb(45, 45, 48)
            };

            var labelMessage = new Label
            {
                Text = "Click on the target window",
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font("Segoe UI", 14F),
                ForeColor = System.Drawing.Color.White
            };

            var labelCountdown = new Label
            {
                Text = secondes.ToString(),
                Dock = DockStyle.Top,
                Height = 280,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font("Segoe UI", 180F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                BackColor = System.Drawing.Color.FromArgb(45, 45, 48)
            };

            var panelButton = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = System.Drawing.Color.FromArgb(45, 45, 48)
            };

            var boutonAnnuler = new RoundedButton
            {
                Text = "Cancel",
                Size = new System.Drawing.Size(140, 50),
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.FromArgb(0, 122, 204),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            boutonAnnuler.FlatAppearance.BorderSize = 0;
            boutonAnnuler.Click += (s, e) =>
            {
                cancelled = true;
                countdownForm.Close();
            };

            // Center the button in the panel
            panelButton.Controls.Add(boutonAnnuler);
            panelButton.Resize += (s, e) =>
            {
                boutonAnnuler.Location = new System.Drawing.Point(
                    (panelButton.Width - boutonAnnuler.Width) / 2,
                    (panelButton.Height - boutonAnnuler.Height) / 2
                );
            };

            // Add controls in reverse order for DockStyle.Top
            countdownForm.Controls.Add(panelButton);
            countdownForm.Controls.Add(labelCountdown);
            countdownForm.Controls.Add(labelMessage);
            countdownForm.Show();

            // Trigger resize to center button
            boutonAnnuler.Location = new System.Drawing.Point(
                (panelButton.Width - boutonAnnuler.Width) / 2,
                (panelButton.Height - boutonAnnuler.Height) / 2
            );

            for (int i = secondes; i > 0 && !cancelled; i--)
            {
                labelCountdown.Text = i.ToString();
                for (int j = 0; j < 10 && !cancelled; j++)
                {
                    await Task.Delay(100);
                    Application.DoEvents();
                }
            }

            if (!cancelled)
            {
                countdownForm.Close();
            }

            return cancelled;
        }
    }

    /// <summary>
    /// Bouton personnalisé avec coins arrondis
    /// </summary>
    public class RoundedButton : Button
    {
        public int BorderRadius { get; set; } = 10;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            GraphicsPath path = new GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            int radius = BorderRadius;

            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();

            this.Region = new Region(path);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (SolidBrush brush = new SolidBrush(this.BackColor))
            {
                e.Graphics.FillPath(brush, path);
            }

            TextRenderer.DrawText(e.Graphics, this.Text, this.Font, rect, this.ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}