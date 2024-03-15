using GraphApp.Command;
using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.Model.Exception;
using GraphApp.Model.Serializing;
using GraphApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;


namespace GraphApp.ViewModel
{
    /// <summary>
    /// Модель представления страницы редактора.
    /// </summary>
    public class PlaygroundViewModel : ViewModel, INotifyPropertyChanged
    {
        #region fields
        /// <summary>
        /// Обработчик данных.
        /// </summary>
        private IDataHeandlerService _dataHeandler;

        /// <summary>
        /// Маппер.
        /// </summary>
        private IMapper _mapper;

        /// <summary>
        /// Сервис навигации.
        /// </summary>
        private INavigationService _navigationService;

        /// <summary>
        /// Сервис визуального редактора.
        /// </summary>
        private IVisualEditorService _visualEditorService;

        /// <summary>
        /// Модель представления вершины.
        /// </summary>
        private ViewModel _vertexVeiwModel;

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
        /// Команда добавления вершины.
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
        /// Команда сохранения графа.
        /// </summary>
        public ICommand SaveGraph { get; private set; }

        /// <summary>
        /// Команда загрузки графа.
        /// </summary>
        public ICommand LoadGraph { get; private set; }

        /// <summary>
        /// Переход назад.
        /// </summary>
        public ICommand GoBack { get; private set; }

        public int CanvasWidth 
        {
            get
            {
                return _visualEditorService.CanvasWidth;
            }
            set
            {
                _visualEditorService.CanvasHeight = value;
            } 
        }

        public int CanvasHeight 
        {
            get
            {
                return _visualEditorService.CanvasHeight;
            }
            set
            {
                _visualEditorService.CanvasWidth = value;
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
        /// Лист вершин.
        /// </summary>
        public ObservableCollection<VisualVertex> Vertices
        {
            get
            {
                return _visualEditorService.Vertices;
            }
        }

        /// <summary>
        /// Лист связей.
        /// </summary>
        public ObservableCollection<VisualConnection> Connections
        {
            get
            {
                return _visualEditorService.Connections;
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
        public PlaygroundViewModel(
            IDataHeandlerService dataHeandler,
            IMapper mapper,
            INavigationService navigationService,
            IVisualEditorService visualEditorService,
            VertexViewModel vertexViewModel,
            ConnectionViewModel connectionViewModel)
        {
            _dataHeandler = dataHeandler;
            _mapper = mapper;
            _navigationService = navigationService;
            _visualEditorService = visualEditorService;

            _vertexVeiwModel = vertexViewModel;
            _connectionViewModel = connectionViewModel;

            ChangeMouseMode = new RelayCommand(SetMouseMode);
            ClickOnField = new RelayCommand(ClickOnFieldCommand);
            MoveVertex = new RelayCommand(MoveVertexCommand);
            SaveGraph = new RelayCommand(SaveGraphCommand);
            LoadGraph = new RelayCommand(LoadGraphCommand);
            GoBack = new RelayCommand(GoBackCommand);
            ClickOnGraphElement = new RelayCommand(ClickOnGraphElementCommand);

            _visualEditorService.AddVertex(new Point(200, 200), 10, "default");
            _visualEditorService.AddVertex(new Point(100, 100), 10, "default");
            _visualEditorService.AddVertex(new Point(100, 200), 10, "default");


            _visualEditorService.AddConnection((Vertices[0], Vertices[1]), 4);

            _visualEditorService.AddConnection((Vertices[0], Vertices[2]), 4);
        }
        #endregion

        #region public methods
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
                _visualEditorService.AddConnection((SelectedVerticesForConnection[0], SelectedVerticesForConnection[1]), 4);
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
                dragDeltaEventArgs.HorizontalChange,
                dragDeltaEventArgs.VerticalChange
            );
        }

        /// <summary>
        /// Созранение графа.
        /// </summary>
        /// <param name="parameter"></param>
        private void SaveGraphCommand(object parameter)
        {
            var vertices = new List<SerializableVertex>();
            var connections = new List<SerializableConnection>();

            foreach (var vertex in Vertices)
            {
                vertices.Add(_mapper.Map<SerializableVertex>(vertex, null));
            }

            foreach (var connection in Connections)
            {
                connections.Add(_mapper.Map<SerializableConnection>(connection, null));
            }

            _dataHeandler.Save<SerializableData>(new SerializableData()
            {
                Connections = connections,
                Vertices = vertices,
            });
        }

        /// <summary>
        /// Загрузка графа.
        /// </summary>
        /// <param name="parameter"></param>
        private void LoadGraphCommand(object parameter)
        {
            try
            {
                var data = _dataHeandler.Load<SerializableData>();


                if (data == null || data.Vertices == null || data.Connections == null)
                {
                    return;
                }

                Vertices.Clear();
                Connections.Clear();

                foreach (var sVertex in data.Vertices)
                {
                    var vertex = _mapper.Map<VisualVertex>(sVertex, null);
                    Vertices.Add(vertex);
                }

                foreach (var sConnection in data.Connections)
                {
                    var vertices = Vertices.ToList();
                    var connection = _mapper.Map<VisualConnection>(sConnection, vertices);
                    connection.OnDelete += _visualEditorService.DeleteConnection;
                    Connections.Add(connection);
                }
            }
            catch (LoadDataException ex)
            {
                MessageBox.Show(
                    "Произошла оишбка при загрузки графа, возможно файл был отредактирован",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );

                Vertices.Clear();
                Connections.Clear();
            }
            catch (DirectoryNotFoundException ex)
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
        /// Переход назад.
        /// </summary>
        /// <param name="parameter"></param>
        private void GoBackCommand(object parameter)
        {
            _navigationService.NavigateTo<MainMenuViewModel>();
        }

        /// <summary>
        /// Оповещение подписчиков о изменении свойства.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                ? _vertexVeiwModel
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
        #endregion
    }
}
