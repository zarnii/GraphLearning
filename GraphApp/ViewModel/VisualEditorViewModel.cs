﻿using GraphApp.Command;
using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.Model.Exception;
using GraphApp.Model.Serializing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;


namespace GraphApp.ViewModel
{
    /// <summary>
    /// Модель представления страницы редактора.
    /// </summary>
    public class VisualEditorViewModel : ViewModel
    {
        #region fields
        /// <summary>
        /// Ширина вершины по умолчанию.
        /// </summary>
        private int _defaultVertexWidth = 20;

        /// <summary>
        /// Высота вершины по умолчанию.
        /// </summary>
        private int _defaultVertexHeight = 20;

        /// <summary>
        /// Цвет вершины по умолчанию.
        /// </summary>
        private Color _defaultVertexColor = Colors.Red;

        /// <summary>
        /// Команда изменения режима мыши.
        /// </summary>
        private ICommand _changeMouseMode;

        /// <summary>
        /// Команда добавыления вершины.
        /// </summary>
        private ICommand _clickOnField;

        /// <summary>
        /// Команда нажатия на вершину.
        /// </summary>
        private ICommand _clickOnVertex;

        /// <summary>
        /// Команда нажатия на связь.
        /// </summary>
        private ICommand _clickOnConnection;

        /// <summary>
        /// Команда перемещения вершины.
        /// </summary>
        private ICommand _moveVertex;

        /// <summary>
        /// Команда сохранения графа.
        /// </summary>
        private ICommand _saveGraph;

        /// <summary>
        /// Команда загрузки графа.
        /// </summary>
        private ICommand _loadGraph;

        /// <summary>
        /// Команда перехода в предыдущее окно.
        /// </summary>
        private ICommand _goBack;

        /// <summary>
        /// Режим мыши.
        /// </summary>
        private static MouseMode _mouseMode;

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
        #endregion

        #region properties
        /// <summary>
        /// Свойство команды изменения режима мыши.
        /// </summary>
        public ICommand ChangeMouseMode
        {
            get
            {
                return _changeMouseMode;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Команда является пустой.");
                }

                _changeMouseMode = value;
            }
        }

        /// <summary>
        /// Команда добавления вершины.
        /// </summary>
        public ICommand ClickOnField
        {
            get
            {
                return _clickOnField;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Пустая команда нажатия на поле.");
                }

                _clickOnField = value;
            }
        }

        /// <summary>
        /// Команда нажатия на вершину.
        /// </summary>
        public ICommand ClickOnVertex
        {
            get
            {
                return _clickOnVertex;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Пустая команда нажатия на вершину.");
                }

                _clickOnVertex = value;
            }
        }

        /// <summary>
        /// Команда нажатия на связь.
        /// </summary>
        public ICommand ClickOnConnection
        {
            get
            {
                return _clickOnConnection;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Пустая команда нажатия на связь.");
                }

                _clickOnConnection = value;
            }
        }

        /// <summary>
        /// Команда перемещения вершины.
        /// </summary>
        public ICommand MoveVertex
        {
            get
            {
                return _moveVertex;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Пустая команда передмещения верин");
                }

                _moveVertex = value;
            }
        }

        /// <summary>
        /// Команда сохранения графа.
        /// </summary>
        public ICommand SaveGraph
        {
            get
            {
                return _saveGraph;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Пустая команда сохранения графа.");
                }

                _saveGraph = value;
            }
        }

        /// <summary>
        /// Команда загрузки графа.
        /// </summary>
        public ICommand LoadGraph
        {
            get
            {
                return _loadGraph;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Пустая команда загрузки графа.");
                }

                _loadGraph = value;
            }
        }

        public ICommand GoBack
        {
            get
            {
                return _goBack;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Пустая команда перехода в предыдущее окно.");
                }

                _goBack = value;
            }
        }

        /// <summary>
        /// Выбранные вершины
        /// </summary>
        public List<VisualVertex> SelectedVertices { get; private set; }

        /// <summary>
        /// Лист вершин.
        /// </summary>
        public ObservableCollection<VisualVertex> Vertices { get; private set; }

        /// <summary>
        /// Лист связей.
        /// </summary>
        public ObservableCollection<VisualConnection> Connections { get; private set; }
        #endregion

        #region constructor
        /// <summary>
        /// Конструктор.
        /// </summary>
        public VisualEditorViewModel(IDataHeandlerService dataHeandler, IMapper mapper, INavigationService navigationService)
        {
            _dataHeandler = dataHeandler;
            _mapper = mapper;
            _navigationService = navigationService;

            Vertices = new ObservableCollection<VisualVertex>();
            Connections = new ObservableCollection<VisualConnection>();
            SelectedVertices = new List<VisualVertex>();

            ChangeMouseMode = new RelayCommand(SetMouseMode);
            ClickOnField = new RelayCommand(ClickOnFieldCommand);
            ClickOnVertex = new RelayCommand(ClickOnVertexCommand);
            ClickOnConnection = new RelayCommand(ClickOnConnectionCommand);
            MoveVertex = new RelayCommand(MoveVertexCommand);
            SaveGraph = new RelayCommand(SaveGraphCommand);
            LoadGraph = new RelayCommand(LoadGraphCommand);
            GoBack = new RelayCommand(GoBackCommand);


            AddVertex(new Point(200, 200));
            AddVertex(new Point(100, 100));
            AddVertex(new Point(100, 200));


            AddConnection((Vertices[0], Vertices[1]));

            AddConnection((Vertices[0], Vertices[2]));
        }
        #endregion

        #region public methods
        #endregion

        #region private methods
        /// <summary>
        /// Изменение режима мыши.
        /// </summary>
        /// <param name="mode">Режим мыши.</param>
        private void SetMouseMode(object mode)
        {
            _mouseMode = (MouseMode)mode;
            SelectedVertices.Clear();
        }

        /// <summary>
        /// Команда нажатия на поле.
        /// </summary>
        /// <param name="parameter">Аргументы события.</param>
        private void ClickOnFieldCommand(object parameter)
        {
            if (_mouseMode != MouseMode.Create)
            {
                return;
            }

            var mbEventArgs = (MouseButtonEventArgs)parameter;
            var point = mbEventArgs.GetPosition((Rectangle)mbEventArgs.OriginalSource);

            AddVertex(point);
        }

        /// <summary>
        /// Команда нажатия на вершину.
        /// </summary>
        /// <param name="parameter">Нажатая вершина.</param>
        private void ClickOnVertexCommand(object parameter)
        {
            if (_mouseMode == MouseMode.Delete)
            {
                DeleteVertex((VisualVertex)parameter);
            }
            else if (_mouseMode == MouseMode.Connect)
            {
                if (SelectedVertices.Count < 2)
                {
                    SelectedVertices.Add((VisualVertex)parameter);
                }

                if (SelectedVertices.Count == 2)
                {
                    AddConnection((SelectedVertices[0], SelectedVertices[1]));
                    SelectedVertices.Clear();
                }
            }
        }

        /// <summary>
        /// Команда нажатия на связь.
        /// </summary>
        /// <param name="parameter"></param>
        private void ClickOnConnectionCommand(object parameter)
        {
            if (_mouseMode == MouseMode.Delete)
            {
                DeleteConnection((VisualConnection)parameter);
            }
        }

        /// <summary>
        /// Команда добавление новой вершины.
        /// </summary>
        /// <param name="point">Координаты вершины.</param>
        private void AddVertex(Point point)
        {
            Vertices.Add(new VisualVertex(
                (point.X - _defaultVertexWidth / 2, point.Y - _defaultVertexHeight / 2),
                _defaultVertexWidth,
                _defaultVertexHeight,
                Vertices.Count + 1,
                _defaultVertexColor
            ));
        }

        /// <summary>
        /// Удаление вершины.
        /// </summary>
        /// <param name="vertex"></param>
        private void DeleteVertex(VisualVertex vertex)
        {
            vertex.Delete();
            Vertices.Remove(vertex);
        }


        /// <summary>
        /// Создание связи.
        /// </summary>
        /// <param name="connectedVertices">Соеденяемые вершины.</param>
        private void AddConnection((VisualVertex, VisualVertex) connectedVertices,
            double weight = 0,
            ConnectionType connectionType = ConnectionType.NonDirectional)
        {
            var connection = new VisualConnection(connectedVertices, weight, connectionType);
            connection.OnDelete += DeleteConnection;

            Connections.Add(connection);
        }

        /// <summary>
        /// Удаление связи.
        /// </summary>
        /// <param name="connection">Удаляемая связь.</param>
        private void DeleteConnection(VisualConnection connection)
        {
            connection.OnDelete -= DeleteConnection;
            Connections.Remove(connection);
        }

        /// <summary>
        /// Передвижение вершин.
        /// </summary>
        /// <param name="parameter">Аргументы события.</param>
        private void MoveVertexCommand(object parameter)
        {
            if (_mouseMode != MouseMode.Default)
            {
                return;
            }

            var ddEventArgs = (DragDeltaEventArgs)parameter;
            var vertex = (VisualVertex)((FrameworkElement)ddEventArgs.OriginalSource).DataContext;

            if (vertex.X + ddEventArgs.HorizontalChange < 0
                || vertex.Y + ddEventArgs.VerticalChange < 0)
            {
                return;
            }

            vertex.X += ddEventArgs.HorizontalChange;
            vertex.Y += ddEventArgs.VerticalChange;
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
