using GraphApp.Command;
using GraphApp.Interfaces;
using GraphApp.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;

namespace GraphApp.ViewModel
{
	public class TestViewModel : ViewModel, INotifyPropertyChanged
	{
		#region fields
		private Dictionary<Question, Answer> _selectedAnswerByQuestion;
		/// <summary>
		/// Команда проверки ответа.
		/// </summary>
		private ICommand _checkAnswer;

		/// <summary>
		/// Команда открытия LearnLevels.
		/// </summary>
		private ICommand _openLearnLevels;

		/// <summary>
		/// Команда выбора ответа.
		/// </summary>
		private ICommand _selectAnswer;

		/// <summary>
		/// Сервис навигации.
		/// </summary>
		private INavigationService _navigationService;

		/// <summary>
		/// Сервис проверки тестов.
		/// </summary>
		private ITestCheckService _answerCheckService;

		/// <summary>
		/// Событие изменения свойства.
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;
		#endregion

		#region properties
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
		/// Команда выбора ответа.
		/// </summary>
		public ICommand SelectAnswer
		{
			get
			{
				return _selectAnswer;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value), "Пустая команда выбора ответа.");
				}

				_selectAnswer = value;
			}
		}

		/// <summary>
		/// Вопрос.
		/// </summary>
		public Test CurrentTest { get; private set; }
        #endregion

        #region constructor
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="testProvider">Поставщик тестов.</param>
        /// <param name="navigationService">Сервис навигации.</param>
        /// <param name="answerCheckService">Сервис проверки теста.</param>
        public TestViewModel(
			ITestProvider testProvider, 
			INavigationService navigationService, 
			ITestCheckService answerCheckService)
		{
			_navigationService = navigationService;
			_answerCheckService = answerCheckService;
			_selectedAnswerByQuestion = new Dictionary<Question, Answer>();

			CheckAnswer = new RelayCommand(CheckAnswerCommand);
			OpenLearnLevels = new RelayCommand(OpenLearnLevelsCommand);
			SelectAnswer = new RelayCommand(SelectAnswerCommand);
			CurrentTest = testProvider.CurrentTest;
		}
		#endregion

		#region private methods
		/// <summary>
		/// Проверка ответа.
		/// </summary>
		/// <param name="parameter"></param>
		private void CheckAnswerCommand(object parameter)
		{
			_answerCheckService.VerifableTest = CurrentTest;
			_answerCheckService.SelectedAnswerByQuestion = _selectedAnswerByQuestion;
			_navigationService.NavigateTo<VerifyTestViewModel>();
		}

		/// <summary>
		/// Открытие LearnLevels.
		/// </summary>
		/// <param name="parameter"></param>
		private void OpenLearnLevelsCommand(object parameter)
		{
			_navigationService.NavigateTo<LearnLevelsViewModel>();
		}

		/// <summary>
		/// Выбор ответа.
		/// </summary>
		/// <param name="parameter">Выбранный ответ.</param>
		private void SelectAnswerCommand(object parameter)
		{
			// Костыль.
			// Надо как-то по другому сделать, но, честно, я хз как.
			var answer = VisualTreeHelper.GetParent((RadioButton)((RoutedEventArgs)parameter).Source);
			var question = (Question)((System.Windows.Controls.StackPanel)VisualTreeHelper.GetParent(answer)).DataContext;

			_selectedAnswerByQuestion[question] = (Answer)((System.Windows.Controls.ContentPresenter)answer).Content;
        }

		/// <summary>
		/// Оповещение подписчиков о изменении свойства.
		/// </summary>
		/// <param name="propertyName">Имя измененного свойсива.</param>
		private void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}
