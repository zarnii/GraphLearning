using GraphApp.Interfaces;
using GraphApp.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace GraphApp.Services
{
    /// <summary>
    /// Сервис визуального редактора.
    /// </summary>
    public class VisualEditorService : IVisualEditorService
    {
        #region fields
        /// <summary>
        /// Цвет вершины по умолчанию.
        /// </summary>
        private Color _defaultVertexColor = Colors.Red;

        /// <summary>
        /// Режим мыши.
        /// </summary>
        private MouseMode _mouseMode;
        #endregion

        #region properties
        /// <summary>
        /// Режим мыши.
        /// </summary>
        public MouseMode MouseMode
        {
            get
            {
                return _mouseMode;
            }
        }

        /// <summary>
        /// Выбранный элемент графа.
        /// </summary>
        public ViewModel.ViewModel SelectedGraphElement { get; set; }

        /// <summary>
        /// Выбранные вершины для соединения.
        /// </summary>
        public List<VisualVertex> SelectedVerticesForConnection { get; private set; }

        /// <summary>
        /// Лист вершин.
        /// </summary>
        public ObservableCollection<VisualVertex> Vertices { get; private set; }

        /// <summary>
        /// Лист связей.
        /// </summary>
        public ObservableCollection<VisualConnection> Connections { get; private set; }

        /// <summary>
        /// Ширина графического поля.
        /// </summary>
        public int CanvasWidth { get; set; }

        /// <summary>
        /// Высота графического поля.
        /// </summary>
        public int CanvasHeight { get; set; }
        #endregion

        #region constructor
        /// <summary>
        /// Конструктор.
        /// </summary>
        public VisualEditorService()
        {
            Vertices = new ObservableCollection<VisualVertex>();
            Connections = new ObservableCollection<VisualConnection>();
            SelectedVerticesForConnection = new List<VisualVertex>();

            CanvasWidth = 1000;
            CanvasHeight = 900;
        }
        #endregion

        #region public methods
        /// <summary>
        /// Добавление связь.
        /// </summary>
        /// <param name="connectedVertices">Соедененные вершины.</param>
        /// <param name="weight">Вес связи.</param>
        /// <param name="connectionType">Тип связи.</param>
        public void AddConnection(
            (VisualVertex, VisualVertex) connectedVertices,
            double thickness,
            double weight = 0,
            ConnectionType connectionType = ConnectionType.NonDirectional)
        {
            var connection = new VisualConnection(
                connectedVertices,
                Connections.Count + 1,
                thickness,
                weight,
                connectionType
            );
            connection.OnDelete += DeleteConnection;

            Connections.Add(connection);
        }

        /// <summary>
        /// Создание вершины.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <param name = "radius" > Радиус.</param>
        /// <param name="name">Имя.</param>
        public void AddVertex(Point point, int radius, string name, Color color)
        {
            Vertices.Add(new VisualVertex(
                (point.X, point.Y),
                radius,
                Vertices.Count + 1,
                color,
                name
            ));
        }

        /// <summary>
        /// Удаление связи.
        /// </summary>
        /// <param name="connection">Удаляемая связь.</param>
        public void DeleteConnection(VisualConnection connection)
        {
            if (connection.OnDelete != null)
            {
                connection.OnDelete -= DeleteConnection;
                Connections.Remove(connection);
            }
        }

        /// <summary>
        /// Удаление вершины.
        /// </summary>
        /// <param name="vertex">Удаляемая вершина.</param>
        public void DeleteVertex(VisualVertex vertex)
        {
            vertex.Delete();
            Vertices.Remove(vertex);
        }

        /// <summary>
        /// Передвижение вершины.
        /// </summary>
        /// <param name="vertex">Вершина.</param>
        /// <param name="x">Новая координата X.</param>
        /// <param name="y">Новая координата Y.</param>
        public void MoveVertex(VisualVertex vertex, double x, double y)
        {
            if (_mouseMode != MouseMode.Default)
            {
                return;
            }

            if (vertex.X + x < 0
                || vertex.Y + y < 0)
            {
                return;
            }

            if (vertex.X + vertex.Radius + x > CanvasWidth
                || vertex.Y + vertex.Radius + y > CanvasHeight)
            {
                return;
            }

            vertex.X += x;
            vertex.Y += y;
        }

        /// <summary>
        /// Установка режима мыши.
        /// </summary>
        /// <param name="mode">Режим мыши.</param>
        public void SetMouseMode(MouseMode mode)
        {
            _mouseMode = mode;
            SelectedVerticesForConnection.Clear();
        }

        /// <summary>
        /// Создание матрицы смежности.
        /// </summary>
        /// <returns></returns>
        public AdjacencyMatrix CreateAdjacencyMatrix()
        {
            var vertices = Vertices.Select(v => v.Vertex).ToList();
            var connections = Connections.Select(v => v.Connection).ToList();

            return new AdjacencyMatrix(vertices, connections);
        }
        #endregion

        #region private methods
        #endregion
    }
}
