using System;
using System.Collections.Generic;

namespace CheckersGame
{
	public class Soldier
	{
		private enum eSoldierDirection
		{
			Up,
			Down,
			Both,
		}

		private Coordinate m_Position;
		private Player m_OwnerPlayer;
		private Board m_Board;
		private eSymbols m_Symbol;
		private eSoldierDirection m_SoldierDirection;
		private bool m_AbleToSkip;
		private bool m_IsKing;

		public Soldier(Player i_OwnerPlayer)
		{
			m_Symbol = i_OwnerPlayer.Symbol;
			m_OwnerPlayer = i_OwnerPlayer;
			if (m_Symbol == eSymbols.X)
			{
				m_SoldierDirection = eSoldierDirection.Up;
			}
			else if (m_Symbol == eSymbols.O)
			{
				m_SoldierDirection = eSoldierDirection.Down;
			}
		}

		public Coordinate Position
		{
			get
			{
				return m_Position;
			}

			set
			{
				m_Position = value;
			}
		}

		public eSymbols Symbol
		{
			get
			{
				return m_Symbol;
			}
		}

		public bool IsAbleToSkip()
		{
			return m_AbleToSkip;
		}

		public bool IsKing()
		{
			return m_IsKing;
		}

		public void SetBoard(Board i_Board)
		{
			m_Board = i_Board;
		}

		public Move.eMoveResult MoveTo(Coordinate i_ToCoordinate)
		{
			Move.eMoveResult moveResult = Move.eMoveResult.Invalid;

			if (m_Board.IsValidCoordinate(i_ToCoordinate))
			{
				int rowDelta = Math.Abs(m_Position.Row - i_ToCoordinate.Row);
				int colDelta = Math.Abs(m_Position.Column - i_ToCoordinate.Column);

				if (rowDelta == 1 && colDelta == 1)
				{
					moveResult = Move.eMoveResult.Moving;
				}
				else if (rowDelta == 2 && colDelta == 2)
				{
					moveResult = Move.eMoveResult.Skipping;
				}
			}

			if (moveResult != Move.eMoveResult.Invalid)
			{
				m_Board.UpdateSoldierPosition(m_Position, i_ToCoordinate);
				m_Position = i_ToCoordinate;
				if (m_Position.Row == 0 || m_Position.Row == m_Board.Size - 1)
				{
					becomeKing();
				}
			}

			return moveResult;
		}

		private void becomeKing()
		{
			if (!m_IsKing)
			{
				m_IsKing = true;
				m_SoldierDirection = eSoldierDirection.Both;
				if (m_Symbol == eSymbols.X)
				{
					m_Symbol = eSymbols.K;
				}
				else if (m_Symbol == eSymbols.O)
				{
					m_Symbol = eSymbols.U;
				}
			}
		}

		public List<Move> CalcSoldierAvaibleMoves()
		{
			List<Move> availableMovesList = new List<Move>();
			List<Coordinate> optionalMovingCoordinates = buildOptionalCoordinateList(1);
			List<Coordinate> optionalSkippingCoordinates = buildOptionalCoordinateList(2);

			m_AbleToSkip = false;
			for (int i = 0; i < optionalSkippingCoordinates.Count; i++)
			{
				Coordinate toCoordinate = optionalSkippingCoordinates[i];
				Coordinate betweenCoordinate = optionalMovingCoordinates[i];

				if (isAbleToSkip(betweenCoordinate, toCoordinate))
				{
					m_AbleToSkip = true;
					availableMovesList.Add(new Move(m_Position, toCoordinate));
				}
			}

			if (!m_AbleToSkip)
			{
				foreach (Coordinate toCoordinate in optionalMovingCoordinates)
				{
					if (m_Board.IsValidCoordinate(toCoordinate) &&
						m_Board.IsEmptyCell(toCoordinate))
					{
						availableMovesList.Add(new Move(m_Position, toCoordinate));
					}
				}
			}

			return availableMovesList;
		}

		private List<Coordinate> buildOptionalCoordinateList(int i_Distance)
		{
			List<Coordinate> optionalCoordinates = new List<Coordinate>();
			Coordinate upperRight = new Coordinate(m_Position.Row - i_Distance, m_Position.Column + i_Distance);
			Coordinate upperLeft = new Coordinate(m_Position.Row - i_Distance, m_Position.Column - i_Distance);
			Coordinate buttomRight = new Coordinate(m_Position.Row + i_Distance, m_Position.Column + i_Distance);
			Coordinate buttomLeft = new Coordinate(m_Position.Row + i_Distance, m_Position.Column - i_Distance);

			switch (m_SoldierDirection)
			{
				case eSoldierDirection.Both:
					optionalCoordinates.Add(upperRight);
					optionalCoordinates.Add(upperLeft);
					optionalCoordinates.Add(buttomRight);
					optionalCoordinates.Add(buttomLeft);
					break;
				case eSoldierDirection.Up:
					optionalCoordinates.Add(upperRight);
					optionalCoordinates.Add(upperLeft);
					break;
				case eSoldierDirection.Down:
					optionalCoordinates.Add(buttomRight);
					optionalCoordinates.Add(buttomLeft);
					break;
			}

			return optionalCoordinates;
		}
		
        private bool isAbleToSkip(Coordinate i_BetweenCoordinate, Coordinate i_ToCoordinate)
		{
			return m_Board.IsValidCoordinate(i_ToCoordinate) &&
					m_Board.IsValidCoordinate(i_BetweenCoordinate) &&
					m_Board.IsEmptyCell(i_ToCoordinate) &&
					!m_Board.IsEmptyCell(i_BetweenCoordinate) &&
					isEnemySoldier(m_Board.GetSoliderAt(i_BetweenCoordinate));
		}

		private bool isEnemySoldier(Soldier i_Soldier)
		{
			bool isEnemySoldier = false;

			if (m_Symbol == eSymbols.X || m_Symbol == eSymbols.K)
			{
				if (i_Soldier.Symbol == eSymbols.O || i_Soldier.Symbol == eSymbols.U)
				{
					isEnemySoldier = true;
				}
			}
			else if (m_Symbol == eSymbols.O || m_Symbol == eSymbols.U)
			{
				if (i_Soldier.Symbol == eSymbols.X || i_Soldier.Symbol == eSymbols.K)
				{
					isEnemySoldier = true;
				}
			}

			return isEnemySoldier;
		}

		public void Delete()
		{
			m_OwnerPlayer.RemoveSoldier(this);
			m_Board.RemoveSoliderAt(m_Position);
		}
	}
}
