﻿using GraphApp.Interfaces;
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
            var connection = new VisualConnection(
                connectedVertices,
                Connections.Count + 1,
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
        public void AddVertex(Point point, int radius, string name)
        {
            Vertices.Add(new VisualVertex(
                (point.X, point.Y),
                radius * 2,
                radius * 2,
                Vertices.Count + 1,
                _defaultVertexColor,
                name
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
        /// <param name="vertex">Вершина.</param>
        /// <param name="x">Новая координата X.</param>
        /// <param name="y">Новая координата Y.</param>
        public void MoveVertex(VisualVertex vertex, double x, double y)
        {
            if (_mouseMode != MouseMode.Default)
            {
                return;
            }

            /*
            var ddEventArgs = dragDeltaEventArgs;
            var vertex = (VisualVertex)((FrameworkElement)ddEventArgs.OriginalSource).DataContext;
            */

            if (vertex.X + x < 0
                || vertex.Y + y < 0)
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
            SelectedVertices.Clear();
        }
        #endregion

        #region private methods
        #endregion
    }
}