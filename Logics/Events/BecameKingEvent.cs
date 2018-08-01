namespace CheckersGame.Logics.Events
{
	public class BecameKingEvent : IGameLogicsEvent
	{
		private Coordinate m_Coordinate;
		private eSymbols m_Symbol;

		public BecameKingEvent(eSymbols i_Symbol, Coordinate i_SoldierCoordinate)
		{
			m_Symbol = i_Symbol;
			m_Coordinate = i_SoldierCoordinate;
		}

		public Coordinate Coordinate
		{
			get
			{
				return m_Coordinate;
			}
		}

		public eSymbols Symbol
		{
			get
			{
				return m_Symbol;
			}
		}
	}
}
