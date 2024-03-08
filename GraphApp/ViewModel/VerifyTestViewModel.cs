using GraphApp.Command;
using GraphApp.Interfaces;
using GraphApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace GraphApp.ViewModel
{
    /// <summary>
    /// Модель представления проверки теста.
    /// </summary>
    public class VerifyTestViewModel : ViewModel
    {
        #region fields
        /// <summary>
        /// Сервис проверки ответов.
        /// </summary>
        private ITestCheckService _testCheckService;

        /// <summary>
        /// Сервис навигации.
        /// </summary>
        private INavigationService _navigationService;

        /// <summary>
        /// Команда открытия TestView.
        /// </summary>
        private ICommand _openTestView;
        #endregion

        #region properties
        /// <summary>
        /// Команда открытия TestView.
        /// </summary>
        public ICommand OpenTestView
        {
            get
            {
                return _openTestView;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Пустая команда перехода.");
                }

                _openTestView = value;
            }
        }

        /// <summary>
        /// Проверенные ответы по вопросам.
        /// </summary>
        public ObservableCollection<KeyValuePair<Question, List<VisualAnswer>>> QuestionVerifiedAnswerMap { get; private set; }
        #endregion

        #region constructor
        /// <summary>
        /// Констурктор.
        /// </summary>
        /// <param name="testCheckService">Сервис проверки теста.</param>
        /// <param name="navigationService">Сервис навигации.</param>
        public VerifyTestViewModel(ITestCheckService testCheckService, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _testCheckService = testCheckService;
            _testCheckService.CheckAnswer();

            QuestionVerifiedAnswerMap = new ObservableCollection<KeyValuePair<Question, List<VisualAnswer>>>(
                _testCheckService.QuestionVerifiedAnswerMap.ToList()
            );

            OpenTestView = new RelayCommand(OpenTestViewCommand);
        }
        #endregion

        #region private methods
        /// <summary>
        /// Открытие TestView.
        /// </summary>
        /// <param name="parameter"></param>
        private void OpenTestViewCommand(object parameter)
        {
            _navigationService.NavigateTo<TestViewModel>();
        }
        #endregion
    }
}
