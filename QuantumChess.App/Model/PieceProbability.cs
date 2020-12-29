namespace QuantumChess.App.Model
{
	public class PieceProbability
	{
		public Piece Piece { get; set; }
		public long Count { get; set; }
		public long Potential { get; set; }

		public decimal Probability => Count / (decimal) Potential;
	}
}