using GraphApp.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace GraphApp.Interfaces
{
    /// <summary>
    /// Сервис визуального редактора.
    /// </summary>
    public interface IVisualEditorService
    {
        /// <summary>
        /// Связи.
        /// </summary>
        ObservableCollection<VisualConnection> Connections { get; }

        /// <summary>
        /// Выбранные вершины.
        /// </summary>
        List<VisualVertex> SelectedVertices { get; }

        /// <summary>
        /// Вершины.
        /// </summary>
        ObservableCollection<VisualVertex> Vertices { get; }

        /// <summary>
        /// Добавление связь.
        /// </summary>
        /// <param name="connectedVertices">Соедененные вершины.</param>
        /// <param name="weight">Вес связи.</param>
        /// <param name="connectionType">Тип связи.</param>
        void AddConnection((VisualVertex, VisualVertex) connectedVertices, 
            double weight = 0, 
            ConnectionType connectionType = ConnectionType.NonDirectional);
        
        /// <summary>
        /// Создание вершины.
        /// </summary>
        /// <param name="point">Точка.</param>
        void AddVertex(Point point);

        /// <summary>
        /// Нажатие на связь.
        /// </summary>
        /// <param name="connection">Связь.</param>
        void ClickOnConnection(VisualConnection connection);

        /// <summary>
        /// Нажатие на поле.
        /// </summary>
        /// <param name="mouseButtonEventArgs">События мыши.</param>
        void ClickOnField(MouseButtonEventArgs mouseButtonEventArgs);

        /// <summary>
        /// Нажатие на вершину.
        /// </summary>
        /// <param name="vertex">Вершина.</param>
        void ClickOnVertex(VisualVertex vertex);

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
        /// <param name="dragDeltaEventArgs">События передвижения.</param>
        void MoveVertex(DragDeltaEventArgs dragDeltaEventArgs);

        /// <summary>
        /// Установка режима мыши.
        /// </summary>
        /// <param name="mode">Режим мыши.</param>
        void SetMouseMode(MouseMode mode);
    }
}