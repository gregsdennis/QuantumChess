using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using QuantumChess.App.Framework;

namespace QuantumChess.App.Model
{
	public class BoardSpace : ViewModelBase
	{
		private List<QuantumCell> _cells;
		private long _totalBoardCount;
		private int _distinctBoardCount;
		private PieceColor _turn;
		private int _turns;
		private QuantumCell _selected;
		private bool _viewSingleBoard;
		private int _selectedBoardIndex = 1;
		private int _blackWinCount;
		private int _whiteWinCount;

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

		public long TotalBoardCount
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

		public int Turns
		{
			get => _turns;
			set
			{
				if (value == _turns) return;
				_turns = value;
				NotifyOfPropertyChange();
			}
		}

		public QuantumCell Selected
		{
			get => _selected;
			set
			{
				if (Equals(value, _selected)) return;
				_selected = value;
				NotifyOfPropertyChange();
			}
		}

		public bool ViewSingleBoard
		{
			get => _viewSingleBoard;
			set
			{
				if (value == _viewSingleBoard) return;
				_viewSingleBoard = value;
				PopulateBoard();
				NotifyOfPropertyChange();
			}
		}

		public int SelectedBoardIndex
		{
			get => _selectedBoardIndex;
			set
			{
				if (value == _selectedBoardIndex) return;
				_selectedBoardIndex = value;
				PopulateBoard();
				NotifyOfPropertyChange();
			}
		}

		public int BlackWinCount
		{
			get => _blackWinCount;
			set
			{
				if (value == _blackWinCount) return;
				_blackWinCount = value;
				NotifyOfPropertyChange();
				NotifyOfPropertyChange(nameof(BlackWinPercent));
			}
		}

		public int WhiteWinCount
		{
			get => _whiteWinCount;
			set
			{
				if (value == _whiteWinCount) return;
				_whiteWinCount = value;
				NotifyOfPropertyChange();
				NotifyOfPropertyChange(nameof(WhiteWinPercent));
			}
		}

		public decimal BlackWinPercent => BlackWinCount / (decimal) TotalBoardCount;
		public decimal WhiteWinPercent => WhiteWinCount / (decimal) TotalBoardCount;

		public ICommand Reset { get; }

		public BoardSpace()
		{
			PopulateBoard();
			_turn = PieceColor.White;

			Reset = new SimpleCommand(() =>
			{
				Space.Clear();
				Space.Add(Board.CreateNew());
				Turn = PieceColor.White;
				Turns = 0;
				PopulateBoard();
			});
		}

		private void PopulateBoard()
		{
			TotalBoardCount = Space.Sum(b => b.DuplicationCount);
			DistinctBoardCount = Space.Count;
			BlackWinCount = Space.Count(b => b.CapturedKingColor == PieceColor.White);
			WhiteWinCount = Space.Count(b => b.CapturedKingColor == PieceColor.Black);

			var quantumBoard = Enumerable.Range(0, 64).Select(i => new QuantumCell
				{
					Color = (SquareColor)((i + i / 8) % 2)
				})
				.ToList();

			if (ViewSingleBoard)
				PopulateSingleBoard(quantumBoard);
			else
				PopulateQuantumBoard(quantumBoard);
	
			Cells = quantumBoard;
		}

		private void PopulateSingleBoard(List<QuantumCell> quantumBoard)
		{
			var boardIndex = SelectedBoardIndex - 1; // uses 1-indexing because it uses the board counts for the range
			var board = Space[boardIndex];

			foreach (var piece in board.Pieces.Where(p => p.IsPlayable))
			{
				var index = piece.Row * 8 + piece.Column;
				quantumBoard[index].Pieces.Add(new PieceProbability
				{
					Piece = piece,
					Count = 1,
					Potential = 1
				});
			}
		}

		private void PopulateQuantumBoard(List<QuantumCell> quantumBoard)
		{
			var enumerators = Space.Select(b => new {Enumerator = b.Pieces.GetEnumerator(), b.DuplicationCount}).ToList();
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
			if (Selected != null && Selected.IsSelected && Selected.Pieces.Count != 0)
			{
				var targetCell = (QuantumCell) sender;
				var targetIndex = Cells.IndexOf(targetCell);
				var targetRow = targetIndex / 8;
				var targetCol = targetIndex % 8;
				var sourceIndex = Cells.IndexOf(Selected);
				var sourceRow = sourceIndex / 8;
				var sourceCol = sourceIndex % 8;

				if (Selected.Pieces.Any(p => p.Piece.Color == Turn))
				{
					var newBoards = Space.Select(b => b.PerformMove(sourceRow, sourceCol, targetRow, targetCol, _turn))
						.Where(b => b != null)
						.ToList();

					if (newBoards.Any())
					{
						Space.AddRange(newBoards);
						PopulateBoard();
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

			Selected = (QuantumCell) sender;
			foreach (var cell in Cells)
			{
				if (!ReferenceEquals(cell, sender))
					cell.IsSelected = false;
			}
		}
	}
}