using GraphApp.Command;
using GraphApp.Interfaces;
using System;
using System.Configuration;
using System.Windows.Input;

namespace GraphApp.ViewModel
{
	/// <summary>
	/// Модель представления окна с уроками.
	/// </summary>
	public class LearnLevelsViewModel: ViewModel
	{
		#region fields
		/// <summary>
		/// Сервис навигации.
		/// </summary>
		private INavigationService _navigationService;

		private IQuestionService _questionService;

		/// <summary>
		/// Команда открытия окна.
		/// </summary>
		private ICommand _openWindow;

		/// <summary>
		/// Команда перехода назад.
		/// </summary>
		private ICommand _goBack;
		#endregion

		#region properties
		/// <summary>
		/// Команда открытия окна.
		/// </summary>
		public ICommand OpenWindow
		{
			get
			{
				return _openWindow;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}

				_openWindow = value;
			}
		}

		/// <summary>
		/// Команда перехода назад.
		/// </summary>
		public ICommand OpenQuestion
		{
			get
			{
				return _goBack;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value), "Пустая команда перехода назад.");
				}

				_goBack = value;
			}
		}
		#endregion

		#region constructor
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="navigationService">Сервис навигации.</param>
		public LearnLevelsViewModel(INavigationService navigationService, IQuestionService questionService)
		{
			_navigationService = navigationService;
			_questionService = questionService;

			OpenWindow = new RelayCommand(OpenWindowCommand);
			OpenQuestion = new RelayCommand(OpenQuestionCommand);
		}
		#endregion

		#region private methods
		/// <summary>
		/// Открытие окна.
		/// </summary>
		/// <param name="parameter">Тип окна.</param>
		private void OpenWindowCommand(object parameter)
		{
			_navigationService.NavigateTo((Type)parameter, this);
		}

		private void OpenQuestionCommand(object parameter) 
		{
			_questionService.CurrentQuestionPathKey = (string)parameter;
			_navigationService.NavigateTo<QuestionViewModel>(null);
		}
		#endregion

		#region public methods
		#endregion
	}
}
