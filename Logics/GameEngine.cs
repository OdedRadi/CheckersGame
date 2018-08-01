using System;
using System.Collections.Generic;
using CheckersGame.Logics.Events;

namespace CheckersGame
{
	public class GameEngine
	{
		public event Action<IGameLogicsEvent> EventOccured;

		private Board m_Board;
		private Player m_FirstPlayer;
		private Player m_SecondPlayer;
		private Player m_CurrentPlayer;
		private Player m_PreviousPlayer;
		private Move m_LastMove;

		public GameEngine(Player i_FirstPlayer, Player i_SecondPlayer, int i_BoardSize)
		{
			m_Board = new Board(i_BoardSize);
			m_FirstPlayer = i_FirstPlayer;
			m_SecondPlayer = i_SecondPlayer;
		}

		public void Reset()
		{
			int solidersCount = m_Board.CalcAvailbleSoldiersCount();
			List<Soldier> firstPlayerSoldiersList = m_FirstPlayer.ResetSoldiersList(solidersCount);
			List<Soldier> secondPlayerSoldiersList = m_SecondPlayer.ResetSoldiersList(solidersCount);

			m_Board.Reset(firstPlayerSoldiersList, secondPlayerSoldiersList);
			calcPlayersAviableMoves();
			m_CurrentPlayer = m_FirstPlayer;
		}

		public int GetBoardSize()
		{
			return m_Board.Size;
		}

		public int GetPlayerScore(int i_PlayerNum)
		{
			int playerScore = m_FirstPlayer.Score;

			if (i_PlayerNum == 2)
			{
				playerScore = m_SecondPlayer.Score;
			}

			return playerScore;
		}

		public eSymbols GetBoardSquareSymbol(Coordinate i_CurrentCoordinate)
		{
			return m_Board.GetBoardCellSymbol(i_CurrentCoordinate);
		}

		public bool TryMove(Move i_Move)
		{
			bool validMove = false;

			if (m_CurrentPlayer.IsAvaibleMove(i_Move))
			{
				Soldier soldierToMove = m_Board.GetSoliderAt(i_Move.FromCoordinate);
				bool wasKing = soldierToMove.IsKing();
				Move.eMoveResult moveResult = soldierToMove.MoveTo(i_Move.ToCoordinate);

				if (moveResult != Move.eMoveResult.Invalid)
				{
					OnEventOccured(new SoldierMovedEvent(i_Move));
					validMove = true;
					m_LastMove = i_Move;
					if (moveResult == Move.eMoveResult.Skipping)
					{
						removeSkippedSoldier(i_Move);
					}

					if (!wasKing && soldierToMove.IsKing())
					{
						OnEventOccured(new BecameKingEvent(soldierToMove.Symbol, soldierToMove.Position));
					}

					calcPlayersAviableMoves();
					if (moveResult == Move.eMoveResult.Skipping && soldierToMove.IsAbleToSkip())
					{
						m_PreviousPlayer = m_CurrentPlayer;
					}
					else
					{
						switchPlayerTurn();
					}

					if (m_CurrentPlayer.Kind == Player.ePlayerKind.Computer)
					{
						makeComputerMove();
					}
				}
			}

			return validMove;
		}

		private void OnEventOccured(IGameLogicsEvent i_Event)
		{
			if (EventOccured != null)
			{
				EventOccured.Invoke(i_Event);
			}
		}

		private void calcPlayersAviableMoves()
		{
			m_FirstPlayer.CalcAvailbleMoves();
			m_SecondPlayer.CalcAvailbleMoves();
		}

		private void makeComputerMove()
		{
			int moveIndex = 0;

			if (m_CurrentPlayer.Kind == Player.ePlayerKind.Computer)
			{
				Random random = new Random();
				List<Move> aviableMoves = m_CurrentPlayer.Moves;

				if (aviableMoves.Count > 0)
				{
					moveIndex = random.Next(0, aviableMoves.Count - 1);
					TryMove(aviableMoves[moveIndex]);
				}
			}
		}

		private void removeSkippedSoldier(Move i_Move)
		{
			int skippedSoldierRow = 0;
			int skippedSoldierColumn = 0;

			if (m_Board.IsValidCoordinate(i_Move.FromCoordinate) &&
				m_Board.IsValidCoordinate(i_Move.ToCoordinate))
			{
				if (i_Move.FromCoordinate.Row > i_Move.ToCoordinate.Row)
				{
					skippedSoldierRow = i_Move.FromCoordinate.Row - 1;
				}
				else
				{
					skippedSoldierRow = i_Move.FromCoordinate.Row + 1;
				}

				if (i_Move.FromCoordinate.Column > i_Move.ToCoordinate.Column)
				{
					skippedSoldierColumn = i_Move.FromCoordinate.Column - 1;
				}
				else
				{
					skippedSoldierColumn = i_Move.FromCoordinate.Column + 1;
				}

				Coordinate skippedSoldierCoordiante = new Coordinate(skippedSoldierRow, skippedSoldierColumn);

				if (m_Board.IsValidCoordinate(skippedSoldierCoordiante))
				{
					Soldier skippedSoldier = m_Board.GetSoliderAt(skippedSoldierCoordiante);

					if (skippedSoldier != null)
					{
						skippedSoldier.Delete();
						OnEventOccured(new SoldierRemovedEvent(skippedSoldier.Position));
					}
				}
			}
		}

		private void switchPlayerTurn()
		{
			m_PreviousPlayer = m_CurrentPlayer;
			if (m_CurrentPlayer == m_FirstPlayer)
			{
				m_CurrentPlayer = m_SecondPlayer;
			}
			else
			{
				m_CurrentPlayer = m_FirstPlayer;
			}
		}

		public bool IsCurrentPlayerSoldier(Coordinate i_Coordinate)
		{
			Soldier soldier = m_Board.GetSoliderAt(i_Coordinate);
			bool isCurrentPlayerSoldier = false;

			switch (m_CurrentPlayer.Symbol)
			{
				case eSymbols.X:
					isCurrentPlayerSoldier = soldier.Symbol == eSymbols.X || soldier.Symbol == eSymbols.K;
					break;
				case eSymbols.O:
					isCurrentPlayerSoldier = soldier.Symbol == eSymbols.O || soldier.Symbol == eSymbols.U;
					break;
			}

			return isCurrentPlayerSoldier;
		}

		internal void EnableMoving()
		{
			throw new NotImplementedException();
		}

		public bool IsGameEnded()
		{
			bool isGameEnded = false;

			if (m_CurrentPlayer == m_FirstPlayer && !m_FirstPlayer.HasAvailbleMoves())
			{
				isGameEnded = true;
				addScoresToWinner(m_SecondPlayer, m_FirstPlayer);
			}
			else if (m_CurrentPlayer == m_SecondPlayer && !m_SecondPlayer.HasAvailbleMoves())
			{
				isGameEnded = true;
				addScoresToWinner(m_FirstPlayer, m_SecondPlayer);
			}

			return isGameEnded;
		}

		private void addScoresToWinner(Player i_WinnerPlayer, Player i_LosingPlayer)
		{
			int score = i_WinnerPlayer.SoldiersValue - i_LosingPlayer.SoldiersValue;

			if (score > 0)
			{
				i_WinnerPlayer.Score += score;
			}
		}

		public Player CurrentPlayer
		{
			get
			{
				return m_CurrentPlayer;
			}
		}

		public Player PreviousPlayer
		{
			get
			{
				return m_PreviousPlayer;
			}
		}
	}
}
