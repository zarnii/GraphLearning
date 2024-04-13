using GraphApp.Command;
using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.Services;
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
        /// Таймер.
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// Прозрачноить таймера.
        /// </summary>
        private double _timerOpasity;

        /// <summary>
        /// Сервис проверки практических заданий.
        /// </summary>
        private IVerifyPracticTaskService _verifyPracticTaskService;

        /// <summary>
        /// Сервис навигации.
        /// </summary>
        private INavigationService _navigationService;

        /// <summary>
        /// Сервис контроля доступа.
        /// </summary>
        private IAccessControlService _accessControlService;

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
                return CurrentPracticTask.Title;
            }
        }

        /// <summary>
        /// Текст практического задания.
        /// </summary>
        public string PracticTaskText
        {
            get
            {
                return CurrentPracticTask.Text;
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

        /// <summary>
        /// Прозрачность таймера.
        /// </summary>
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
        public PracticTask CurrentPracticTask { get; private set; }
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
            VerifyPracticViewModel verifyPracticViewModel,
            IVisualEditorService visualEditorService, 
            IAccessControlService accessControlService,
            IVerifyPracticTaskService verifyPracticTaskService,
            INavigationService navigationService)
            : base(vertexViewModel, connectionViewModel, visualEditorService)
        {
            _accessControlService = accessControlService;
            _verifyPracticTaskService = verifyPracticTaskService;
            _navigationService = navigationService;
            CurrentPracticTask = (PracticTask)accessControlService.CurrentEducationMaterial.EducationMaterial;

            VerifyTask = new RelayCommand(VerifyTaskCommand);
            GoBack = new RelayCommand(GoBackCommand);
            ResultOpasity = 0;

            if (CurrentPracticTask.LeadTime != null)
            {
                _timer = new Timer(
                    CurrentPracticTask.LeadTime.Value,
                    VerifyTaskCommand,
                    null,
                    UpdateTimer
                );
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// Проверка практического задания.
        /// </summary>
        /// <param name="parameter"></param>
        private void VerifyTaskCommand(object parameter)
        {
            _timer?.Stop();
            var result = _verifyPracticTaskService.VerifyPracticTask(CurrentPracticTask, Vertices, Connections);
            var isDone = true;

            if (CurrentPracticTask.NeedCheckVertexCount && !result.VertexCountIsDone)
            {
                isDone = false;
            }

            if (CurrentPracticTask.NeedCheckVertexPosition && !result.VertexPositionIsDone)
            {
                isDone = false;
            }

            if (CurrentPracticTask.NeedCheckVertexSize && !result.VertexSizeIsDone)
            {
                isDone = false;
            }

            if (CurrentPracticTask.NeedCheckVertexName && !result.VertexNameIsDone)
            {
                isDone = false;
            }

            if (CurrentPracticTask.NeedCheckConnectionCount && !result.ConnectionCountIsDone)
            {
                isDone = false;
            }

            if (CurrentPracticTask.NeedCheckConnection && !result.ConnectionIsDone)
            {
                isDone = false;
            }

            if (CurrentPracticTask.NeedCheckConnectionWeight && !result.ConnectionWeightIsDone)
            {
                isDone = false;
            }

            if (CurrentPracticTask.NeedCheckConnectionType && !result.ConnectionTypeIsDone)
            {
                isDone = false;
            }

            if (!_accessControlService.CheckEducationMaterialIsPassed(_accessControlService.CurrentEducationMaterial))
            {
                _accessControlService.AddAttempt(_accessControlService.CurrentEducationMaterial);
            }

            CheckResult(isDone);

            if (isDone)
            {
                _accessControlService.OpenNext(_accessControlService.CurrentEducationMaterial);
            }
        }

        /// <summary>
        /// Команда перехода назад.
        /// </summary>
        /// <param name="parameter"></param>
        private void GoBackCommand(object parameter)
        {
            _timer?.Stop();
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

        /// <summary>
        /// Обновление таймера.
        /// </summary>
        private void UpdateTimer(object parameter)
        {
            OnPropertyChanged(nameof(TimerValue));
        }
        #endregion
    }
}