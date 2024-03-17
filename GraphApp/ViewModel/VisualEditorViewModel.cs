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
    /// <summary>
    /// Базовый класс, для моделей вредставления, которые имеют графический редактор.
    /// </summary>
    public class VisualEditorViewModel : ViewModel, INotifyPropertyChanged
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
        /// Модель представления связи.
        /// </summary>
        private ViewModel _connectionViewModel;
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

        /// <summary>
        /// Событие изменения свойства.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
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
        }
        #endregion

        #region public methods
        /// <summary>
        /// Добавление вершины.
        /// </summary>
        /// <param name="coordinates">Координаты.</param>
        /// <param name="radius">Радиус.</param>
        /// <param name="name">Навзание.</param>
        /// <exception cref="ArgumentOutOfRangeException">Координаты заданы не верно.</exception>
        public void AddVertrex(Point coordinates, int radius, string name)
        {
            if (coordinates.X < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(coordinates.X), "Координата X не может быть меньше нуля.");
            }

            if (coordinates.X > CanvasHeight)
            {
                throw new ArgumentOutOfRangeException(nameof(coordinates.X), $"Координата X не может быть больше {CanvasHeight}.");
            }

            if (coordinates.Y > CanvasWidth)
            {
                throw new ArgumentOutOfRangeException(nameof(coordinates.X), $"Координата Y не может быть больше {CanvasWidth}.");
            }

            if (coordinates.Y < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(coordinates.X), "Координата Y не может быть меньше нуля.");
            }

            if (radius < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(radius), "Радиус вершины не может быть меньше 1.");
            }

            _visualEditorService.AddVertex(coordinates, radius, name);
        }

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
                    createVertexWindow.VertexName
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
                    SelectedVerticesForConnection.Clear();
                }
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
                dragDeltaEventArgs.HorizontalChange,
                dragDeltaEventArgs.VerticalChange
            );
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

        /// <summary>
        /// Оповещение подписчиков о изменении свойства.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
