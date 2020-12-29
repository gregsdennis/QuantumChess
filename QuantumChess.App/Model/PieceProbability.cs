namespace QuantumChess.App.Model
{
	public class PieceProbability
	{
		public Piece Piece { get; set; }
		public int Count { get; set; }
		public int Potential { get; set; }

		public decimal Probability => Count / (decimal) Potential;
	}
}