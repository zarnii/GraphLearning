using GraphApp.Command;
using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.Model.Exception;
using GraphApp.Model.Serializing;
using GraphApp.Services;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;


namespace GraphApp.ViewModel
{
    /// <summary>
    /// Модель представления страницы редактора.
    /// </summary>
    public class PlaygroundViewModel : VisualEditorViewModel
    {
        #region fields
        /// <summary>
        /// Обработчик данных.
        /// </summary>
        private IDataHandlerService _dataHandler;

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
        /// Команда сохранения графа.
        /// </summary>
        public ICommand SaveGraph { get; private set; }

        /// <summary>
        /// Команда загрузки графа.
        /// </summary>
        public ICommand LoadGraph { get; private set; }

        /// <summary>
        /// Команда сохранения графа как изображение.
        /// </summary>
        public ICommand SaveGraphAsPng { get; private set; }

        /// <summary>
        /// Переход назад.
        /// </summary>
        public ICommand GoBack { get; private set; }
        #endregion

        #region constructor
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="vertexViewModel">Модель представления вершин.</param>
        /// <param name="connectionViewModel">Модель представления связи.</param>
        /// <param name="visualEditorService">Сервис графического редактора.</param>
        /// <param name="dataHandler">Обработчик данных.</param>
        /// <param name="navigationService">Сервис навигации.</param>
        /// <param name="mapper">Маппер.</param>
        public PlaygroundViewModel(
            VertexViewModel vertexViewModel,
            ConnectionViewModel connectionViewModel,
            IVisualEditorService visualEditorService,
            IDataHandlerService dataHandler,
            INavigationService navigationService,
            IMapper mapper)
            : base(vertexViewModel, connectionViewModel, visualEditorService)
        {

            _dataHandler = dataHandler;
            _mapper = mapper;
            _navigationService = navigationService;

            SaveGraph = new RelayCommand(SaveGraphCommand);
            LoadGraph = new RelayCommand(LoadGraphCommand);
            GoBack = new RelayCommand(GoBackCommand);
            SaveGraphAsPng = new RelayCommand(SaveGraphAsPngCommand);
        }
        #endregion

        #region public methods
        #endregion

        #region private methods
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

            _dataHandler.Save<SerializableData>(new SerializableData()
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
                var data = _dataHandler.Load<SerializableData>();

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
                    SubscribeConnectionOnDelete(connection);
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
        /// Сохранение графа как изображение.
        /// </summary>
        /// <param name="parameter"></param>
        private void SaveGraphAsPngCommand(object parameter)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "Png files (*.png)|*.png";

            if (dialog.ShowDialog() == false)
            {
                return;
            }

            var saver = new GraphImageSaver();
            saver.SaveAsPng(Vertices, Connections, dialog.FileName, CanvasWidth, CanvasHeight, ConnectionWeightOpasity, ConnectionNumberOpasity);
        }

        /// <summary>
        /// Переход назад.
        /// </summary>
        /// <param name="parameter"></param>
        private void GoBackCommand(object parameter)
        {
            _navigationService.NavigateTo<MainMenuViewModel>();
        }
        #endregion
    }
}
