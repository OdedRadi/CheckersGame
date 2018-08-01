using System;
using System.Drawing;
using System.Windows.Forms;

namespace CheckersGame.UserInterface
{
	public class PictureBoxSoldier : PictureBoxTile
	{
		private const int k_MovingAnimationSpeed = 10;
		private const int k_MovingStepSize = 4;
		private const int k_DisapearBlinkingAnimationSpeed = 250;
		private const int k_DisapearBlinkingCount = 2;
		private const int k_BecomeKingAnimationSpeed = 50;
		private const int k_BecomeKingGrowingAmplitude = 7;
		private readonly Timer r_MovingTimer = new Timer();
		private readonly Timer r_DisapearTimer = new Timer();
		private readonly Timer r_BecomeKingTimer = new Timer();
		private Point m_LocationToMove;
		private int m_MovingDirectionX;
		private int m_MovingDirectionY;
		private int m_MovingDistance;
		private int m_DisapearBlinkingCounter;
		private int m_KingGrowingCounter;
		private eSymbols m_Symbol;

		public event SoldierAnimationFinishedEventHandler AnimationFinished;

		public PictureBoxSoldier(eSymbols i_Symbol, Coordinate i_Coordinate) : base(i_Coordinate)
		{
			m_Symbol = i_Symbol;
			BackColor = Color.Transparent;
			SetImage();
			initTimers();
		}

		private void initTimers()
		{
			r_MovingTimer.Interval = k_MovingAnimationSpeed;
			r_MovingTimer.Tick += movingTimer_Tick;
			r_DisapearTimer.Interval = k_DisapearBlinkingAnimationSpeed;
			r_DisapearTimer.Tick += disapearTimer_Tick;
			r_BecomeKingTimer.Interval = k_BecomeKingAnimationSpeed;
			r_BecomeKingTimer.Tick += becomeKingTimer_Tick;
		}

		private void disapearTimer_Tick(object sender, EventArgs e)
		{
			const bool v_BeVisible = true;

			Visible = m_DisapearBlinkingCounter % 2 == 0 ? !v_BeVisible : v_BeVisible;
			m_DisapearBlinkingCounter++;
			if (m_DisapearBlinkingCounter == (k_DisapearBlinkingCount * 2) + 1)
			{
				r_DisapearTimer.Stop();
				OnAnimationFinished();
			}
		}

		private void becomeKingTimer_Tick(object sender, EventArgs e)
		{
			if (m_KingGrowingCounter < k_BecomeKingGrowingAmplitude)
			{
				Width += 2;
				Height += 2;
				Left--;
				Top--;
			}
			else
			{
				Width -= 2;
				Height -= 2;
				Left++;
				Top++;
			}

			m_KingGrowingCounter++;
			if (m_KingGrowingCounter == k_BecomeKingGrowingAmplitude * 2)
			{
				r_BecomeKingTimer.Stop();
				OnAnimationFinished();
			}
		}

		public void SetSelected(bool i_IsSelected)
		{
			BackColor = i_IsSelected ? Color.DarkBlue : Color.Transparent;
		}

		protected override void SetImage()
		{
			switch (m_Symbol)
			{
				case eSymbols.X:
					m_InitianlizedImage = Image.FromFile(@"..\..\Resources\checker black.png");
					m_MarkedRedImage = Image.FromFile(@"..\..\Resources\checker black marked red.png");
					break;
				case eSymbols.O:
					m_InitianlizedImage = Image.FromFile(@"..\..\Resources\checker white.png");
					m_MarkedRedImage = Image.FromFile(@"..\..\Resources\checker white marked red.png");
					break;
			}

			Image = m_InitianlizedImage;
		}

		private void movingTimer_Tick(object sender, EventArgs e)
		{
			Left += m_MovingDirectionX;
			Top += m_MovingDirectionY;
			if (Math.Abs(Location.X - m_LocationToMove.X) >= m_MovingDistance / 2)
			{
				Height++;
				Width++;
			}
			else
			{
				Height--;
				Width--;
			}

			if (Location == m_LocationToMove)
			{
				r_MovingTimer.Stop();
				OnAnimationFinished();
			}
		}

		private void OnAnimationFinished()
		{
			if (AnimationFinished != null)
			{
				AnimationFinished.Invoke();
			}
		}

		public void MoveTo(Coordinate i_Coordinate)
		{
			m_MovingDirectionX = i_Coordinate.Column > m_Coordinate.Column ? k_MovingStepSize : -k_MovingStepSize;
			m_MovingDirectionY = i_Coordinate.Row > m_Coordinate.Row ? k_MovingStepSize : -k_MovingStepSize;
			BringToFront();
			SetSelected(false);
			m_LocationToMove = CoordinateToLocation(i_Coordinate);
			m_MovingDistance = Math.Abs(Location.X - m_LocationToMove.X);
			r_MovingTimer.Start();
			m_Coordinate = i_Coordinate;
		}

		public void BecomeKing(eSymbols i_Symbol)
		{
			switch (i_Symbol)
			{
				case eSymbols.K:
					m_InitianlizedImage = Image.FromFile(@"..\..\Resources\checker black king.png");
					m_MarkedRedImage = Image.FromFile(@"..\..\Resources\checker black king marked red.png");
					break;
				case eSymbols.U:
					m_InitianlizedImage = Image.FromFile(@"..\..\Resources\checker white king.png");
					m_MarkedRedImage = Image.FromFile(@"..\..\Resources\checker white king marked red.png");
					break;
			}

			m_Symbol = i_Symbol;
			Image = m_InitianlizedImage;
			m_KingGrowingCounter = 0;
			r_BecomeKingTimer.Start();
		}

		public void Disapear()
		{
			m_DisapearBlinkingCounter = 0;
			r_DisapearTimer.Start();
		}
	}
}
