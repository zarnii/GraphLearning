using GraphApp.Command;
using GraphApp.Interfaces;
using GraphApp.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace GraphApp.ViewModel
{
    public class PracticViewModel : ViewModel
    {
        #region fields
        /// <summary>
        /// Сервис визуального редактора.
        /// </summary>
        private IVisualEditorService _visualEditorService;

        /// <summary>
        /// Поставщик практических заданий.
        /// </summary>
        private IPracticProvider _practicProvider;
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
        /// Связи.
        /// </summary>
        public ObservableCollection<VisualConnection> Connections
        {
            get
            {
                return _visualEditorService.Connections;
            }
        }

        /// <summary>
        /// Выбранные вершины.
        /// </summary>
        public List<VisualVertex> SelectedVertices
        {
            get
            {
                return _visualEditorService.SelectedVertices;
            }
        }

        /// <summary>
        /// Вершины.
        /// </summary>
        public ObservableCollection<VisualVertex> Vertices
        {
            get
            {
                return _visualEditorService.Vertices;
            }
        }

        public string PracticTaskTitle
        {
            get
            {
                return _practicProvider?.CurrentPractic?.Title;
            }
        }

        public string PracticTaskText
        {
            get
            {
                return _practicProvider?.CurrentPractic?.Text;
            }
        }
        #endregion

        #region constructor
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="visualEditorService">Сервис визуального редактора.</param>
        /// <param name="practicProvider">Поставщик практических заданий.</param>
        public PracticViewModel(IVisualEditorService visualEditorService, 
            IPracticProvider practicProvider)
        {
            _visualEditorService = visualEditorService;

            ChangeMouseMode = new RelayCommand(SetMouseMode);
            ClickOnField = new RelayCommand(ClickOnFieldCommand);
            ClickOnVertex = new RelayCommand(ClickOnVertexCommand);
            ClickOnConnection = new RelayCommand(ClickOnConnectionCommand);
            MoveVertex = new RelayCommand(MoveVertexCommand);
        }
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
            _visualEditorService.ClickOnField((MouseButtonEventArgs)parameter);
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
        /// Команда добавление новой вершины.
        /// </summary>
        /// <param name="point">Координаты вершины.</param>
        private void AddVertex(Point point)
        {
            _visualEditorService.AddVertex(point);
        }

        /// <summary>
        /// Удаление вершины.
        /// </summary>
        /// <param name="vertex"></param>
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
            _visualEditorService.MoveVertex((DragDeltaEventArgs)parameter);
        }
        #endregion
    }
}