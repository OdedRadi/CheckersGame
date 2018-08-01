using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CheckersGame.Logics.Events;

namespace CheckersGame.UserInterface
{
	public class PanelCheckers : Panel
	{
		public event GameEndedEventHanlder GameEnded;

		private readonly GameEngine r_GameEngine;
		private readonly Queue<IGameLogicsEvent> r_LogicsEventsQueue;
		private readonly PictureBoxSoldier[,] r_PictureBoxSoldiersMatrix;
		private PictureBoxSoldier m_SelectedSoldier = null;

		public PanelCheckers(Settings i_Settings)
		{
			r_GameEngine = new GameEngine(i_Settings.Player1, i_Settings.Player2, i_Settings.BoardSize);
			r_GameEngine.EventOccured += gameEngine_EventOccured;
			r_PictureBoxSoldiersMatrix = new PictureBoxSoldier[i_Settings.BoardSize, i_Settings.BoardSize];
			r_LogicsEventsQueue = new Queue<IGameLogicsEvent>();
			initializeBoard();
			Size = new Size(i_Settings.BoardSize * PictureBoxTile.TileSize, i_Settings.BoardSize * PictureBoxTile.TileSize);
		}

		private void initializeBoard()
		{
			int boardSize = r_GameEngine.GetBoardSize();

			for (int i = 0; i < boardSize; i++)
			{
				for (int j = 0; j < boardSize; j++)
				{
					PictureBoxTile newPictureBoxTile = new PictureBoxTile(new Coordinate(i, j));

					newPictureBoxTile.Click += tile_Click;
					Controls.Add(newPictureBoxTile);
				}
			}
		}

		public void Restart()
		{
			r_GameEngine.Reset();
			removeSoldiersControls();
			restartSoldiers();
		}

		private void removeSoldiersControls()
		{
			for (int i = Controls.Count - 1; i >= 0; i--)
			{
				if (Controls[i] is PictureBoxSoldier)
				{
					Controls.Remove(Controls[i]);
				}
			}
		}

		private void restartSoldiers()
		{
			int boardSize = r_GameEngine.GetBoardSize();

			for (int i = 0; i < boardSize; i++)
			{
				for (int j = 0; j < boardSize; j++)
				{
					Coordinate currentCoordinate = new Coordinate(i, j);
					eSymbols symbol = r_GameEngine.GetBoardSquareSymbol(currentCoordinate);

					r_PictureBoxSoldiersMatrix[i, j] = null;
					if (symbol != eSymbols.None)
					{
						PictureBoxSoldier newPictureBoxSolider = new PictureBoxSoldier(symbol, currentCoordinate);

						newPictureBoxSolider.Click += soldier_Click;
						newPictureBoxSolider.AnimationFinished += soldier_AnimationFinished;
						Controls.Add(newPictureBoxSolider);
						r_PictureBoxSoldiersMatrix[i, j] = newPictureBoxSolider;
						newPictureBoxSolider.BringToFront();
					}
				}
			}
		}

		private void soldier_Click(object sender, EventArgs e)
		{
			PictureBoxSoldier clickedSoldier = sender as PictureBoxSoldier;

			if (clickedSoldier != null)
			{
				if (r_LogicsEventsQueue.Count == 0 &&
					r_GameEngine.IsCurrentPlayerSoldier(clickedSoldier.Coordinate))
				{
					if (m_SelectedSoldier == clickedSoldier)
					{
						m_SelectedSoldier.SetSelected(false);
						m_SelectedSoldier = null;
					}
					else
					{
						if (m_SelectedSoldier != null)
						{
							m_SelectedSoldier.SetSelected(false);
						}

						clickedSoldier.SetSelected(true);
						m_SelectedSoldier = clickedSoldier;
					}
				}
				else
				{
					clickedSoldier.BlinkRed();
				}
			}
		}

		private void tile_Click(object sender, EventArgs e)
		{
			PictureBoxTile clickedTile = sender as PictureBoxTile;
			bool legalMove = false;

			if (m_SelectedSoldier != null)
			{
				Move move = new Move(m_SelectedSoldier.Coordinate, clickedTile.Coordinate);

				legalMove = r_GameEngine.TryMove(move);
				if (r_GameEngine.IsGameEnded())
				{
					OnGameEnded();
				}
			}

			if (!legalMove)
			{
				clickedTile.BlinkRed();
			}
		}

		private void gameEngine_EventOccured(IGameLogicsEvent i_Event)
		{
			bool queueWasEmpty = r_LogicsEventsQueue.Count == 0;

			r_LogicsEventsQueue.Enqueue(i_Event);
			if (queueWasEmpty)
			{
				proceedEvent(r_LogicsEventsQueue.Peek());
			}
		}

		private void soldier_AnimationFinished()
		{
			r_LogicsEventsQueue.Dequeue();
			if (r_LogicsEventsQueue.Count != 0)
			{
				proceedEvent(r_LogicsEventsQueue.Peek());
			}
		}

		private void proceedEvent(IGameLogicsEvent i_Event)
		{
			SoldierMovedEvent soldierMovedEvent = i_Event as SoldierMovedEvent;
			BecameKingEvent becameKingEvent = i_Event as BecameKingEvent;
			SoldierRemovedEvent soldierRemovedEvent = i_Event as SoldierRemovedEvent;

			if (soldierMovedEvent != null)
			{
				moveSoldier(soldierMovedEvent.Move);
			}
			else if (becameKingEvent != null)
			{
				becomeKing(becameKingEvent.Symbol, becameKingEvent.Coordinate);
			}
			else if (soldierRemovedEvent != null)
			{
				removeSoldier(soldierRemovedEvent.Coordinate);
			}
		}

		private void moveSoldier(Move i_Move)
		{
			PictureBoxSoldier pictureboxSoldier = r_PictureBoxSoldiersMatrix[i_Move.FromCoordinate.Row, i_Move.FromCoordinate.Column];

			r_PictureBoxSoldiersMatrix[i_Move.ToCoordinate.Row, i_Move.ToCoordinate.Column] = pictureboxSoldier;
			r_PictureBoxSoldiersMatrix[i_Move.FromCoordinate.Row, i_Move.FromCoordinate.Column] = null;
			m_SelectedSoldier = null;
			pictureboxSoldier.MoveTo(i_Move.ToCoordinate);
		}

		private void becomeKing(eSymbols i_Symbol, Coordinate i_Coordinate)
		{
			PictureBoxSoldier pictureboxSoldier = r_PictureBoxSoldiersMatrix[i_Coordinate.Row, i_Coordinate.Column];

			pictureboxSoldier.BecomeKing(i_Symbol);
		}

		private void removeSoldier(Coordinate i_Coordinate)
		{
			PictureBoxSoldier pictureboxSoldier = r_PictureBoxSoldiersMatrix[i_Coordinate.Row, i_Coordinate.Column];

			pictureboxSoldier.Disapear();
			r_PictureBoxSoldiersMatrix[i_Coordinate.Row, i_Coordinate.Column] = null;
		}

		private void OnGameEnded()
		{
			if (GameEnded != null)
			{
				GameEnded.Invoke(r_GameEngine.PreviousPlayer.Name, r_GameEngine.GetPlayerScore(1), r_GameEngine.GetPlayerScore(2));
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			foreach (Control control in Controls)
			{
				PictureBox pictureBox = control as PictureBox;

				if (pictureBox != null)
				{
					e.Graphics.DrawImage(pictureBox.Image, pictureBox.Location);
				}
			}
		}
	}
}
