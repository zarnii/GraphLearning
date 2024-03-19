using GraphApp.Command;
using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphApp.ViewModel
{
    public class PracticViewModel : VisualEditorViewModel
    {
        #region fields
        /// <summary>
        /// Поставщик практических заданий.
        /// </summary>
        private IAccessControlService _accsessControlService;

        /// <summary>
        /// Сервис проверки практических заданий.
        /// </summary>
        private IVerifyPracticTaskService _verifyPracticTaskService;

        /// <summary>
        /// Сервис навигации.
        /// </summary>
        private INavigationService _navigationService;

        private string _resultText;

        private Brush _resultColor;

        private int _resultOpasity;
        #endregion

        #region properties
        /// <summary>
        /// Команда проверки задания.
        /// </summary>
        public ICommand VerifyTask { get; private set; }

        /// <summary>
        /// Переход назад.
        /// </summary>
        public ICommand GoBack { get; private set; }

        /// <summary>
        /// Заголовок практического задания.
        /// </summary>
        public string PracticTaskTitle
        {
            get
            {
                return ((PracticTask)_accsessControlService?.CurrentEducationMaterial.EducationMaterial).Title;
            }
        }

        /// <summary>
        /// Текст практического задания.
        /// </summary>
        public string PracticTaskText
        {
            get
            {
                return ((PracticTask)_accsessControlService?.CurrentEducationMaterial.EducationMaterial).Text;
            }
        }

        /// <summary>
        /// Текст ответа.
        /// </summary>
        public string ResultText 
        {
            get
            {
                return _resultText;
            }
            private set
            {
                _resultText = value;
                OnPropertyChanged(nameof(ResultText));
            }
        }

        /// <summary>
        /// Цвет ответа.
        /// </summary>
        public Brush ResultColor 
        {
            get
            {
                return _resultColor;
            }
            private set
            {
                _resultColor = value;
                OnPropertyChanged(nameof(ResultColor));
            }
        }

        /// <summary>
        /// Прозрачность ответа.
        /// </summary>
        public int ResultOpasity 
        {
            get
            {
                return _resultOpasity;
            } 
            private set
            {
                _resultOpasity = value;
                OnPropertyChanged(nameof(ResultOpasity));
            }
        }
        #endregion

        #region constructor
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="vertexViewModel">Модель представления вершин.</param>
        /// <param name="connectionViewModel">Модель представления связи.</param>
        /// <param name="visualEditorService">Сервис графического редактора.</param>
        /// <param name="accessControlService">Сервис контроля доступа.</param>
        /// <param name="verifyPracticTaskService">Сервис проверки практических заданий.</param>
        /// <param name="navigationService">Сервис навигации.</param>
        public PracticViewModel(
            VertexViewModel vertexViewModel,
            ConnectionViewModel connectionViewModel,
            IVisualEditorService visualEditorService, 
            IAccessControlService accessControlService,
            IVerifyPracticTaskService verifyPracticTaskService,
            INavigationService navigationService)
            : base(vertexViewModel, connectionViewModel, visualEditorService)
        {
            
            _accsessControlService = accessControlService;
            _verifyPracticTaskService = verifyPracticTaskService;
            _navigationService = navigationService;

            VerifyTask = new RelayCommand(VerifyTaskCommand);
            GoBack = new RelayCommand(GoBackCommand);
            ResultOpasity = 0;
        }
        #endregion

        #region private methods
        /// <summary>
        /// Проверка практического задания.
        /// </summary>
        /// <param name="parameter"></param>
        private void VerifyTaskCommand(object parameter)
        {
            var practicTask = (PracticTask)_accsessControlService.CurrentEducationMaterial.EducationMaterial;

            var result = _verifyPracticTaskService.VerifyPracticTask(practicTask, Vertices, Connections);

            var isDone = true;

            if (practicTask.NeedCheckVertexCount && !result.VertexCountIsDone)
            {
                isDone = false;
            }

            if (practicTask.NeedCheckVertexPosition && !result.VertexPositionIsDone)
            {
                isDone = false;
            }

            if (practicTask.NeedCheckVertexSize && !result.VertexSizeIsDone)
            {
                isDone = false;
            }

            if (practicTask.NeedCheckVertexName && !result.VertexNameIsDone)
            {
                isDone = false;
            }

            if (practicTask.NeedCheckConnectionCount && !result.ConnectionCountIsDone)
            {
                isDone = false;
            }

            if (practicTask.NeedCheckConnection && !result.ConnectionIsDone)
            {
                isDone = false;
            }

            if (practicTask.NeedCheckConnectionWeight && !result.ConnectionWeightIsDone)
            {
                isDone = false;
            }

            if (practicTask.NeedCheckConnectionType && !result.ConnectionTypeIsDone)
            {
                isDone = false;
            }

            CheckResult(isDone);

            if (isDone)
            {
                _accsessControlService.OpenNext(_accsessControlService.CurrentEducationMaterial);
            }
        }

        /// <summary>
        /// Команда перехода назад.
        /// </summary>
        /// <param name="parameter"></param>
        private void GoBackCommand(object parameter)
        {
            _navigationService.NavigateTo<EducationViewModel>();
        }

        private void CheckResult(bool isDone)
        {
            if (isDone)
            {
                ResultText = "Верно";
                ResultColor = new SolidColorBrush(Colors.Green);
                ResultOpasity = 1;

                return;
            }

            ResultText = "Не верно";
            ResultColor = new SolidColorBrush(Colors.Red);
            ResultOpasity = 1;
        }
        #endregion
    }
}