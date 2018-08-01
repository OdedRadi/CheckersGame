using System;
using System.Drawing;
using System.Windows.Forms;

namespace CheckersGame.UserInterface
{
	public class FormSettings : Form
	{
		private const int k_ComponentSpacing = 10;
		private Label labelPlayers;
		private Label labelPlayer1;
		private CheckBox checkBoxPlayer2;
		private TextBox textBoxPlayer1Name;
		private TextBox textBoxPlayer2Name;
		private Panel panelPlayers;
		private Label labelBoardSize;
		private Panel panelBoardSize;
		private RadioButton radioButton6x6;
		private RadioButton radioButton8x8;
		private RadioButton radioButton10x10;
		private Button buttonDone;
		private ErrorProvider m_PlayerNameErrorProvider;
		private RadioButton m_RadioButtonSelected;

		public FormSettings()
		{
			InitializeComponent();
		}

		public Settings Settings
		{
			get
			{
				Player player1 = createPlayer(1);
				Player player2 = createPlayer(2);
				int boardSize = (int)m_RadioButtonSelected.Tag;

				return new Settings(player1, player2, boardSize);
			}
		}

		private Player createPlayer(int i_PlayerNum)
		{
			Player newPlayer = new Player();

			newPlayer.Name = i_PlayerNum == 1 ? textBoxPlayer1Name.Text : textBoxPlayer2Name.Text;
			newPlayer.Symbol = i_PlayerNum == 1 ? eSymbols.X : eSymbols.O;
			newPlayer.Score = 0;
			newPlayer.Kind = i_PlayerNum == 1 || checkBoxPlayer2.Checked ? Player.ePlayerKind.Human : Player.ePlayerKind.Computer;

			return newPlayer;
		}

		private void radioButtons_CheckedChanged(object sender, EventArgs e)
		{
			m_RadioButtonSelected = sender as RadioButton;
		}

		private void checkBoxPlayer2_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBoxPlayer2.Checked)
			{
				textBoxPlayer2Name.Enabled = true;
				textBoxPlayer2Name.Clear();
			}
			else
			{
				textBoxPlayer2Name.Enabled = false;
				textBoxPlayer2Name.Text = "[Computer]";
			}
		}

		private void textBoxes_TextChanged(object sender, EventArgs e)
		{
			buttonDone.Enabled = (textBoxPlayer1Name.Text != string.Empty) && (textBoxPlayer2Name.Text != string.Empty);
			m_PlayerNameErrorProvider.Clear();
			validatePlayerName(textBoxPlayer1Name);
			validatePlayerName(textBoxPlayer2Name);
		}

		private void validatePlayerName(TextBox i_TextBox)
		{
			string text = i_TextBox.Text;
			string errorMessage;

			if (text.Length > Player.k_MaxPlayerNameLength)
			{
				errorMessage = string.Format("Player name max length is {0}", Player.k_MaxPlayerNameLength);
				m_PlayerNameErrorProvider.SetError(i_TextBox, errorMessage);
				buttonDone.Enabled = false;
			}
			else if (text.Contains(" "))
			{
				errorMessage = "Player name cannot contain spaces";
				m_PlayerNameErrorProvider.SetError(i_TextBox, errorMessage);
				buttonDone.Enabled = false;
			}
		}

		private void buttonDone_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (DialogResult != DialogResult.OK)
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
			labelBoardSize = new Label();
			radioButton6x6 = new RadioButton();
			radioButton8x8 = new RadioButton();
			radioButton10x10 = new RadioButton();
			panelBoardSize = new Panel();
			labelPlayers = new Label();
			labelPlayer1 = new Label();
			textBoxPlayer1Name = new TextBox();
			checkBoxPlayer2 = new CheckBox();
			textBoxPlayer2Name = new TextBox();
			panelPlayers = new Panel();
			buttonDone = new Button();
			m_PlayerNameErrorProvider = new ErrorProvider();

			// Init board size radio button and their panel 
			// Label board size 
			labelBoardSize.Location = new Point(k_ComponentSpacing * 2, k_ComponentSpacing * 2);
			labelBoardSize.AutoSize = true;
			labelBoardSize.TabIndex = 10;
			labelBoardSize.Text = "Board Size:";

			// Radio button 6x6 
			radioButton6x6.Location = new Point(labelBoardSize.Left, labelBoardSize.Top);
			radioButton6x6.Tag = 6;
			radioButton6x6.Text = "6 x 6";
			radioButton6x6.TabIndex = 20;
			radioButton6x6.Width = 60;
			radioButton6x6.Checked = true;
			radioButton6x6.CheckedChanged += radioButtons_CheckedChanged;
			m_RadioButtonSelected = radioButton6x6;

			// Radio button 8x8 
			radioButton8x8.Left = radioButton6x6.Right + k_ComponentSpacing;
			radioButton8x8.Top = radioButton6x6.Top;
			radioButton8x8.Tag = 8;
			radioButton8x8.Text = "8 x 8";
			radioButton8x8.TabIndex = 30;
			radioButton8x8.Width = 60;
			radioButton8x8.CheckedChanged += radioButtons_CheckedChanged;

			// Radio button 10x10 
			radioButton10x10.Left = radioButton8x8.Right + k_ComponentSpacing;
			radioButton10x10.Top = radioButton8x8.Top;
			radioButton10x10.Tag = 10;
			radioButton10x10.Text = "10 x 10";
			radioButton10x10.TabIndex = 40;
			radioButton10x10.Width = 60;
			radioButton10x10.CheckedChanged += radioButtons_CheckedChanged;

			// Radio buttons panel 
			panelBoardSize.Location = new Point(labelBoardSize.Left, labelBoardSize.Top + k_ComponentSpacing);
			panelBoardSize.Controls.Add(radioButton6x6);
			panelBoardSize.Controls.Add(radioButton8x8);
			panelBoardSize.Controls.Add(radioButton10x10);
			panelBoardSize.Height = k_ComponentSpacing + radioButton6x6.Height + k_ComponentSpacing;
			panelBoardSize.Width = k_ComponentSpacing + radioButton6x6.Width + k_ComponentSpacing + radioButton8x8.Width + k_ComponentSpacing + radioButton10x10.Width + k_ComponentSpacing;
			panelBoardSize.BackColor = Color.Transparent;

			// Init players names labels and textboxes and their groupbox and checkbox 
			// Players Label 
			labelPlayers.Location = new Point(labelBoardSize.Left, panelBoardSize.Bottom + k_ComponentSpacing);
			labelPlayers.Text = "Players:";
			labelPlayers.TabIndex = 50;
			labelPlayers.AutoSize = true;

			// Player1 label 
			labelPlayer1.Size = new Size(70, 20);
			labelPlayer1.Location = new Point(labelPlayers.Left, k_ComponentSpacing * 2);
			labelPlayer1.Text = "Player 1:";
			labelPlayer1.TabIndex = 60;

			// Player1 textbox 
			textBoxPlayer1Name.Size = new Size(80, 20);
			textBoxPlayer1Name.Left = labelPlayer1.Right + k_ComponentSpacing;
			textBoxPlayer1Name.Top = labelPlayer1.Top;
			textBoxPlayer1Name.TabIndex = 70;
			textBoxPlayer1Name.TextChanged += textBoxes_TextChanged;
			textBoxPlayer1Name.Width = radioButton10x10.Right - textBoxPlayer1Name.Left - (k_ComponentSpacing * 2);

			// Player2 checkbox 
			checkBoxPlayer2.Left = labelPlayers.Left;
			checkBoxPlayer2.Top = labelPlayer1.Bottom + k_ComponentSpacing;
			checkBoxPlayer2.AutoSize = true;
			checkBoxPlayer2.Text = "Player 2:";
			checkBoxPlayer2.CheckedChanged += checkBoxPlayer2_CheckedChanged;
			checkBoxPlayer2.Checked = true;
			checkBoxPlayer2.Checked = false;
			checkBoxPlayer2.TabIndex = 80;

			// Player2 textbox 
			textBoxPlayer2Name.Size = new Size(80, 20);
			textBoxPlayer2Name.Left = textBoxPlayer1Name.Left;
			textBoxPlayer2Name.Top = checkBoxPlayer2.Top;
			textBoxPlayer2Name.TabIndex = 90;
			textBoxPlayer2Name.TextChanged += textBoxes_TextChanged;
			textBoxPlayer2Name.Width = textBoxPlayer1Name.Width;

			// Players panel
			panelPlayers.Location = new Point(labelPlayers.Left, labelPlayers.Top + k_ComponentSpacing);
			panelPlayers.Controls.Add(labelPlayer1);
			panelPlayers.Controls.Add(checkBoxPlayer2);
			panelPlayers.Controls.Add(textBoxPlayer1Name);
			panelPlayers.Controls.Add(textBoxPlayer2Name);
			panelPlayers.Height = k_ComponentSpacing + textBoxPlayer1Name.Height + k_ComponentSpacing + textBoxPlayer2Name.Height + k_ComponentSpacing;
			panelPlayers.Width = panelBoardSize.Width;

			// Init form 
			Text = "Game Settings";
			StartPosition = FormStartPosition.CenterScreen;
			FormBorderStyle = FormBorderStyle.Fixed3D;
			MaximizeBox = false;
			MinimizeBox = false;
			Controls.Add(labelBoardSize);
			Controls.Add(panelBoardSize);
			Controls.Add(labelPlayers);
			Controls.Add(panelPlayers);
			Controls.Add(buttonDone);
			Height = labelBoardSize.Top + labelBoardSize.Height + k_ComponentSpacing + panelBoardSize.Height + k_ComponentSpacing + labelPlayers.Height + k_ComponentSpacing + panelPlayers.Height + k_ComponentSpacing + buttonDone.Height + (k_ComponentSpacing * 2);
			Width = k_ComponentSpacing + (panelBoardSize.Right - labelBoardSize.Left) + (k_ComponentSpacing * 4);
			AcceptButton = buttonDone;
			FormClosing += FormSettings_FormClosing;

			// Init button done 
			buttonDone.Text = "Done";
			buttonDone.TabIndex = 100;
			buttonDone.Size = new Size(80, 28);
			buttonDone.Left = ClientSize.Width - buttonDone.Width - k_ComponentSpacing;
			buttonDone.Top = ClientSize.Height - buttonDone.Height - k_ComponentSpacing;
			buttonDone.Enabled = false;
			buttonDone.Click += buttonDone_Click;
		}
	}
}
