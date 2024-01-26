using GraphApp.Interfaces;
using GraphApp.Model;
using System.Windows.Input;
using System;
using GraphApp.Command;
using System.Windows;

namespace GraphApp.ViewModel
{
	public class QuestionViewModel : ViewModel
	{
		#region fields
		/// <summary>
		/// Выбранный флаг ответа.
		/// </summary>
		private bool _selectedFlag;

		/// <summary>
		/// Команда выбора RadioButton.
		/// </summary>
		private ICommand _selectRadioButton;

		/// <summary>
		/// Команда проверки ответа.
		/// </summary>
		private ICommand _checkAnswer;

		/// <summary>
		/// Команда открытия LearnLevels.
		/// </summary>
		private ICommand _openLearnLevels;

		/// <summary>
		/// Сервис навигации.
		/// </summary>
		private INavigationService _navigationService;
		#endregion

		#region properties
		/// <summary>
		/// Команда выбора RadioButton.
		/// </summary>
		public ICommand SelectRadioButton
		{
			get
			{
				return _selectRadioButton;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value), "Пустая команда выбора RadioButton");
				}

				_selectRadioButton = value;
			}
		}

		/// <summary>
		/// Команда проверки ответа.
		/// </summary>
		public ICommand CheckAnswer
		{
			get
			{
				return _checkAnswer;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value), "Пустая команда выбора проверки ответа.");
				}

				_checkAnswer = value;
			}
		}

		/// <summary>
		/// Команда открытия LearnLevels.
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
					throw new ArgumentNullException(nameof(value), "Пустая команда открытия LearnLevels");
				}

				_openLearnLevels = value;
			}
		}

		/// <summary>
		/// Вопрос.
		/// </summary>
		public Question Question { get; private set; }
		#endregion

		#region constructor
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="questionService">Сервис вопросов.</param>
		/// <param name="navigationService">Сервис навигации.</param>
		public QuestionViewModel(IQuestionService questionService, INavigationService navigationService)
		{
			_navigationService = navigationService;
			Question = questionService.GetCurrentQuestion();

			SelectRadioButton = new RelayCommand(SelectRadioButtonCommand);
			CheckAnswer = new RelayCommand(CheckAnswerCommand);
			OpenLearnLevels = new RelayCommand(OpenLearnLevelsCommand);
		}
		#endregion

		#region private methods
		/// <summary>
		/// Выбор RadioButton.
		/// </summary>
		/// <param name="parameter"></param>
		private void SelectRadioButtonCommand(object parameter)
		{
			_selectedFlag = (bool)parameter;
		}

		/// <summary>
		/// Проверка ответа.
		/// </summary>
		/// <param name="parameter"></param>
		private void CheckAnswerCommand(object parameter)
		{
			if (_selectedFlag)
			{
				MessageBox.Show(
					"Ответ верный",
					"Ответ верный",
					MessageBoxButton.OK
				);
			}
			else
			{
				MessageBox.Show(
					"Ответ не верный",
					"Ответ не верный",
					MessageBoxButton.OK
				);
			}
		}

		/// <summary>
		/// Открытие LearnLevels.
		/// </summary>
		/// <param name="parameter"></param>
		private void OpenLearnLevelsCommand(object parameter)
		{
			_navigationService.NavigateTo<LearnLevelsViewModel>(null);
		}
		#endregion
	}
}
