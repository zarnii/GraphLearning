using GraphApp.Command;
using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// Выбранные вершины.
        /// </summary>
        public List<VisualVertex> SelectedVertices
        {
            get
            {
                return _visualEditorService.SelectedVertices;
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
            IVerifyPracticTaskService verifyPracticTaskService)
        {
            _visualEditorService = visualEditorService;
            _accsessControlService = accessControlService;
            _verifyPracticTaskService = verifyPracticTaskService;

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
            _visualEditorService.ClickOnVertex((VisualVertex)parameter);
        }

        /// <summary>
        /// Команда нажатия на связь.
        /// </summary>
        /// <param name="parameter"></param>
        private void ClickOnConnectionCommand(object parameter)
        {
            _visualEditorService.ClickOnConnection((VisualConnection)parameter);
        }

        /// <summary>
        /// Удаление вершины.
        /// </summary>
        /// <param name="vertex"></param>
        private void DeleteVertex(VisualVertex vertex)
        {
            _visualEditorService.DeleteVertex(vertex);
        }


        /// <summary>
        /// Создание связи.
        /// </summary>
        /// <param name="connectedVertices">Соеденяемые вершины.</param>
        private void AddConnection((VisualVertex, VisualVertex) connectedVertices,
            double weight = 0,
            ConnectionType connectionType = ConnectionType.NonDirectional)
        {
            _visualEditorService.AddConnection(connectedVertices, weight, connectionType);
        }

        /// <summary>
        /// Удаление связи.
        /// </summary>
        /// <param name="connection">Удаляемая связь.</param>
        private void DeleteConnection(VisualConnection connection)
        {
            _visualEditorService.DeleteConnection(connection);
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
        #endregion
    }
}