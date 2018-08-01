using System;
using System.Drawing;
using System.Windows.Forms;

namespace CheckersGame.UserInterface
{
	public class PictureBoxTile : PictureBox
	{
		private const int k_TileSize = 80;
		private const int k_BlinkingRedCount = 4;
		private const int k_BlinkRedAnimationSpeed = 250;
		private readonly Timer r_BlinkRedTimer = new Timer();
		private int m_BlinkingRedCounter;
		protected Image m_InitianlizedImage;
		protected Image m_MarkedRedImage;
		protected Coordinate m_Coordinate;

		public PictureBoxTile(Coordinate i_Coordinate)
		{
			m_Coordinate = i_Coordinate;
			Location = CoordinateToLocation(i_Coordinate);
			Size = new Size(k_TileSize, k_TileSize);
			SizeMode = PictureBoxSizeMode.StretchImage;
			SetImage();
			r_BlinkRedTimer.Interval = k_BlinkRedAnimationSpeed;
			r_BlinkRedTimer.Tick += blinkRedTimer_Tick;
		}

		private void blinkRedTimer_Tick(object sender, EventArgs e)
		{
			Image = m_BlinkingRedCounter % 2 == 0 ? m_MarkedRedImage : m_InitianlizedImage;
			m_BlinkingRedCounter++;
			if (m_BlinkingRedCounter == k_BlinkingRedCount)
			{
				r_BlinkRedTimer.Stop();
			}
		}

		public void BlinkRed()
		{
			m_BlinkingRedCounter = 0;
			r_BlinkRedTimer.Start();
		}

		protected virtual void SetImage()
		{
			int i = m_Coordinate.Row;
			int j = m_Coordinate.Column;

			if ((i + j) % 2 == 0)
			{
				m_InitianlizedImage = Image.FromFile(@"..\..\Resources\tile light.png");
				m_MarkedRedImage = Image.FromFile(@"..\..\Resources\tile light marked red.png");
			}
			else
			{
				m_InitianlizedImage = Image.FromFile(@"..\..\Resources\tile dark.png");
				m_MarkedRedImage = Image.FromFile(@"..\..\Resources\tile dark marked red.png");
			}

			Image = m_InitianlizedImage;
		}

		public Coordinate Coordinate
		{
			get
			{
				return m_Coordinate;
			}
		}

		public static int TileSize
		{
			get
			{
				return k_TileSize;
			}
		}

		protected Point CoordinateToLocation(Coordinate i_Coordinate)
		{
			return new Point(i_Coordinate.Column * k_TileSize, i_Coordinate.Row * k_TileSize);
		}
	}
}
