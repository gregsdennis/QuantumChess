using System;
using QuantumChess.App.Framework;

namespace QuantumChess.App.Model
{
	public class Piece : ViewModelBase
	{
		public PieceColor Color { get; }
		public PieceKind Kind { get; }
		public int InitialPosition { get; }
		
		public bool IsPlayable { get; private set; }
		// row 0 rendered at bottom
		public int Row { get; set; }
		// col 0 rendered at left
		public int Column { get; set; }

		public Piece(PieceColor color, PieceKind kind, int startRow, int startColumn)
		{
			Color = color;
			Kind = kind;
			Row = startRow;
			Column = startColumn;
			IsPlayable = true;
			InitialPosition = startRow * 8 + startColumn;
		}

		public Piece(PieceColor color, PieceKind kind, int startRow, int startColumn, int initialPosition)
			: this(color, kind, startRow, startColumn)
		{
			InitialPosition = initialPosition;
		}

		public void Capture()
		{
			if (!IsPlayable)
				throw new InvalidOperationException("Piece is not playable.");

			IsPlayable = false;
		}

		public Piece Clone() => new Piece(Color, Kind, Row, Column, InitialPosition) {IsPlayable = IsPlayable};
	}
}
