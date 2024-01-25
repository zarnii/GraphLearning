using System;
using GraphApp.Command;
using GraphApp.Interfaces;
using System.Windows.Input;

namespace GraphApp.ViewModel
{
	/// <summary>
	/// Модель представления главного меню.
	/// </summary>
	public class MainMenuViewModel: ViewModel
	{
		#region fields
		/// <summary>
		/// Сервис навигации.
		/// </summary>
		private INavigationService _navigationService;

		/// <summary>
		/// Команда открытия окна.
		/// </summary>
		private ICommand _openVisualEditor;
		#endregion

		#region properties
		/// <summary>
		/// Команда открытия окна.
		/// </summary>
		public ICommand OpenWindow
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
		#endregion

		#region constructor
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="navigationService">Сервис навигации.</param>
		public MainMenuViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
			OpenWindow = new RelayCommand(OpenWindowCommand);
		}
		#endregion

		#region private methods
		/// <summary>
		/// Открытие окна.
		/// </summary>
		/// <param name="parameter">Открываемое окно</param>
		private void OpenWindowCommand(object parameter)
		{
			_navigationService.NavigateTo((Type)parameter, this);
		}
		#endregion
	}
}
