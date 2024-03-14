using GraphApp.Command;
using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.Model.Exception;
using GraphApp.Model.Serializing;
using GraphApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;


namespace GraphApp.ViewModel
{
    /// <summary>
    /// Модель представления страницы редактора.
    /// </summary>
    public class PlaygroundViewModel : ViewModel
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

        /// <summary>
        /// Выбранные вершины
        /// </summary>
        public List<VisualVertex> SelectedVertices
        {
            get
            {
                return _visualEditorService.SelectedVertices;
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
        #endregion

        #region constructor
        /// <summary>
        /// Конструктор.
        /// </summary>
        public PlaygroundViewModel(
            IDataHeandlerService dataHeandler,
            IMapper mapper,
            INavigationService navigationService,
            IVisualEditorService visualEditorService)
        {
            _dataHeandler = dataHeandler;
            _mapper = mapper;
            _navigationService = navigationService;
            _visualEditorService = visualEditorService;

            ChangeMouseMode = new RelayCommand(SetMouseMode);
            ClickOnField = new RelayCommand(ClickOnFieldCommand);
            ClickOnVertex = new RelayCommand(ClickOnVertexCommand);
            ClickOnConnection = new RelayCommand(ClickOnConnectionCommand);
            MoveVertex = new RelayCommand(MoveVertexCommand);
            SaveGraph = new RelayCommand(SaveGraphCommand);
            LoadGraph = new RelayCommand(LoadGraphCommand);
            GoBack = new RelayCommand(GoBackCommand);

            _visualEditorService.AddVertex(new Point(200, 200), 10, "default");
            //_visualEditorService.AddVertex(new Point(100, 100), 10, "default");
            //_visualEditorService.AddVertex(new Point(100, 200), 10, "default");


            //AddConnection((Vertices[0], Vertices[1]));

            //AddConnection((Vertices[0], Vertices[2]));
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
        /// <param name="vertex">Удаляемая вершина.</param>
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
                    connection.OnDelete += DeleteConnection;
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

        private void GoBackCommand(object parameter)
        {
            _navigationService.NavigateTo<MainMenuViewModel>();
        }
        #endregion
    }
}
