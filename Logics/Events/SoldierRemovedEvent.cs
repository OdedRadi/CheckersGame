namespace CheckersGame.Logics.Events
{
	public class SoldierRemovedEvent : IGameLogicsEvent
	{
		private Coordinate m_Coordinate;

		public SoldierRemovedEvent(Coordinate i_RemovedSoldierCoordinate)
		{
			m_Coordinate = i_RemovedSoldierCoordinate;
		}		

		public Coordinate Coordinate
		{
			get
			{
				return m_Coordinate;
			}
		}		
	}
}
