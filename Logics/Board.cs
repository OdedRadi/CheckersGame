using System;
using System.Collections.Generic;

namespace CheckersGame
{
	public class Board
	{
		private Soldier[,] m_BoardState;
		private int m_Size;

		public Board(int i_BoardSize)
		{
			m_Size = i_BoardSize;
		}

		public int CalcAvailbleSoldiersCount()
		{
			int soldiersCount = 0;
			int soldiersRowsCount = (m_Size / 2) - 1;

			for (int i = 0; i < soldiersRowsCount; i++)
			{
				for (int j = 0; j < m_Size; j++)
				{
					if ((i + j) % 2 == 1)
					{
						soldiersCount++;
					}
				}
			}

			return soldiersCount;
		}

		public void Reset(List<Soldier> i_FirstPlayerSoldiersList, List<Soldier> i_SecondPlayerSoldiersList)
		{
			int soldiersRowsCount = (m_Size / 2) - 1;
			int soldierIndex = 0;

			m_BoardState = new Soldier[m_Size, m_Size];
			for (int i = 0; i < soldiersRowsCount; i++)
			{
				for (int j = 0; j < m_Size; j++)
				{
					if ((i + j) % 2 == 1)
					{
						i_SecondPlayerSoldiersList[soldierIndex].Position = new Coordinate(i, j);
						i_SecondPlayerSoldiersList[soldierIndex].SetBoard(this);
						m_BoardState[i, j] = i_SecondPlayerSoldiersList[soldierIndex];
						soldierIndex++;
					}
				}
			}

			soldierIndex = 0;
			for (int i = m_Size - 1; i >= m_Size - soldiersRowsCount; i--)
			{
				for (int j = 0; j < m_Size; j++)
				{
					if ((i + j) % 2 == 1)
					{
						i_FirstPlayerSoldiersList[soldierIndex].Position = new Coordinate(i, j);
						i_FirstPlayerSoldiersList[soldierIndex].SetBoard(this);
						m_BoardState[i, j] = i_FirstPlayerSoldiersList[soldierIndex];
						soldierIndex++;
					}
				}
			}
		}

		public int Size
		{
			get
			{
				return m_Size;
			}
		}

		public eSymbols GetBoardCellSymbol(Coordinate i_Coordinate)
		{
			eSymbols symbol = eSymbols.None;

			if (IsValidCoordinate(i_Coordinate))
			{
				if (!IsEmptyCell(i_Coordinate))
				{
					symbol = GetSoliderAt(i_Coordinate).Symbol;
				}
			}

			return symbol;
		}

		public bool IsValidCoordinate(Coordinate i_Coordinate)
		{
			bool isValidCoordinate = false;

			if (i_Coordinate.Row >= 0 && i_Coordinate.Row < m_Size &&
				i_Coordinate.Column >= 0 && i_Coordinate.Column < m_Size)
			{
				isValidCoordinate = true;
			}

			return isValidCoordinate;
		}

		public void RemoveSoliderAt(Coordinate i_Coordinate)
		{
			if (IsValidCoordinate(i_Coordinate))
			{
				m_BoardState[i_Coordinate.Row, i_Coordinate.Column] = null;
			}
		}

		public Soldier GetSoliderAt(Coordinate i_Coordinate)
		{
			Soldier returnedSoldier = null;

			if (IsValidCoordinate(i_Coordinate))
			{
				returnedSoldier = m_BoardState[i_Coordinate.Row, i_Coordinate.Column];
			}

			return returnedSoldier;
		}

		public void UpdateSoldierPosition(Coordinate i_FromCoordinate, Coordinate i_ToCoordinate)
		{
			if (IsValidCoordinate(i_FromCoordinate) && IsValidCoordinate(i_ToCoordinate))
			{
				Soldier movedSolider = GetSoliderAt(i_FromCoordinate);

				m_BoardState[i_ToCoordinate.Row, i_ToCoordinate.Column] = movedSolider;
				RemoveSoliderAt(i_FromCoordinate);
			}
		}

		public bool IsEmptyCell(Coordinate i_Coordinate)
		{
			bool isEmptyCell = false;

			if (IsValidCoordinate(i_Coordinate))
			{
				isEmptyCell = m_BoardState[i_Coordinate.Row, i_Coordinate.Column] == null;
			}

			return isEmptyCell;
		}
	}
}
