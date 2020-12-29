using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace QuantumChess.App.Framework
{
	/// <summary>
	/// Decorates a collection of <see cref="ICommand"/>s into a single <see cref="ICommand"/> class.
	/// </summary>
	public class MultiCommand : ICommand
	{
		private readonly bool _useAndLogic;
		private readonly IEnumerable<ICommand> _commands;

		/// <summary>Occurs when changes occur that affect whether or not the command should execute.</summary>
		public event EventHandler CanExecuteChanged
		{
			add
			{
				foreach (var command in _commands)
				{
					command.CanExecuteChanged += value;
				}
			}
			remove
			{
				foreach (var command in _commands)
				{
					command.CanExecuteChanged -= value;
				}
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="useAndLogic">
		/// If true, all child commands must be able to execute for this command to execute.
		/// If false, at least one command must be able to execute for this command.  Only
		/// those child commands which can execute will be executed.
		/// </param>
		/// <param name="commands">The child commands</param>
		public MultiCommand(bool useAndLogic, params ICommand[] commands)
			: this(useAndLogic, (IEnumerable<ICommand>) commands) {}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="useAndLogic">
		/// If true, all child commands must be able to execute for this command to execute.
		/// If false, at least one command must be able to execute for this command.  Only
		/// those child commands which can execute will be executed.
		/// </param>
		/// <param name="commands">The child commands</param>
		public MultiCommand(bool useAndLogic, IEnumerable<ICommand> commands)
		{
			if (commands == null) throw new ArgumentNullException(nameof(commands));
			if (!commands.Any()) throw new ArgumentException("'commands' must be non-empty");

			_useAndLogic = useAndLogic;
			_commands = commands.ToList();
		}

		/// <summary>Defines the method that determines whether the command can execute in its current state.</summary>
		/// <returns>true if this command can be executed; otherwise, false.</returns>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		public bool CanExecute(object parameter)
		{
			return _useAndLogic
				       ? _commands.All(c => c.CanExecute(parameter))
				       : _commands.Any(c => c.CanExecute(parameter));
		}

		/// <summary>Defines the method to be called when the command is invoked.</summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		public void Execute(object parameter)
		{
			_commands.Where(c => c.CanExecute(parameter))
			         .Apply(c => c.Execute(parameter));
		}
	}
}
