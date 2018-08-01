using System;
using System.Collections.Generic;

namespace CheckersGame
{
	public class Player
	{
		public enum ePlayerKind
		{
			Human,
			Computer,
		}

		public const int k_MaxPlayerNameLength = 20;
		private const int k_KingSoldierValue = 4;
		private readonly List<Move> r_AvailbleMoves = new List<Move>();
		private readonly List<Soldier> r_SoldiersList = new List<Soldier>();
		private string m_Name;
		private int m_Score;
		private ePlayerKind m_Kind;
		private eSymbols m_Symbol;

		public List<Move> Moves
		{
			get
			{
				return r_AvailbleMoves;
			}
		}

		public string Name
		{
			get
			{
				return m_Name;
			}

			set
			{
				if (value.Contains(" "))
				{
					throw new PlayerNameWithSpacesException();
				}
				else if (value.Length > k_MaxPlayerNameLength)
				{
					throw new PlayerNameTooLongException();
				}

				m_Name = value;
			}
		}

		public List<Soldier> ResetSoldiersList(int i_SoldiersCount)
		{
			r_SoldiersList.Clear();
			for (int i = 0; i < i_SoldiersCount; i++)
			{
				Soldier newSoldier = new Soldier(this);

				r_SoldiersList.Add(newSoldier);
			}

			return r_SoldiersList;
		}

		public int SoldiersValue
		{
			get
			{
				int soldiersValue = 0;

				foreach (Soldier solider in r_SoldiersList)
				{
					if (solider.IsKing())
					{
						soldiersValue += k_KingSoldierValue;
					}
					else
					{
						soldiersValue++;
					}
				}

				return soldiersValue;
			}
		}

		public int Score
		{
			get
			{
				return m_Score;
			}

			set
			{
				if (value < 0)
				{
					throw new NegetiveScoreException();
				}

				m_Score = value;
			}
		}

		public ePlayerKind Kind
		{
			get
			{
				return m_Kind;
			}

			set
			{
				m_Kind = value;
			}
		}

		public eSymbols Symbol
		{
			get
			{
				return m_Symbol;
			}

			set
			{
				m_Symbol = value;
			}
		}

		public bool IsAvaibleMove(Move i_Move)
		{
			return r_AvailbleMoves.Contains(i_Move);
		}

		public bool HasAvailbleMoves()
		{
			return r_AvailbleMoves.Count != 0;
		}

		public void CalcAvailbleMoves()
		{
			bool anySoldierHasSkippingMove = false;

			r_AvailbleMoves.Clear();
			foreach (Soldier soldier in r_SoldiersList)
			{
				List<Move> soldierAvailbleMoves = soldier.CalcSoldierAvaibleMoves();

				if (anySoldierHasSkippingMove)
				{
					if (soldier.IsAbleToSkip())
					{
						r_AvailbleMoves.AddRange(soldierAvailbleMoves);
					}
				}
				else
				{
					if (soldier.IsAbleToSkip())
					{
						anySoldierHasSkippingMove = true;
						r_AvailbleMoves.Clear();
					}

					r_AvailbleMoves.AddRange(soldierAvailbleMoves);
				}
			}
		}

		public void RemoveSoldier(Soldier i_RemovedSoldier)
		{
			r_SoldiersList.Remove(i_RemovedSoldier);
		}
	}
}
