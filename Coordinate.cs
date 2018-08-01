namespace CheckersGame
{
	public struct Coordinate
	{
		private int m_Row;
		private int m_Col;

		public Coordinate(int i_Row, int i_Col)
		{
			m_Row = i_Row;
			m_Col = i_Col;
		}

		public int Row
		{
			get
			{
				return m_Row;
			}
		}

		public int Column
		{
			get
			{
				return m_Col;
			}
		}
	}
}
