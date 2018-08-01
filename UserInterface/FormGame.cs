using System;
using System.Drawing;
using System.Windows.Forms;

namespace CheckersGame.UserInterface
{
	public class FormGame : Form
	{
		private const int k_ComponentSpacing = 10;
		private Settings m_Settings;
		private Panel panelPlayersLabels;
		private Label labelPlayer1;
		private Label labelPlayer2;
		private PanelCheckers panelCheckers;

		public FormGame(Settings i_Settings)
		{
			m_Settings = i_Settings;
			InitializeComponent();
			panelCheckers.Restart();
		}

		private void panelCheckers_GameEnded(string i_WinnerPlayerName, int i_Player1Score, int i_Player2Score)
		{
			labelPlayer1.Text = string.Format("{0}: {1}", m_Settings.Player1.Name, i_Player1Score.ToString());
			labelPlayer2.Text = string.Format("{0}: {1}", m_Settings.Player2.Name, i_Player2Score.ToString());
			DialogResult = DialogResult.None;
			askForContinue(i_WinnerPlayerName);
		}

		private void askForContinue(string i_WinnerPlayerName)
		{
			DialogResult result = MessageBox.Show(
				   string.Format("{0} Won!{1}Another round?", i_WinnerPlayerName, Environment.NewLine),
				   "Round Over",
				   MessageBoxButtons.YesNo,
				   MessageBoxIcon.Information);

			if (result == DialogResult.Yes)
			{
				panelCheckers.Restart();
			}
			else
			{
				Close();
			}
		}

		private void FormGame_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (DialogResult != DialogResult.None)
			{
				DialogResult result = MessageBox.Show(
					"Are you sure you want to quit?",
					"Quit Game",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Warning);

				e.Cancel = result == DialogResult.No;
			}
		}

		private void InitializeComponent()
		{
			// Allocate all components
			panelCheckers = new PanelCheckers(m_Settings);
			panelPlayersLabels = new Panel();
			labelPlayer1 = new Label();
			labelPlayer2 = new Label();

			// Players labels panel 
			panelPlayersLabels.Location = new Point(k_ComponentSpacing, k_ComponentSpacing);
			panelPlayersLabels.Size = new Size(panelCheckers.Width, labelPlayer1.Height + (k_ComponentSpacing * 2));
			panelPlayersLabels.Controls.Add(labelPlayer1);
			panelPlayersLabels.Controls.Add(labelPlayer2);

			// Label player 1 name 
			labelPlayer1.AutoSize = true;
			labelPlayer1.Font = new Font("Consolas", 12);
			labelPlayer1.Text = string.Format("{0}: 0", m_Settings.Player1.Name);
			labelPlayer1.Location = new Point(k_ComponentSpacing, k_ComponentSpacing);

			// Label player 2 name 
			labelPlayer2.AutoSize = true;
			labelPlayer2.Font = new Font("Consolas", 12);
			labelPlayer2.Text = string.Format("{0}: 0", m_Settings.Player2.Name);
			labelPlayer2.Left = panelPlayersLabels.Right - labelPlayer2.Width - (k_ComponentSpacing * 2);
			labelPlayer2.Top = labelPlayer1.Top;

			// Checkers panel
			panelCheckers.GameEnded += panelCheckers_GameEnded;
			panelCheckers.Top = panelPlayersLabels.Bottom + k_ComponentSpacing;
			panelCheckers.Left = k_ComponentSpacing;
			panelCheckers.Anchor = AnchorStyles.Bottom;
			panelCheckers.BorderStyle = BorderStyle.FixedSingle;

			// Init form 
			ClientSize = new Size(
				k_ComponentSpacing + panelCheckers.Width + k_ComponentSpacing,
				k_ComponentSpacing + panelPlayersLabels.Height + k_ComponentSpacing + panelCheckers.Height + k_ComponentSpacing);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			StartPosition = FormStartPosition.CenterScreen;
			MaximizeBox = false;
			Text = "Checkers Game";
			Controls.Add(panelPlayersLabels);
			Controls.Add(panelCheckers);
			FormClosing += FormGame_FormClosing;
		}
	}
}
