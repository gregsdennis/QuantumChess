using System;
using System.Collections.Generic;
using System.Windows.Input;
using QuantumChess.App.Framework;

namespace QuantumChess.App.Model
{
	public class QuantumCell : ViewModelBase
	{
		private bool _isSelected;

		public List<PieceProbability> Pieces { get; }
		public SquareColor Color { get; set; }

		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				if (value == _isSelected) return;
				_isSelected = value;
				NotifyOfPropertyChange();
				if (value) Selected?.Invoke(this, new EventArgs());
			}
		}

		public ICommand ToggleSelection { get; }

		public event EventHandler Selected;

		public QuantumCell()
		{
			Pieces = new List<PieceProbability>();
			ToggleSelection = new SimpleCommand(() => IsSelected = !IsSelected);
		}
	}
}