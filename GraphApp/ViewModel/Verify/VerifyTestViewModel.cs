using GraphApp.Command;
using GraphApp.Interfaces;
using GraphApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace GraphApp.ViewModel.Verify
{
    /// <summary>
    /// Модель представления проверки теста.
    /// </summary>
    public class VerifyTestViewModel : ViewModel
    {
        #region fields
        /// <summary>
        /// Сервис навигации.
        /// </summary>
        private INavigationService _navigationService;
        #endregion

        #region properties
        /// <summary>
        /// Команда открытия TestView.
        /// </summary>
        public ICommand OpenTestView { get; private set; }

        /// <summary>
        /// Проверенные ответы по вопросам.
        /// </summary>
        public ObservableCollection<KeyValuePair<Question, List<VisualAnswer>>> QuestionVerifiedAnswerMap { get; private set; }

        /// <summary>
        /// Сообщение.
        /// </summary>
        public string Message { get; set; }
        #endregion

        #region constructor
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="navigationService">Сервис навигации.</param>
        /// <param name="questionVerifiedAnswerMap">Проверенные ответы по вопросам.</param>
        /// <param name="message">Сообщение.</param>
        public VerifyTestViewModel(
            INavigationService navigationService,
            Dictionary<Question, List<VisualAnswer>> questionVerifiedAnswerMap,
            string message)
        {
            _navigationService = navigationService;
            Message = message;

            QuestionVerifiedAnswerMap = new ObservableCollection<KeyValuePair<Question, List<VisualAnswer>>>(
                questionVerifiedAnswerMap.ToList()
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
