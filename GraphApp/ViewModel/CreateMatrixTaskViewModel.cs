using DocumentFormat.OpenXml.InkML;
using GraphApp.Command;
using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.Services.FactoryViewModel;
using GraphApp.ViewModel.Verify;
using Gu.Wpf.DataGrid2D;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace GraphApp.ViewModel
{
    /// <summary>
    /// Модель представления составления матрицы по графу.
    /// </summary>
    public class CreateMatrixTaskViewModel : ViewModel
    {
        #region fields
        /// <summary>
        /// Сервис навигации.
        /// </summary>
        private INavigationService _navigationService;

        /// <summary>
        /// Сервис контроля доступа.
        /// </summary>
        private IAccessControlService _accessControlService;

        /// <summary>
        /// Фабрика VerifyCreateMatrixTask.
        /// </summary>
        private IFactoryViewModel _factoryVerifyCreateMatrixTask;

        /// <summary>
        /// Матрица смежности.
        /// </summary>
        private BaseMatrix _matrix;

        /// <summary>
        /// Матрица, строимая пользователем.
        /// </summary>
        private int[,] _userMatrix;
        #endregion

        #region properties
        /// <summary>
        /// Комаанда перехода назад.
        /// </summary>
        public ICommand GoBack { get; private set; }

        /// <summary>
        /// Команда проверки задания.
        /// </summary>
        public ICommand VerifyTask { get; private set; }

        /// <summary>
        /// Команда выбора ячейки.
        /// </summary>
        public ICommand SelectCell { get; private set; }

        /// <summary>
        /// Коллекция вершин.
        /// </summary>
        public IList<VisualVertex> Vertices { get; private set; }

        /// <summary>
        /// Коллекция связей.
        /// </summary>
        public IList<VisualConnection> Connections { get; private set; }

        /// <summary>
        /// Прозрачность номера связи.
        /// </summary>
        public int ConnectionNumberOpasity { get; private set; }

        /// <summary>
        /// Прозрачность веса связи.
        /// </summary>
        public int ConnectionWeightOpasity { get; private set; }

        /// <summary>
        /// Описание колонок таблицы.
        /// </summary>
        public string[] ColumnsDescription 
        {
            get
            {
                return _matrix.ColumnsDescription;
            }
        }

        /// <summary>
        /// Описание строк таблицы.
        /// </summary>
        public string[] RowsDescription 
        {
            get
            {
                return _matrix.RowsDescription;
            }
        }

        /// <summary>
        /// Матрица, строимая пользователем.
        /// </summary>
        public int[,] UserMatrix
        {
            get
            {
                return _userMatrix;
            }
            private set
            {
                _userMatrix = value;
                OnPropertyChanged(nameof(UserMatrix));
            }
        }

        /// <summary>
        /// Выбранная ячейка.
        /// </summary>
        public RowColumnIndex SelectedCell { get; set; }

        /// <summary>
        /// заголовок.
        /// </summary>
        public string Title { get; private set; }
        #endregion

        #region constructor
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="navigationService">Сервис навигации.</param>
        /// <param name="accessControlService">Сервис констроля доступа.</param>
        /// <param name="factoryVerifyCreateMatrixTask">Фабрика VerifyCreateMatrixTaskViewModel.</param>
        public CreateMatrixTaskViewModel(
            INavigationService navigationService,
            IAccessControlService accessControlService,
            [FromKeyedServices(typeof(FactoryVerifyCreateMatrixTask))]IFactoryViewModel factoryVerifyCreateMatrixTask)
        {
            _navigationService = navigationService;
            _accessControlService = accessControlService;
            ConnectionNumberOpasity = 1;
            ConnectionWeightOpasity = 0;
            Vertices = ((CreateMatrixTask)_accessControlService.CurrentEducationMaterial.EducationMaterial).Vertices;
            Connections = ((CreateMatrixTask)_accessControlService.CurrentEducationMaterial.EducationMaterial).Connections;

            GoBack = new RelayCommand(GoBackCommand);
            VerifyTask = new RelayCommand(VerifyTaskCommand);

            Init();
            _factoryVerifyCreateMatrixTask = factoryVerifyCreateMatrixTask;
        }
        #endregion

        #region private methods
        /// <summary>
        /// Инициализация.
        /// </summary>
        private void Init()
        {
            var rowCount = 0;
            var columnCount = 0;
            var matrixType = ((CreateMatrixTask)_accessControlService
                .CurrentEducationMaterial
                .EducationMaterial).CreatableMatrixType;

            if (matrixType == CreatableMatrixType.AdjacencyMatrix)
            {
                _matrix = new AdjacencyMatrix(
                    Vertices.Select(v => v.Vertex).ToArray(),
                    Connections.Select(c => c.Connection).ToArray()
                );

                rowCount = Vertices.Count;
                columnCount = rowCount;
                SelectCell = new RelayCommand(SelectCellAdjacencyMatrixCommand);
                Title = "Постройте матрицу смежности по графу";
            }
            else
            {
                _matrix = new IncidenceMatrix(
                    Vertices.Select(v => v.Vertex).ToArray(),
                    Connections.Select(c => c.Connection).ToArray()
                );

                rowCount = Connections.Count;
                columnCount = Vertices.Count;
                SelectCell = new RelayCommand(SelectCellIncidenceMatrixCommand);
                Title = "Постройте матрицу инцидентности по графу";
            }

            UserMatrix = new int[rowCount, columnCount];
        }

        /// <summary>
        /// Проверка задания.
        /// </summary>
        /// <param name="parameter"></param>
        private void VerifyTaskCommand(object parameter)
        {
            _navigationService.NavigateTo(_factoryVerifyCreateMatrixTask, 
                new object[2] 
                {
                    UserMatrix,
                    (Vertices, Connections)
                }
            );
        }

        /// <summary>
        /// Выбор ячейки.
        /// </summary>
        /// <param name="parameter"></param>
        private void SelectCellAdjacencyMatrixCommand(object parameter)
        {
            var matrix = (int[,])UserMatrix.Clone();

            if (matrix[SelectedCell.Row, SelectedCell.Column] == 0)
            {
                matrix[SelectedCell.Row, SelectedCell.Column] = 1;
            }
            else
            {
                matrix[SelectedCell.Row, SelectedCell.Column] = 0;
            }
            UserMatrix = matrix;
        }

        /// <summary>
        /// Выбор ячейки.
        /// </summary>
        /// <param name="parameter"></param>
        private void SelectCellIncidenceMatrixCommand(object parameter)
        {
            var matrix = (int[,])UserMatrix.Clone();

            if (matrix[SelectedCell.Row, SelectedCell.Column] == -1)
            {
                matrix[SelectedCell.Row, SelectedCell.Column] = 0;
            }
            else if (matrix[SelectedCell.Row, SelectedCell.Column] == 0)
            {
                matrix[SelectedCell.Row, SelectedCell.Column] = 1;
            }
            else
            {
                matrix[SelectedCell.Row, SelectedCell.Column] = -1;
            }

            UserMatrix = matrix;
        }

        /// <summary>
        /// Переход назад.
        /// </summary>
        /// <param name="parameter"></param>
        private void GoBackCommand(object parameter)
        {
            _navigationService.NavigateTo<EducationViewModel>();
        }
        #endregion
    }
}
