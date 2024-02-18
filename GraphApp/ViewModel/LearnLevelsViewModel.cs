using GraphApp.Command;
using GraphApp.Interfaces;
using GraphApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace GraphApp.ViewModel
{
	/// <summary>
	/// Модель представления окна с уроками.
	/// </summary>
	public class LearnLevelsViewModel : ViewModel
	{
		#region fields
		/// <summary>
		/// Сервис навигации.
		/// </summary>
		private INavigationService _navigationService;

		/// <summary>
		/// Сервис вопросов.
		/// </summary>
		private IQuestionService _questionService;

		/// <summary>
		/// Сервис user control.
		/// </summary>
		private ITheoryService _userControlService;

		/// <summary>
		/// Команда открытия окна.
		/// </summary>
		private ICommand _openWindow;

		/// <summary>
		/// Команда перехода назад.
		/// </summary>
		private ICommand _goBack;

		/// <summary>
		/// Команда открытия теории.
		/// </summary>
		private ICommand _openTheory;
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

		/// <summary>
		/// Команда открытия теории.
		/// </summary>
		public ICommand OpenTheory
		{
			get
			{
				return _openTheory;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value), "Пустая команда открытия теории.");
				}

				_openTheory = value;
			}
		}

		/// <summary>
		/// Коллекция вопросов.
		/// </summary>
		public List<Question> Questions
		{
			get
			{
				return (List<Question>)_questionService.Questions;
			}
		}

		/// <summary>
		/// Коллекция теории.
		/// </summary>
		public List<Theory> Theories
		{
			get
			{
				return _userControlService.TheoryControls;
			}
		}
		#endregion

		#region constructor
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="navigationService">Сервис навигации.</param>
		public LearnLevelsViewModel(INavigationService navigationService, 
			IQuestionService questionService, 
			ITheoryService userControlService)
		{
			_navigationService = navigationService;
			_questionService = questionService;
			_userControlService = userControlService;

			OpenWindow = new RelayCommand(OpenWindowCommand);
			OpenQuestion = new RelayCommand(OpenQuestionCommand);
			OpenTheory = new RelayCommand(OpenTheoryCommand);
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

		/// <summary>
		/// Открытие окна с вопросом.
		/// </summary>
		/// <param name="parameter">Открываемый вопрос.</param>
		private void OpenQuestionCommand(object parameter)
		{
			_questionService.CurrentQuestion = (Question)parameter;
			_navigationService.NavigateTo<QuestionViewModel>(null);
		}

		/// <summary>
		/// Открытия теории.
		/// </summary>
		/// <param name="parameter">Открываемый раздел теории.</param>
		private void OpenTheoryCommand(object parameter)
		{
			_userControlService.CurrentTheory = (Theory)parameter;
			_navigationService.NavigateTo<TheoryViewModel>(null);
		}
		#endregion

		#region public methods
		#endregion
	}
}
