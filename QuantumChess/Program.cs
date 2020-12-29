using System;
using System.Collections.Generic;

namespace QuantumChess
{
	class Piece
	{
		public PieceColor Color { get; }
		public PieceKind Kind { get; }
		
		public bool IsPlayable { get; private set; }
		// row 0 rendered at bottom
		public int Row { get; private set; }
		// col 0 rendered at left
		public int Column { get; private set; }

		public Piece(PieceColor color, PieceKind kind, int startRow, int startColumn)
		{
			Color = color;
			Kind = kind;
			Row = startRow;
			Column = startColumn;
			IsPlayable = true;
		}

		public void Capture()
		{
			if (!IsPlayable)
				throw new InvalidOperationException("Piece is not playable.");

			IsPlayable = false;
		}

		public void Render()
		{
			if (Color == PieceColor.Black)
				Console.ForegroundColor = ConsoleColor.Black;
			else
				Console.ForegroundColor = ConsoleColor.White;

			Console.Write(Kind.ToString().Substring(0,2));
		}

		public object Clone() => new Piece(Color, Kind, Row, Column) {IsPlayable = IsPlayable};
	}

	enum PieceKind
	{
		Pawn,
		Rook,
		Knight,
		Bishop,
		Queen,
		King
	}

	enum PieceColor
	{
		White,
		Black
	}

	class Board
	{
		// keep the pieces sorted by row then column
		private readonly List<Piece> _pieces = new List<Piece>
		{
			new Piece(PieceColor.White, PieceKind.Rook, 0, 0),
			new Piece(PieceColor.White, PieceKind.Knight, 0, 1),
			new Piece(PieceColor.White, PieceKind.Bishop, 0, 2),
			new Piece(PieceColor.White, PieceKind.King, 0, 3),
			new Piece(PieceColor.White, PieceKind.Queen, 0, 4),
			new Piece(PieceColor.White, PieceKind.Bishop, 0, 5),
			new Piece(PieceColor.White, PieceKind.Knight, 0, 6),
			new Piece(PieceColor.White, PieceKind.Rook, 0, 7),

			new Piece(PieceColor.White, PieceKind.Pawn, 1, 0),
			new Piece(PieceColor.White, PieceKind.Pawn, 1, 1),
			new Piece(PieceColor.White, PieceKind.Pawn, 1, 2),
			new Piece(PieceColor.White, PieceKind.Pawn, 1, 3),
			new Piece(PieceColor.White, PieceKind.Pawn, 1, 4),
			new Piece(PieceColor.White, PieceKind.Pawn, 1, 5),
			new Piece(PieceColor.White, PieceKind.Pawn, 1, 6),
			new Piece(PieceColor.White, PieceKind.Pawn, 1, 7),

			new Piece(PieceColor.Black, PieceKind.Pawn, 6, 0),
			new Piece(PieceColor.Black, PieceKind.Pawn, 6, 1),
			new Piece(PieceColor.Black, PieceKind.Pawn, 6, 2),
			new Piece(PieceColor.Black, PieceKind.Pawn, 6, 3),
			new Piece(PieceColor.Black, PieceKind.Pawn, 6, 4),
			new Piece(PieceColor.Black, PieceKind.Pawn, 6, 5),
			new Piece(PieceColor.Black, PieceKind.Pawn, 6, 6),
			new Piece(PieceColor.Black, PieceKind.Pawn, 6, 7),

			new Piece(PieceColor.Black, PieceKind.Rook, 7, 0),
			new Piece(PieceColor.Black, PieceKind.Knight, 7, 1),
			new Piece(PieceColor.Black, PieceKind.Bishop, 7, 2),
			new Piece(PieceColor.Black, PieceKind.King, 7, 3),
			new Piece(PieceColor.Black, PieceKind.Queen, 7, 4),
			new Piece(PieceColor.Black, PieceKind.Bishop, 7, 5),
			new Piece(PieceColor.Black, PieceKind.Knight, 7, 6),
			new Piece(PieceColor.Black, PieceKind.Rook, 7, 7),
		};

		public void Render()
		{
			var pieceIndex = 0;
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					var cellIndex = i * 8 + j;
					Console.BackgroundColor = (cellIndex + i) % 2 == 0
						? ConsoleColor.Gray
						: ConsoleColor.DarkGray;

					if (pieceIndex < 32)
					{
						var piece = _pieces[pieceIndex];
						if (piece.Row == i && piece.Column == j)
						{
							piece.Render();
							pieceIndex++;
							continue;
						}
					}
					
					Console.Write("  ");
				}
				Console.WriteLine();
			}
		}
	}
}
