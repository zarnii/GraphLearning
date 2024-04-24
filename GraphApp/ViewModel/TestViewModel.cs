using GraphApp.Command;
using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.Services;
using GraphApp.Services.FactoryViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphApp.ViewModel
{
    public class TestViewModel : ViewModel
    {
        #region fields
        /// <summary>
        /// Выбранные ответы по вопросам.
        /// </summary>
        private Dictionary<Question, Answer> _selectedAnswerByQuestion;

        /// <summary>
        /// Таймер.
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// Прозрачность таймера.
        /// </summary>
        private double _timerOpasity;

        /// <summary>
        /// Сервис контроля доступа.
        /// </summary>
        private IAccessControlService _accessControlService;

        /// <summary>
        /// Команда проверки ответа.
        /// </summary>
        private ICommand _checkAnswer;

        /// <summary>
        /// Команда открытия LearnLevels.
        /// </summary>
        private ICommand _openEducation;

        /// <summary>
        /// Команда выбора ответа.
        /// </summary>
        private ICommand _selectAnswer;

        /// <summary>
        /// Сервис навигации.
        /// </summary>
        private INavigationService _navigationService;

        /// <summary>
        /// Фабрика VerifyTestVM.
        /// </summary>
        private IFactoryViewModel _factoryVerifyTestVM;
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
        public ICommand OpenEducation
        {
            get
            {
                return _openEducation;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Пустая команда открытия LearnLevels");
                }

                _openEducation = value;
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

        /// <summary>
        /// Значение таймера.
        /// </summary>
        public TimeSpan? TimerValue
        {
            get
            {
                return _timer?.TimerValue;
            }
        }

        public double TimerOpasity
        {
            get
            {
                return _timerOpasity;
            }
            set
            {
                _timerOpasity = value;
                OnPropertyChanged(nameof(TimerOpasity));
            }
        }
        #endregion

        #region constructor
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="accessControlService">Поставщик тестов.</param>
        /// <param name="navigationService">Сервис навигации.</param>
        /// <param name="factoryVerifyTestVM">Фабрика FactoryVerifyTestViewModel.</param>
        public TestViewModel(
            IAccessControlService accessControlService,
            INavigationService navigationService,
            [FromKeyedServices(typeof(FactoryVerifyTestViewModel))] IFactoryViewModel factoryVerifyTestVM)
        {
            _accessControlService = accessControlService;
            _navigationService = navigationService;
            _factoryVerifyTestVM = factoryVerifyTestVM;
            _selectedAnswerByQuestion = new Dictionary<Question, Answer>();

            CheckAnswer = new RelayCommand(CheckAnswerCommand);
            OpenEducation = new RelayCommand(OpenEducationCommand);
            SelectAnswer = new RelayCommand(SelectAnswerCommand);
            CurrentTest = (Test)accessControlService.CurrentEducationMaterial.EducationMaterial;

            if (CurrentTest.LeadTime != null)
            {
                _timer = new Timer(
                    CurrentTest.LeadTime.Value,
                    CheckAnswerCommand,
                    "Время истекло!",
                    UpdateTimerValue
                );
                _timer.Start();
                TimerOpasity = 1;
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// Проверка ответа.
        /// </summary>
        /// <param name="parameter"></param>
        private void CheckAnswerCommand(object parameter)
        {
            _timer?.Stop();
            string message = string.Empty;

            if (parameter as string != null)
            {
                message = (string)parameter;
            }

            var verifyTestService = new VerifyTestService();
            var result = verifyTestService.VerifyTest(_selectedAnswerByQuestion, CurrentTest);

            if (!_accessControlService.CheckEducationMaterialIsPassed(_accessControlService.CurrentEducationMaterial))
            {
                _accessControlService.AddAttempt(_accessControlService.CurrentEducationMaterial);
            }

            if (result.Item1 == CurrentTest.Questions.Length)
            {
                _accessControlService.OpenNext(_accessControlService.CurrentEducationMaterial);
            }

            _navigationService.NavigateTo(_factoryVerifyTestVM, new object[2] { result.Item2, message });
        }

        /// <summary>
        /// Открытие окна с обучающим материалом.
        /// </summary>
        /// <param name="parameter"></param>
        private void OpenEducationCommand(object parameter)
        {
            _timer?.Stop();
            _navigationService.NavigateTo<EducationViewModel>();
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

        private void UpdateTimerValue(object parameter)
        {
            OnPropertyChanged(nameof(TimerValue));
        }
        #endregion
    }
}
