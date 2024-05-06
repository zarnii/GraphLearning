using GraphApp.Command;
using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace GraphApp.ViewModel
{
    /// <summary>
    /// Базовый класс, для моделей вредставления, которые имеют графический редактор.
    /// </summary>
    public class VisualEditorViewModel : ViewModel
    {
        #region fields
        /// <summary>
        /// Сервис графического редактора.
        /// </summary>
        private IVisualEditorService _visualEditorService;

        /// <summary>
        /// Модель представления вершины.
        /// </summary>
        private ViewModel _vertexViewModel;

        /// <summary>
        /// Прозрачность номера связи.
        /// </summary>
        private int _connectionNumberOpasity;

        /// <summary>
        /// Прозрачность веса связи.
        /// </summary>
        private int _connectionWeightOpasity;

        /// <summary>
        /// Модель представления связи.
        /// </summary>
        private ViewModel _connectionViewModel;

        /// <summary>
        /// Матрица смежности.
        /// </summary>
        private AdjacencyMatrix _adjacencyMatrix;

        /// <summary>
        /// Матрица инцидентности.
        /// </summary>
        private IncidenceMatrix _incidenceMatrix;
        #endregion

        #region properties
        /// <summary>
        /// Свойство команды изменения режима мыши.
        /// </summary>
        public ICommand ChangeMouseMode { get; private set; }

        /// <summary>
        /// Команда нажатия на поле.
        /// </summary>
        public ICommand ClickOnField { get; private set; }

        /// <summary>
        /// Команда нажатия на элемент графа.
        /// </summary>
        public ICommand ClickOnGraphElement { get; private set; }

        /// <summary>
        /// Команда перемещения вершины.
        /// </summary>
        public ICommand MoveVertex { get; private set; }

        /// <summary>
        /// Команда создания матрицы смежности.
        /// </summary>
        public ICommand CreateAdjacencyMatrix { get; private set; }

        /// <summary>
        /// Команда построения матрицы инцидентности.
        /// </summary>
        public ICommand CreateIncidenceMatrix { get; private set; }

        /// <summary>
        /// Команда очистки поля.
        /// </summary>
        public ICommand Clear { get; private set; }

        /// <summary>
        /// Команда смены отображения номера связи.
        /// </summary>
        public ICommand ChangeConnectionNumberVisible { get; private set; }

        /// <summary>
        /// Команда смены отображения веса связи.
        /// </summary>
        public ICommand ChangeConnectionWeightVisible { get; private set; }

        /// <summary>
        /// Ширина графического поля.
        /// </summary>
        public int CanvasWidth
        {
            get
            {
                return _visualEditorService.CanvasWidth;
            }
            set
            {
                _visualEditorService.CanvasWidth = value;
            }
        }

        /// <summary>
        /// Высота графического поля.
        /// </summary>
        public int CanvasHeight
        {
            get
            {
                return _visualEditorService.CanvasHeight;
            }
            set
            {
                _visualEditorService.CanvasHeight = value;
            }
        }

        /// <summary>
        /// Прозрачность номера связи.
        /// </summary>
        public int ConnectionNumberOpasity 
        {
            get
            {
                return _connectionNumberOpasity;
            }
            private set
            {
                _connectionNumberOpasity = value;
                OnPropertyChanged(nameof(ConnectionNumberOpasity));
            }
        }

        /// <summary>
        /// Прозрачность веса связи.
        /// </summary>
        public int ConnectionWeightOpasity
        {
            get
            {
                return _connectionWeightOpasity;
            }
            set
            {
                _connectionWeightOpasity = value;
                OnPropertyChanged(nameof(ConnectionWeightOpasity));
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
        /// Коллекция вершин.
        /// </summary>
        public ObservableCollection<VisualVertex> Vertices
        {
            get
            {
                return _visualEditorService.Vertices;
            }
        }

        /// <summary>
        /// Коллекция связей.
        /// </summary>
        public ObservableCollection<VisualConnection> Connections
        {
            get
            {
                return _visualEditorService.Connections;
            }
        }

        public AdjacencyMatrix AdjancencyMatrix
        {
            get
            {
                return _adjacencyMatrix;
            }
            private set
            {
                _adjacencyMatrix = value;
                OnPropertyChanged(nameof(this.AdjancencyMatrix));
            }
        }

        public IncidenceMatrix IncidenceMatrix
        {
            get
            {
                return _incidenceMatrix;
            }
            private set
            {
                _incidenceMatrix = value;
                OnPropertyChanged(nameof(this.IncidenceMatrix));
            }
        }
        #endregion

        #region constructor
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="vertexViewModel">Модель представления вершины.</param>
        /// <param name="connectionViewModel">Модель представления связи.</param>
        /// <param name="visualEditorService">Сервис графического редактора.</param>
        public VisualEditorViewModel(
            VertexViewModel vertexViewModel,
            ConnectionViewModel connectionViewModel,
            IVisualEditorService visualEditorService)
        {
            _vertexViewModel = vertexViewModel;
            _connectionViewModel = connectionViewModel;
            _visualEditorService = visualEditorService;

            ChangeMouseMode = new RelayCommand(SetMouseMode);
            ClickOnField = new RelayCommand(ClickOnFieldCommand);
            MoveVertex = new RelayCommand(MoveVertexCommand);
            ClickOnGraphElement = new RelayCommand(ClickOnGraphElementCommand);
            CreateAdjacencyMatrix = new RelayCommand(CreateAdjacencyMatrixCommand);
            CreateIncidenceMatrix = new RelayCommand(CreateIncidenceMatrixCommand);
            ChangeConnectionNumberVisible = new RelayCommand(ChangeConnectionNumberVisibleCommand);
            ChangeConnectionWeightVisible = new RelayCommand(ChangeConnectionWeightVisibleCommand);
            Clear = new RelayCommand(ClearCommand);

            ConnectionNumberOpasity = 1;
            ConnectionWeightOpasity = 1;
        }
        #endregion

        #region public methods
        /// <summary>
        /// Добавление связи.
        /// </summary>
        /// <param name="connection">Связью</param>
        /// <exception cref="ArgumentNullException">NULL аргумент.</exception>
        public void AddConnection(VisualConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection), "Пустая связь.");
            }

            connection.OnDelete += _visualEditorService.DeleteConnection;
        }
        #endregion

        #region private methods
        /// <summary>
        /// Изменение режима мыши.
        /// </summary>
        /// <param name="mode">Режим мыши.</param>
        private void SetMouseMode(object parameter)
        {
            SelectedGraphElement = null;
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
                point.X -= createVertexWindow.VertexRadius;
                point.Y -= createVertexWindow.VertexRadius;

                _visualEditorService.AddVertex(
                    point,
                    createVertexWindow.VertexRadius,
                    createVertexWindow.VertexName,
                    createVertexWindow.VertexColor
                );
            }
        }

        /// <summary>
        /// Команда нажатия на вершину.
        /// </summary>
        /// <param name="vertex">Нажатая вершина.</param>
        private void ConnectVertices(VisualVertex vertex)
        {
            if (SelectedVerticesForConnection.Count < 2)
            {
                SelectedVerticesForConnection.Add(vertex);
            }

            if (SelectedVerticesForConnection.Count == 2)
            {
                var createConnectionWindow = new CreateConnectionWindow();

                if ((bool)createConnectionWindow.ShowDialog())
                {
                    _visualEditorService.AddConnection(
                        (SelectedVerticesForConnection[0], SelectedVerticesForConnection[1]),
                        createConnectionWindow.ConnectionThickness,
                        createConnectionWindow.ConnectionWeight,
                        createConnectionWindow.ConnectionType
                    );
                }

                SelectedVerticesForConnection.Clear();
            }
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
                (int)dragDeltaEventArgs.HorizontalChange,
                (int)dragDeltaEventArgs.VerticalChange
            );
        }

        private void ClearCommand(object parameter)
        {
            _visualEditorService.Clear();
        }

        private void CreateAdjacencyMatrixCommand(object parameter)
        {
            try
            {
                AdjancencyMatrix = _visualEditorService.CreateAdjacencyMatrix();
            }
            catch (DuplicateNameException)
            {
                MessageBox.Show(
                    "Нельза составить матрицу, так как есть дубликат в названии вершин",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void CreateIncidenceMatrixCommand(object parameter)
        {
            try
            {
                IncidenceMatrix = _visualEditorService.CreateIncidenceMatrix();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        /// <summary>
        /// Нажатие на элемент графа.
        /// </summary>
        /// <param name="parameter">Элемент графа.</param>
        private void ClickOnGraphElementCommand(object parameter)
        {
            if (_visualEditorService.MouseMode == MouseMode.Delete)
            {
                if (parameter is VisualVertex)
                {
                    _visualEditorService.DeleteVertex((VisualVertex)parameter);
                }
                else
                {
                    _visualEditorService.DeleteConnection((VisualConnection)parameter);
                }

                return;
            }

            if (_visualEditorService.MouseMode == MouseMode.Connect)
            {
                if (parameter is VisualVertex)
                {
                    ConnectVertices((VisualVertex)parameter);
                }

                return;
            }

            SelectedGraphElement = parameter is VisualVertex
                ? _vertexViewModel
                : _connectionViewModel;

            if (SelectedGraphElement is VertexViewModel)
            {
                ((VertexViewModel)SelectedGraphElement).VisualVertex = (VisualVertex)parameter;

                ((VertexViewModel)SelectedGraphElement).MaxXCoordinate = CanvasWidth - ((VisualVertex)parameter).Radius;
                ((VertexViewModel)SelectedGraphElement).MaxYCoordinate = CanvasHeight - ((VisualVertex)parameter).Radius;
            }
            else if (SelectedGraphElement is ConnectionViewModel)
            {
                ((ConnectionViewModel)SelectedGraphElement).VisualConnection = (VisualConnection)parameter;
            }
        }

        private void ChangeConnectionNumberVisibleCommand(object parameter)
        {
            ConnectionNumberOpasity = ConnectionNumberOpasity == 1
                ? 0
                : 1;
        }

        private void ChangeConnectionWeightVisibleCommand(object parameter)
        {
            ConnectionWeightOpasity = ConnectionWeightOpasity == 1
                ? 0
                : 1;
        }
        #endregion
    }
}
