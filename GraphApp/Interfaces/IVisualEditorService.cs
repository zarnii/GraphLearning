using GraphApp.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace GraphApp.Interfaces
{
    /// <summary>
    /// Сервис визуального редактора.
    /// </summary>
    public interface IVisualEditorService
    {
        /// <summary>
        /// Режим мыши.
        /// </summary>
        public MouseMode MouseMode { get; }

        /// <summary>
        /// Связи.
        /// </summary>
        ObservableCollection<VisualConnection> Connections { get; }

        /// <summary>
        /// Выбранный элемент графа.
        /// </summary>
        public ViewModel.ViewModel SelectedGraphElement { get; set; }

        /// <summary>
        /// Выбранные вершины для соединения.
        /// </summary>
        List<VisualVertex> SelectedVerticesForConnection { get; }

        /// <summary>
        /// Вершины.
        /// </summary>
        ObservableCollection<VisualVertex> Vertices { get; }

        int CanvasWidth { get; set; }

        int CanvasHeight { get; set; }

        /// <summary>
        /// Добавление связь.
        /// </summary>
        /// <param name="connectedVertices">Соедененные вершины.</param>
        /// <param name="weight">Вес связи.</param>
        /// <param name="connectionType">Тип связи.</param>
        void AddConnection((VisualVertex, VisualVertex) connectedVertices,
            double thickness,
            double weight = 0,
            ConnectionType connectionType = ConnectionType.NonDirectional);

        /// <summary>
        /// Создание вершины.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <param name="radius">Радиус.</param>
        /// <param name="name">Имя.</param>
        void AddVertex(Point point, int radius, string name, Color color);

        /// <summary>
        /// Удаление связи.
        /// </summary>
        /// <param name="connection">Удаляемая связь.</param>
        void DeleteConnection(VisualConnection connection);

        /// <summary>
        /// Удаление вершины.
        /// </summary>
        /// <param name="vertex">Удаляемая вершина.</param>
        void DeleteVertex(VisualVertex vertex);

        /// <summary>
        /// Передвижение вершины.
        /// </summary>
        /// <param name="vertex">Вершина.</param>
        /// <param name="x">Новая координата X.</param>
        /// <param name="y">Новая координата Y.</param>
        void MoveVertex(VisualVertex vertex, int x, int y);

        /// <summary>
        /// Установка режима мыши.
        /// </summary>
        /// <param name="mode">Режим мыши.</param>
        void SetMouseMode(MouseMode mode);

        /// <summary>
        /// Создание матрицы смежности
        /// </summary>
        /// <returns>Матрица смежности.</returns>
        AdjacencyMatrix CreateAdjacencyMatrix();

        /// <summary>
        /// Создание матрица инцидентности.
        /// </summary>
        /// <returns>Матрица инцидентности.</returns>
        IncidenceMatrix CreateIncidenceMatrix();

        /// <summary>
        /// Очистка поля.
        /// </summary>
        void Clear();
    }
}