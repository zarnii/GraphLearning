using GraphApp.Command;
using System;
using System.Windows.Input;
using System.Windows.Controls;
using GraphApp.Interfaces;
using GraphApp.View;

namespace GraphApp.ViewModel
{
	/// <summary>
	/// Модель представления главного меню.
	/// </summary>
	public class MainMenuViewModel
	{
		#region fields
		/// <summary>
		/// Сервис навигации.
		/// </summary>
		private INavigationService _navigationService;

		/// <summary>
		/// Команда открытия окна редактора.
		/// </summary>
		private ICommand _openVisualEditor;

		/// <summary>
		/// Команда открытия окна обучения.
		/// </summary>
		private ICommand _openLearnLevels;
		#endregion

		#region properties
		/// <summary>
		/// Команда открытия окна редактора.
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

		/// <summary>
		/// Команда открытия окна обучения.
		/// </summary>
		public ICommand OpenLearnLevels
		{
			get
			{
				return _openLearnLevels;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value), "Пустая команда открытия окна");
				}

				_openLearnLevels = value;
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
			OpenVisualEditor = new RelayCommand(OpenVisualEditorCommand);
			OpenLearnLevels = new RelayCommand(OpenLearnLevelsCommand);
		}
		#endregion

		#region private methods
		/// <summary>
		/// Открытие окна редактора.
		/// </summary>
		/// <param name="parameter"></param>
		private void OpenVisualEditorCommand(object parameter)
		{
			_navigationService.NavigateTo<VisualEditorWindow>();
		}

		/// <summary>
		/// Открытие окна обучения.
		/// </summary>
		/// <param name="parameter"></param>
		private void OpenLearnLevelsCommand(object parameter)
		{
			_navigationService.NavigateTo<LearnLevelsWindow>();
		}
		#endregion
	}
}
