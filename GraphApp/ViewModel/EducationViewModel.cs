using GraphApp.Command;
using GraphApp.Interfaces;
using GraphApp.Model;
using InputBox;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace GraphApp.ViewModel
{
    /// <summary>
    /// Модель представления окна с уроками.
    /// </summary>
    public class EducationViewModel : ViewModel
    {
        #region fields
        /// <summary>
        /// Сервис навигации.
        /// </summary>
        private INavigationService _navigationService;

        /// <summary>
        /// Сервис вопросов.
        /// </summary>
        private ITestProvider _testProvider;

        /// <summary>
        /// Генератор тестов.
        /// </summary>
        private ITestGenerator _testGenerator;

        /// <summary>
        /// Сервис контроля доступа.
        /// </summary>
        private IAccessControlService _accessControlService;

        /// <summary>
        /// Сервис user control.
        /// </summary>
        private ITheoryService _userControlService;

        /// <summary>
        /// Команда открытия окна.
        /// </summary>
        private ICommand _openWindow;

        /// <summary>
        /// Команда открытия вопросов.
        /// </summary>
        private ICommand _openQuestion;

        /// <summary>
        /// Команда открытия теории.
        /// </summary>
        private ICommand _openTheory;

        /// <summary>
        /// Команда генерации теста.
        /// </summary>
        private ICommand _generateTest;
        #endregion

        #region properties
        /// <summary>
        /// Команда открытия окна.
        /// </summary>
        public ICommand OpenWindow
        {
            get
            {
                return _openWindow;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                _openWindow = value;
            }
        }

        /// <summary>
        /// Команда перехода назад.
        /// </summary>
        public ICommand OpenMaterial
        {
            get
            {
                return _openQuestion;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Пустая команда открытия вопроса.");
                }

                _openQuestion = value;
            }
        }

        /// <summary>
        /// Команда открытия теории.
        /// </summary>
        public ICommand OpenTheory
        {
            get
            {
                return _openTheory;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Пустая команда открытия теории.");
                }

                _openTheory = value;
            }
        }

        /// <summary>
        /// Команда генерации теста.
        /// </summary>
        public ICommand GenerateTest
        {
            get
            {
                return _generateTest;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Пустая команда генерации теста.");
                }

                _generateTest = value;
            }
        }

        /// <summary>
        /// Коллекция тестов.
        /// </summary>
        public EducationMaterialNode[] EducationMaterials
        {
            get
            {
                return _accessControlService.EducationMaterialsCollection;
            }
        }

        /// <summary>
        /// Коллекция теории.
        /// </summary>
        public List<Theory> Theories
        {
            get
            {
                return _userControlService.TheoryControls;
            }
        }
        #endregion

        #region constructor
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="navigationService">Сервис навигации.</param>
        public EducationViewModel(INavigationService navigationService,
            ITestProvider questionService,
            ITheoryService userControlService,
            ITestGenerator testGenerator,
            IAccessControlService accessControlService)
        {
            _navigationService = navigationService;
            _testProvider = questionService;
            _testGenerator = testGenerator;
            _accessControlService = accessControlService;
            _userControlService = userControlService;

            OpenWindow = new RelayCommand(OpenWindowCommand);
            OpenMaterial = new RelayCommand(OpenMaterialCommand, CheckCanExecute);
            OpenTheory = new RelayCommand(OpenTheoryCommand);
            GenerateTest = new RelayCommand(GenerateTestCommand);
        }
        #endregion

        #region private methods
        /// <summary>
        /// Открытие окна.
        /// </summary>
        /// <param name="parameter">Тип окна.</param>
        private void OpenWindowCommand(object parameter)
        {
            _navigationService.NavigateTo((Type)parameter);
        }

        /// <summary>
        /// Открытие окна с вопросом.
        /// </summary>
        /// <param name="parameter">Открываемый вопрос.</param>
        private void OpenMaterialCommand(object parameter)
        {
            var material = (EducationMaterialNode)parameter;
            var type = material.EducationMaterial.GetType();

            if (type == typeof(Test))
            {
                _accessControlService.CurrentEducationMaterial = material;
                _navigationService.NavigateTo<TestViewModel>();
            }
            else if (type == typeof(PracticTask))
            {
                _accessControlService.CurrentEducationMaterial = material;
                _navigationService.NavigateTo<PracticViewModel>();
            }
        }

        /// <summary>
        /// Открытия теории.
        /// </summary>
        /// <param name="parameter">Открываемый раздел теории.</param>
        private void OpenTheoryCommand(object parameter)
        {
            _userControlService.CurrentTheory = (Theory)parameter;
            _navigationService.NavigateTo<TheoryViewModel>();
        }

        /// <summary>
        /// Генерация теста.
        /// </summary>
        /// <param name="parameter"></param>
        private void GenerateTestCommand(object parameter)
        {
            var inputBox = new InputBoxWindow("Введите количество вопросов", "Введите количество вопросов");

            if (!inputBox.ShowDialog())
            {
                return;
            }

            try
            {
                _testProvider.CurrentTest = _testGenerator.RandomGenerate(inputBox.TextBoxResult);
                _navigationService.NavigateTo<TestViewModel>();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // Временно.
                MessageBox.Show(
                    ex.Message,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private bool CheckCanExecute(object parameter)
        {
            return ((EducationMaterialNode)parameter).EducationMaterial != null;
        }
        #endregion

        #region public methods
        #endregion
    }
}
