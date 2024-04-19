﻿using DocumentFormat.OpenXml.InkML;
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

        private IFactoryViewModel _factoryVerifyCreateMatrixTask;

        /// <summary>
        /// Матрица смежности.
        /// </summary>
        private AdjacencyMatrix _adjacencyMatrix;

        /// <summary>
        /// Матрица, строимая пользователем.
        /// </summary>
        private int[,] _matrix;
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
        /// Описание колонок таблицы.
        /// </summary>
        public string[] ColumnsDescription 
        {
            get
            {
                return _adjacencyMatrix.ColumnsDescription;
            }
        }

        /// <summary>
        /// Описание строк таблицы.
        /// </summary>
        public string[] RowsDescription 
        {
            get
            {
                return _adjacencyMatrix.RowsDescription;
            }
        }

        /// <summary>
        /// Матрица, строимая пользователем.
        /// </summary>
        public int[,] Matrix
        {
            get
            {
                return _matrix;
            }
            private set
            {
                _matrix = value;
                OnPropertyChanged(nameof(Matrix));
            }
        }

        /// <summary>
        /// Выбранная ячейка.
        /// </summary>
        public RowColumnIndex SelectedCell { get; set; }

        /// <summary>
        /// Цвет заголовка.
        /// </summary>
        public Brush TitleColor { get; private set; }
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
            Vertices = ((CreateMatrixTask)_accessControlService.CurrentEducationMaterial.EducationMaterial).Vertices;
            Connections = ((CreateMatrixTask)_accessControlService.CurrentEducationMaterial.EducationMaterial).Connections;

            GoBack = new RelayCommand(GoBackCommand);
            VerifyTask = new RelayCommand(VerifyTaskCommand);
            SelectCell = new RelayCommand(SelectCellCommand);

            _adjacencyMatrix = new AdjacencyMatrix(
                Vertices.Select(v => v.Vertex).ToArray(),
                Connections.Select(c => c.Connection).ToArray()
            );
            var rowCount = _adjacencyMatrix.Matrix.GetUpperBound(0) + 1;
            var columnCount = _adjacencyMatrix.Matrix.Length / rowCount;
            Matrix = new int[rowCount, rowCount];
            _factoryVerifyCreateMatrixTask = factoryVerifyCreateMatrixTask;
        }
        #endregion

        #region private methods
        /// <summary>
        /// Проверка задания.
        /// </summary>
        /// <param name="parameter"></param>
        private void VerifyTaskCommand(object parameter)
        {
            _navigationService.NavigateTo(_factoryVerifyCreateMatrixTask, 
                new object[4] 
                { 
                    _adjacencyMatrix, 
                    Matrix,
                    Vertices,
                    Connections
                }
            );
        }

        /// <summary>
        /// Выбор ячейки.
        /// </summary>
        /// <param name="parameter"></param>
        private void SelectCellCommand(object parameter)
        {
            var matrix = (int[,])Matrix.Clone();

            if (matrix[SelectedCell.Row, SelectedCell.Column] == 0)
            {
                matrix[SelectedCell.Row, SelectedCell.Column] = 1;
            }
            else
            {
                matrix[SelectedCell.Row, SelectedCell.Column] = 0;
            }
            Matrix = matrix;
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
