namespace CheckersGame.Logics.Events
{
	public class SoldierMovedEvent : IGameLogicsEvent
	{
		private Move m_Move;

		public SoldierMovedEvent(Move i_Move)
		{
			m_Move = i_Move;
		}

		public Move Move
		{
			get
			{
				return m_Move;
			}
		}
	}
}
