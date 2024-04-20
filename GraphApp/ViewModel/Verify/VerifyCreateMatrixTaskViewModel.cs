using GraphApp.Command;
using GraphApp.Interfaces;
using GraphApp.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphApp.ViewModel.Verify
{
    public class VerifyCreateMatrixTaskViewModel : ViewModel
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
        #endregion

        #region properties
        /// <summary>
        /// Команда перехода назад.
        /// </summary>
        public ICommand GoBack { get; private set; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Цвет заголовка.
        /// </summary>
        public Brush TitleColor { get; private set; }

        /// <summary>
        /// Вершины.
        /// </summary>
        public IList<VisualVertex> Vertices { get; private set; }

        /// <summary>
        /// Связи.
        /// </summary>
        public IList<VisualConnection> Connections { get; private set; }

        /// <summary>
        /// Матрица.
        /// </summary>
        public int[,] Matrix { get; private set; }

        /// <summary>
        /// Описание колонок таблицы.
        /// </summary>
        public string[] ColumnsDescription { get; private set; }

        /// <summary>
        /// Описание строк таблицы.
        /// </summary>
        public string[] RowsDescription { get; private set; }
        #endregion

        #region constructor
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="navigationService">Сервис навигации.</param>
        /// <param name="accessControlService">Сервис контроля доступа.</param>
        /// <param name="userMatrix">Матрица пользователя.</param>
        /// <param name="graph">Граф.</param>
        public VerifyCreateMatrixTaskViewModel(
            INavigationService navigationService,
            IAccessControlService accessControlService,
            int[,] userMatrix,
            (IList<VisualVertex>,
            IList<VisualConnection>) graph)
        {
            _navigationService = navigationService;
            _accessControlService = accessControlService;
            Matrix = userMatrix;
            Vertices = graph.Item1;
            Connections = graph.Item2;

            var matrixType = ((CreateMatrixTask)_accessControlService
                .CurrentEducationMaterial
                .EducationMaterial).CreatableMatrixType;

            BaseMatrix correctMatrix;

            if (matrixType == CreatableMatrixType.AdjacencyMatrix)
            {
                correctMatrix = new AdjacencyMatrix(
                    Vertices.Select(v => v.Vertex).ToArray(),
                    Connections.Select(c => c.Connection).ToArray());
            }
            else
            {
                correctMatrix = new IncidenceMatrix(
                    Vertices.Select(v => v.Vertex).ToArray(),
                    Connections.Select(c => c.Connection).ToArray());
            }

            ColumnsDescription = correctMatrix.ColumnsDescription;
            RowsDescription = correctMatrix.RowsDescription;

            Verify(correctMatrix.Matrix, userMatrix);
            GoBack = new RelayCommand(GoBackCommand);
        }
        #endregion

        #region private methods
        /// <summary>
        /// Проверка.
        /// </summary>
        /// <param name="correctMatrix">Корректная матрица.</param>
        /// <param name="userMatrix">Матрица пользователя.</param>
        private void Verify(int[,] correctMatrix, int[,] userMatrix)
        {
            var flag = true;
            var rows = userMatrix.GetUpperBound(0) + 1;
            var colums = userMatrix.Length / rows;

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < colums; j++)
                {
                    if (correctMatrix[i, j] != userMatrix[i, j])
                    {
                        flag = false;
                        break;
                    }
                }
            }

            if (!_accessControlService.CheckEducationMaterialIsPassed(_accessControlService.CurrentEducationMaterial))
            {
                _accessControlService.AddAttempt(_accessControlService.CurrentEducationMaterial);
            }

            if (flag)
            {
                _accessControlService.OpenNext(_accessControlService.CurrentEducationMaterial);
                Title = "Верно";
                TitleColor = new SolidColorBrush(Colors.Green);
            }
            else
            {
                Title = "Неверно";
                TitleColor = new SolidColorBrush(Colors.Red);
            }
        }

        /// <summary>
        /// Переход в CreateMatrixTaskViewModel.
        /// </summary>
        /// <param name="parameter"></param>
        private void GoBackCommand(object parameter)
        {
            _navigationService.NavigateTo<CreateMatrixTaskViewModel>();
        }
        #endregion
    }
}
