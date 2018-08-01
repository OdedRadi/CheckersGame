using System.Windows.Forms;

namespace CheckersGame.UserInterface
{
	public class Launcher
	{
		public static void Start()
		{
			FormSettings formSettings = new FormSettings();

			formSettings.ShowDialog();
			if (formSettings.DialogResult == DialogResult.OK)
			{
				FormGame m_FormGame = new FormGame(formSettings.Settings);

				m_FormGame.ShowDialog();
			}
		}
	}
}
