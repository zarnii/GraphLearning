using GraphApp.Interfaces;
using GraphApp.Model;
using System;
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
            SelectedVerticesForConnection = new List<VisualVertex>(2);

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
        public VisualConnection AddConnection(
            (VisualVertex, VisualVertex) connectedVertices,
            double thickness,
            double weight = 0,
            ConnectionType connectionType = ConnectionType.NonDirectional)
        {
            var verticesIsConnected = CheckVerticesIsConnected(connectedVertices);

            if (verticesIsConnected)
            {
                return null;
            }

            var connection = new VisualConnection(
                connectedVertices,
                Connections.Count + 1,
                thickness,
                weight,
                connectionType
            );
            connection.OnDelete += DeleteConnection;

            Connections.Add(connection);

            return connection;
        }

        /// <summary>
        /// Создание вершины.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <param name = "radius" > Радиус.</param>
        /// <param name="name">Имя.</param>
        public VisualVertex AddVertex(Point point, int radius, string name, Color color)
        {
            var vertex = new VisualVertex(
                ((int)point.X, (int)point.Y),
                radius,
                Vertices.Count + 1,
                color,
                name
            );

            Vertices.Add(vertex);

            return vertex;
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
        public void MoveVertex(VisualVertex vertex, int x, int y)
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
         /// <returns>Матрица смежности.</returns>
        public AdjacencyMatrix CreateAdjacencyMatrix()
        {
            var vertices = Vertices.Select(v => v.Vertex).ToList();
            var connections = Connections.Select(v => v.Connection).ToList();

            return new AdjacencyMatrix(vertices, connections);
        }

        /// <summary>
        /// Создание матрица инцидентности.
        /// </summary>
        /// <returns>Матрица инцидентности.</returns>
        public IncidenceMatrix CreateIncidenceMatrix()
        {
            var vertices = Vertices.Select(v => v.Vertex).ToList();
            var connection = Connections.Select(v => v.Connection).ToList();

            return new IncidenceMatrix(vertices, connection);
        }

        /// <summary>
        /// Очистка поля.
        /// </summary>
        public void Clear()
        {
            Vertices.Clear();
            Connections.Clear();
        }
        #endregion

        #region private methods
        /// <summary>
        /// Проверка на наличие связи между двумя вершинами.
        /// </summary>
        /// <param name="vertices">Вершины.</param>
        /// <returns>True, если две вершины уже соединены.</returns>
        private bool CheckVerticesIsConnected((VisualVertex, VisualVertex) vertices)
        {
            var verticesTuples = Connections.Select(c => c.ConnectedVertices);
            var flag = verticesTuples.Any(v => v == vertices || v == (vertices.Item2, vertices.Item1));

            return flag;
        }
        #endregion
    }
}
