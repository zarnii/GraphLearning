using GraphApp.Interfaces;
using GraphApp.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
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
        /// Режим мыши.
        /// </summary>
        private static MouseMode _mouseMode;
        #endregion

        #region properties
        /// <summary>
        /// Выбранные вершины.
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
        public VisualEditorService()
        {
            Vertices = new ObservableCollection<VisualVertex>();
            Connections = new ObservableCollection<VisualConnection>();
            SelectedVertices = new List<VisualVertex>();
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
            double weight = 0,
            ConnectionType connectionType = ConnectionType.NonDirectional)
        {
            var connection = new VisualConnection(connectedVertices, weight, connectionType);
            connection.OnDelete += DeleteConnection;

            Connections.Add(connection);
        }

        /// <summary>
        /// Создание вершины.
        /// </summary>
        /// <param name="point">Точка.</param>
        public void AddVertex(Point point)
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
        /// Нажатие на связь.
        /// </summary>
        /// <param name="connection">Связь.</param>
        public void ClickOnConnection(VisualConnection connection)
        {
            if (_mouseMode == MouseMode.Delete)
            {
                DeleteConnection(connection);
            }
        }

        /// <summary>
        /// Нажатие на поле.
        /// </summary>
        /// <param name="mouseButtonEventArgs">События мыши.</param>
        public void ClickOnField(MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (_mouseMode != MouseMode.Create)
            {
                return;
            }

            var mbEventArgs = mouseButtonEventArgs;
            var point = mbEventArgs.GetPosition((UIElement)mbEventArgs.OriginalSource);

            AddVertex(point);
        }

        /// <summary>
        /// Нажатие на вершину.
        /// </summary>
        /// <param name="vertex">Вершина.</param>
        public void ClickOnVertex(VisualVertex vertex)
        {
            if (_mouseMode == MouseMode.Delete)
            {
                DeleteVertex(vertex);
            }
            else if (_mouseMode == MouseMode.Connect)
            {
                if (SelectedVertices.Count < 2)
                {
                    SelectedVertices.Add(vertex);
                }

                if (SelectedVertices.Count == 2)
                {
                    AddConnection((SelectedVertices[0], SelectedVertices[1]));
                    SelectedVertices.Clear();
                }
            }
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
        /// <param name="dragDeltaEventArgs">События передвижения.</param>
        public void MoveVertex(DragDeltaEventArgs dragDeltaEventArgs)
        {
            if (_mouseMode != MouseMode.Default)
            {
                return;
            }

            var ddEventArgs = dragDeltaEventArgs;
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
        /// Установка режима мыши.
        /// </summary>
        /// <param name="mode">Режим мыши.</param>
        public void SetMouseMode(MouseMode mode)
        {
            _mouseMode = mode;
            SelectedVertices.Clear();
        }
        #endregion

        #region private methods
        #endregion
    }
}
