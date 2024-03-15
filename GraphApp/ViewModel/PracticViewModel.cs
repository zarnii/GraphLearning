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

namespace GraphApp.ViewModel
{
    public class PracticViewModel : ViewModel
    {
        #region fields
        /// <summary>
        /// Сервис визуального редактора.
        /// </summary>
        private IVisualEditorService _visualEditorService;

        /// <summary>
        /// Поставщик практических заданий.
        /// </summary>
        private IAccessControlService _accsessControlService;

        /// <summary>
        /// Сервис проверки практических заданий.
        /// </summary>
        private IVerifyPracticTaskService _verifyPracticTaskService;

        private Func<Type, ViewModel> _vmFactory;
        #endregion

        #region properties
        /// <summary>
        /// Свойство команды изменения режима мыши.
        /// </summary>
        public ICommand ChangeMouseMode { get; private set; }

        /// <summary>
        /// Команда добавления вершины.
        /// </summary>
        public ICommand ClickOnField { get; private set; }

        /// <summary>
        /// Команда нажатия на вершину.
        /// </summary>
        public ICommand ClickOnVertex { get; private set; }

        /// <summary>
        /// Команда нажатия на связь.
        /// </summary>
        public ICommand ClickOnConnection { get; private set; }

        /// <summary>
        /// Команда перемещения вершины.
        /// </summary>
        public ICommand MoveVertex { get; private set; }

        /// <summary>
        /// Команда проверки задания.
        /// </summary>
        public ICommand VerifyTask { get; private set; }

        /// <summary>
        /// Связи.
        /// </summary>
        public ObservableCollection<VisualConnection> Connections
        {
            get
            {
                return _visualEditorService.Connections;
            }
        }

        /// <summary>
        /// Выбранные вершины для соединения.
        /// </summary>
        public List<VisualVertex> SelectedVerticesForConnection
        {
            get
            {
                return _visualEditorService.SelectedVerticesForConnection;
            }
        }

        /// <summary>
        /// Вершины.
        /// </summary>
        public ObservableCollection<VisualVertex> Vertices
        {
            get
            {
                return _visualEditorService.Vertices;
            }
        }

        public string PracticTaskTitle
        {
            get
            {
                return ((PracticTask)_accsessControlService?.CurrentEducationMaterial.EducationMaterial).Title;
            }
        }

        public string PracticTaskText
        {
            get
            {
                return ((PracticTask)_accsessControlService?.CurrentEducationMaterial.EducationMaterial).Text;
            }
        }

        /// <summary>
        /// Выбранный элемент графа.
        /// </summary>
        public ViewModel SelectedGraphElement
        {
            get
            {
                return _visualEditorService.SelectedGraphElement;
            }
            set
            {
                _visualEditorService.SelectedGraphElement = value;
                OnPropertyChanged(nameof(SelectedGraphElement));
            }
        }

        /// <summary>
        /// Событие изменения свойтва.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        #region constructor
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="visualEditorService">Сервис визуального редактора.</param>
        /// <param name="practicProvider">Поставщик практических заданий.</param>
        public PracticViewModel(
            IVisualEditorService visualEditorService, 
            IAccessControlService accessControlService,
            IVerifyPracticTaskService verifyPracticTaskService,
            Func<Type, ViewModel> vmFactory)
        {
            _visualEditorService = visualEditorService;
            _accsessControlService = accessControlService;
            _verifyPracticTaskService = verifyPracticTaskService;
            _vmFactory = vmFactory;

            ChangeMouseMode = new RelayCommand(SetMouseMode);
            ClickOnField = new RelayCommand(ClickOnFieldCommand);
            ClickOnVertex = new RelayCommand(ClickOnVertexCommand);
            ClickOnConnection = new RelayCommand(ClickOnConnectionCommand);
            MoveVertex = new RelayCommand(MoveVertexCommand);
            VerifyTask = new RelayCommand(VerifyTaskCommand);
        }
        #endregion

        #region private methods
        /// <summary>
        /// Изменение режима мыши.
        /// </summary>
        /// <param name="mode">Режим мыши.</param>
        private void SetMouseMode(object parameter)
        {
            _visualEditorService.SetMouseMode((MouseMode)parameter);
        }

        /// <summary>
        /// Команда нажатия на поле.
        /// </summary>
        /// <param name="parameter">Аргументы события.</param>
        private void ClickOnFieldCommand(object parameter)
        {
            if (_visualEditorService.MouseMode != MouseMode.Create)
            {
                return;
            }

            var mbEventArgs = parameter as MouseButtonEventArgs;
            var point = mbEventArgs.GetPosition((UIElement)mbEventArgs.OriginalSource);


            var createVertexWindow = new CreateVertexWindow();

            if ((bool)createVertexWindow.ShowDialog())
            {
                _visualEditorService.AddVertex(
                    point,
                    createVertexWindow.VertexRadius,
                    createVertexWindow.VertexName
                );
            }
        }

        /// <summary>
        /// Команда нажатия на вершину.
        /// </summary>
        /// <param name="parameter">Нажатая вершина.</param>
        private void ClickOnVertexCommand(object parameter)
        {
            var vertex = (VisualVertex)parameter;

            if (_visualEditorService.MouseMode == MouseMode.Default)
            {
                SelectedGraphElement = _vmFactory.Invoke(typeof(VertexViewModel));
                ((VertexViewModel)SelectedGraphElement).VisualVertex = vertex;
            }
            else if (_visualEditorService.MouseMode == MouseMode.Delete)
            {
                DeleteVertex(vertex);
            }
            else if (_visualEditorService.MouseMode == MouseMode.Connect)
            {
                if (SelectedVerticesForConnection.Count < 2)
                {
                    SelectedVerticesForConnection.Add(vertex);
                }

                if (SelectedVerticesForConnection.Count == 2)
                {
                    _visualEditorService.AddConnection((SelectedVerticesForConnection[0], SelectedVerticesForConnection[1]), 4);
                    SelectedVerticesForConnection.Clear();
                }
            }
        }

        /// <summary>
        /// Команда нажатия на связь.
        /// </summary>
        /// <param name="parameter">Связь.</param>
        private void ClickOnConnectionCommand(object parameter)
        {
            if (_visualEditorService.MouseMode == MouseMode.Delete)
            {
                _visualEditorService.DeleteConnection((VisualConnection)parameter);
            }
        }

        /// <summary>
        /// Удаление вершины.
        /// </summary>
        /// <param name="parameter">Удаляемая вершина.</param>
        private void DeleteVertex(object parameter)
        {
            var vertex = (VisualVertex)parameter;

            /*
            if (SelectedVertex == vertex)
            {
                _visualEditorService.SelectedVertex = null;
                OnPropertyChanged(nameof(SelectedVertex));
            }
            */
            _visualEditorService.DeleteVertex(vertex);
        }

        /// <summary>
        /// Передвижение вершин.
        /// </summary>
        /// <param name="parameter">Аргументы события.</param>
        private void MoveVertexCommand(object parameter)
        {
            var dragDeltaEventArgs = parameter as DragDeltaEventArgs;

            _visualEditorService.MoveVertex(
                (VisualVertex)((FrameworkElement)dragDeltaEventArgs.OriginalSource).DataContext,
                dragDeltaEventArgs.HorizontalChange,
                dragDeltaEventArgs.VerticalChange
            );
        }

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
        }

        /// <summary>
        /// Оповещение подписчиков о изменении свойства.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}