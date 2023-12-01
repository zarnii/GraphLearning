using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GraphApp.Command
{
	/// <summary>
	/// Команда.
	/// </summary>
	public class RelayCommand : ICommand
	{
		#region private field
		/// <summary>
		/// Выполняемая команда.
		/// </summary>
		private Action<object> _execute;

		/// <summary>
		/// Возможность выполнения команды.
		/// </summary>
		private Func<object, bool> _canExecute;
		#endregion


		#region events
		/// <summary>
		/// Изменение условий, указывающий, может ли команда выполняться.
		/// </summary>
		public event EventHandler? CanExecuteChanged;
		#endregion


		#region constructor
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="execute">Команда.</param>
		/// <param name="canExecute">Функция, определяющая возможность выполнения команды.</param>
		public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
		{
			_execute = execute;
			_canExecute = canExecute;
		}
		#endregion


		#region public methods
		/// <summary>
		/// Определения возможности выполнения команды.
		/// </summary>
		/// <param name="parameter">Параметр команды.</param>
		/// <returns>True, если команда может выполниться.</returns>
		public bool CanExecute(object? parameter)
		{
			return this._canExecute == null || this._canExecute(parameter);
			
		}

		/// <summary>
		/// Выполнение команды.
		/// </summary>
		/// <param name="parameter">Параметр команды.</param>
		public void Execute(object? parameter)
		{
			_execute?.Invoke(parameter);
		}
		#endregion
	}
}
