using GraphApp.Command;
using System;
using System.Windows.Input;

namespace GraphApp.ViewModel
{
	/// <summary>
	/// Модель представления главного меню.
	/// </summary>
	public class MainMenuViewModel
	{
		/// <summary>
		/// Команда открытия страницы редактора.
		/// </summary>
		private ICommand _openVisualEditor;

		/// <summary>
		/// Команда открытия страницы редактора.
		/// </summary>
		public ICommand OpenVisualEditor
		{
			get
			{
				return _openVisualEditor;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value), "Пустая команда.");
				}

				_openVisualEditor = value;
			}
		}
	}
}
