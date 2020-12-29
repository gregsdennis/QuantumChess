using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace QuantumChess.App.Model
{
	public class Board
	{
		// List<T>.Enumerator is a struct, but I need class enumerators so that I can advance
		// them manually.  The enumerator returned by arrays is a class, so we use that.
		public Piece[] Pieces { get; private set; }

		private Board(){ }

		public static Board CreateNew()
		{
			return new Board
			{
				Pieces = new[]
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
				}
			};
		}

		public int DuplicationCount { get; private set; } = 1;

		public Board PerformMove(int sourceRow, int sourceCol, int targetRow, int targetCol, PieceColor turn)
		{
			var piece = Pieces.FirstOrDefault(p => p.Row == sourceRow && p.Column == sourceCol && p.IsPlayable);
			if (piece == null || piece.Color != turn)
			{
				DuplicationCount++;
				return null;
			}

			var victim = Pieces.FirstOrDefault(p => p.Row == targetRow && p.Column == targetCol);
			if (victim?.Color == piece.Color)
			{
				DuplicationCount++;
				return null;
			}

			var board = new Board
			{
				Pieces = Pieces.Select(p => p.Clone()).ToArray()
			};
			piece = board.Pieces.FirstOrDefault(p => p.Row == sourceRow && p.Column == sourceCol);
			victim = board.Pieces.FirstOrDefault(p => p.Row == targetRow && p.Column == targetCol);
			victim?.Capture();

			if (!CanMovePiece(piece, targetRow, targetCol))
			{
				DuplicationCount++;
				return null;
			}
			
			piece.Row = targetRow;
			piece.Column = targetCol;
			return board;
		}

		private bool CanMovePiece(Piece piece, int targetRow, int targetCol)
		{
			// return null if piece cannot move to target at all
			var potentialBlocks = piece.Kind switch
			{
				PieceKind.Pawn => GetPotentialBlocksForPawn(piece, targetRow, targetCol),
				PieceKind.Rook => GetPotentialBlocksForRook(piece, targetRow, targetCol),
				PieceKind.Knight => GetPotentialBlocksForKnight(piece, targetRow, targetCol),
				PieceKind.Bishop => GetPotentialBlocksForBishop(piece, targetRow, targetCol),
				PieceKind.Queen => GetPotentialBlocksForQueen(piece, targetRow, targetCol),
				PieceKind.King => GetPotentialBlocksForKing(piece, targetRow, targetCol),
				_ => null
			};

			if (potentialBlocks == null) return false;
 
			var blocks = Pieces.Join(potentialBlocks,
				p => (p.Row, p.Column),
				b => (b.Row, b.Column),
				(p, b) => b);
			return !blocks.Any();
		}

		private IEnumerable<(int Row, int Column)> GetPotentialBlocksForPawn(Piece piece, int targetRow, int targetCol)
		{
			var direction = (int) piece.Color;
			var homeRow = piece.Color == PieceColor.White ? 1 : 6;
			if (targetCol == piece.Column)
			{
				// this only works for white
				if (targetRow == piece.Row + direction) return new[] {(targetRow, targetCol)};
				if (targetRow == piece.Row + 2*direction && piece.Row == homeRow) return new[]
				{
					(piece.Row + direction, targetCol),
					(targetRow, targetCol)
				};
			}

			if (Math.Abs(targetCol - piece.Column) == 1)
			{
				// ordinarily don't look up piece collisions here, but this move is only
				// allowed during a capture, so we have to.
				if (targetRow == piece.Row + direction && Pieces.Any(p => p.Row == targetRow && p.Column == targetCol))
					return new (int Row, int Column)[] { };
			}

			return null;
		}

		private static List<(int Row, int Column)> GetPotentialBlocksForRook(Piece piece, int targetRow, int targetCol)
		{
			if (piece.Row != targetRow && piece.Column != targetCol) return null;

			var blocks = new List<(int Row, int Column)>();

			if (piece.Row == targetRow)
			{
				var direction = Math.Sign(targetCol - piece.Column);
				var i = piece.Column + direction;
				while (i != targetCol)
				{
					blocks.Add((targetRow, i));
					i += direction;
				}
			}
			else
			{
				var direction = Math.Sign(targetRow - piece.Row);
				var i = piece.Row + direction;
				while (i != targetRow)
				{
					blocks.Add((i, targetCol));
					i += direction;
				}
			}

			return blocks;
		}

		private static List<(int Row, int Column)> GetPotentialBlocksForKnight(Piece piece, int targetRow, int targetCol)
		{
			if ((Math.Abs(piece.Row - targetRow) == 1 && Math.Abs(piece.Column - targetCol) == 2) ||
			    (Math.Abs(piece.Row - targetRow) == 2 && Math.Abs(piece.Column - targetCol) == 1))
				return new List<(int Row, int Column)>();

			return null;
		}

		private List<(int Row, int Column)> GetPotentialBlocksForBishop(Piece piece, int targetRow, int targetCol)
		{
			return null;
		}

		private List<(int Row, int Column)> GetPotentialBlocksForQueen(Piece piece, int targetRow, int targetCol)
		{
			return null;
		}

		private List<(int Row, int Column)> GetPotentialBlocksForKing(Piece piece, int targetRow, int targetCol)
		{
			return null;
		}

		public void RevertDuplication()
		{
			DuplicationCount--;
		}
	}
}