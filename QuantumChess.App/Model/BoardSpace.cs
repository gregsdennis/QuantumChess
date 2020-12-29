using System;
using System.Collections.Generic;
using System.Linq;
using QuantumChess.App.Framework;

namespace QuantumChess.App.Model
{
	public class BoardSpace : ViewModelBase
	{
		private List<QuantumCell> _cells;
		private QuantumCell _selected;
		private int _totalBoardCount;
		private int _distinctBoardCount;
		private PieceColor _turn;

		public List<Board> Space { get; } = new List<Board>{Board.CreateNew()};

		public List<QuantumCell> Cells
		{
			get => _cells;
			set
			{
				if (Equals(value, _cells)) return;
				Unsubscribe();
				_cells = value;
				Subscribe();
				NotifyOfPropertyChange();
			}
		}

		public int TotalBoardCount
		{
			get => _totalBoardCount;
			set
			{
				if (value == _totalBoardCount) return;
				_totalBoardCount = value;
				NotifyOfPropertyChange();
			}
		}

		public int DistinctBoardCount
		{
			get => _distinctBoardCount;
			set
			{
				if (value == _distinctBoardCount) return;
				_distinctBoardCount = value;
				NotifyOfPropertyChange();
			}
		}

		public PieceColor Turn
		{
			get => _turn;
			set
			{
				if (value == _turn) return;
				_turn = value;
				NotifyOfPropertyChange();
			}
		}

		public int Turns { get; set; }

		public BoardSpace()
		{
			PopulateCells();
			_turn = PieceColor.White;
		}

		public void PopulateCells()
		{
			var quantumBoard = Enumerable.Range(0, 64).Select(i => new QuantumCell
				{
					Color = (SquareColor) ((i + i / 8) % 2)
				})
				.ToList();
			var enumerators = Space.Select(b => new {Enumerator = b.Pieces.GetEnumerator(), b.DuplicationCount}).ToList();
			TotalBoardCount = Space.Sum(b => b.DuplicationCount);
			DistinctBoardCount = Space.Count;
			for (int i = 0; i < 32; i++)
			{
				foreach (var enumerator in enumerators)
				{
					enumerator.Enumerator.MoveNext();
					var current = (Piece) enumerator.Enumerator.Current;
					if (current == null || !current.IsPlayable) continue;

					var cell = current.Row * 8 + current.Column;
					var pieceSet = quantumBoard[cell].Pieces;
					var piece = pieceSet.FirstOrDefault(p => p.Piece.InitialPosition == current.InitialPosition);
					if (piece == null)
						pieceSet.Add(new PieceProbability
						{
							Piece = current,
							Count = enumerator.DuplicationCount,
							Potential = TotalBoardCount
						});
					else
						piece.Count += enumerator.DuplicationCount;
				}
			}

			Cells = quantumBoard;
		}

		private void Unsubscribe()
		{
			Cells?.Apply(c => c.Selected -= UpdateSelection);
		}

		private void Subscribe()
		{
			Cells?.Apply(c => c.Selected += UpdateSelection);
		}

		private void UpdateSelection(object sender, EventArgs e)
		{
			if (_selected != null && _selected.IsSelected && _selected.Pieces.Count != 0)
			{
				var targetCell = (QuantumCell) sender;
				var targetIndex = Cells.IndexOf(targetCell);
				var targetRow = targetIndex / 8;
				var targetCol = targetIndex % 8;
				var sourceIndex = Cells.IndexOf(_selected);
				var sourceRow = sourceIndex / 8;
				var sourceCol = sourceIndex % 8;

				if (_selected.Pieces.Any(p => p.Piece.Color == Turn))
				{
					var newBoards = Space.Select(b => b.PerformMove(sourceRow, sourceCol, targetRow, targetCol, _turn))
						.Where(b => b != null)
						.ToList();

					if (newBoards.Any())
					{
						Space.AddRange(newBoards);
						PopulateCells();
						sender = null;
						Turn = Turn == PieceColor.White ? PieceColor.Black : PieceColor.White;
						Turns++;
					}
					else
					{
						Space.Apply(b => b.RevertDuplication());
					}
				}
			}

			_selected = (QuantumCell) sender;
			foreach (var cell in Cells)
			{
				if (!ReferenceEquals(cell, sender))
					cell.IsSelected = false;
			}
		}
	}
}