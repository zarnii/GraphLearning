using GraphApp.Command;
using GraphApp.Interfaces;
using GraphApp.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphApp.ViewModel
{
	public class QuestionViewModel : ViewModel, INotifyPropertyChanged
	{
		#region fields
		private string _correctColorHex = "#145210";

		private string _incorrectColorHex = "#891b21";

		private string _defaultColorHex = "#FF252526";

		private double _defaultOpacity = 0.3;

		private SolidColorBrush _correctColor;

		private SolidColorBrush _incorrectColor;

		private SolidColorBrush _defaultColor;

		/// <summary>
		/// Цвет фона ответа.
		/// </summary>
		private Brush _listBoxColor;

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
		/// Вопрос.
		/// </summary>
		public Question Question { get; private set; }

		/// <summary>
		/// Выбранный ответ.
		/// </summary>
		public Answer SelectedAnswer { get; set; }

		/// <summary>
		/// Цвет фона фариантов ответа.
		/// </summary>
		public Brush ListBoxColor
		{
			get
			{
				return _listBoxColor;
			}
			set
			{
				_listBoxColor = value;
				OnPropertyChanged();
			}
		}
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

			CheckAnswer = new RelayCommand(CheckAnswerCommand);
			OpenLearnLevels = new RelayCommand(OpenLearnLevelsCommand);
			Question = questionService.CurrentQuestion;

			_incorrectColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_incorrectColorHex));
			_correctColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_correctColorHex));
			_defaultColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_defaultColorHex));

			_incorrectColor.Opacity = _defaultOpacity;
			_correctColor.Opacity = _defaultOpacity;

			ListBoxColor = _defaultColor;
		}
		#endregion

		#region private methods
		/// <summary>
		/// Проверка ответа.
		/// </summary>
		/// <param name="parameter"></param>
		private void CheckAnswerCommand(object parameter)
		{
			if (SelectedAnswer == null)
			{
				return;
			}

			if (SelectedAnswer.Flag)
			{
				ListBoxColor = _correctColor;
			}
			else
			{
				ListBoxColor = _incorrectColor;
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
