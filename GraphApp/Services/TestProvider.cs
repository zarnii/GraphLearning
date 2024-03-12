using GraphApp.Interfaces;
using GraphApp.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace GraphApp.Services
{
    /// <summary>
    /// Сервис вопросов.
    /// </summary>
    public class TestProvider : ITestProvider
    {
        #region fields
        /// <summary>
        /// Текущий вопрос.
        /// </summary>
        private Test _currentTest;

        /// <summary>
        /// Список тестов.
        /// </summary>
        private List<Test> _testCollection;
        #endregion

        #region properties
        /// <summary>
        /// Коллекция тестов.
        /// </summary>
        public List<Test> TestCollection
        {
            get
            {
                return _testCollection;
            }
        }

        /// <summary>
        /// Текущий тест.
        /// </summary>
        public Test CurrentTest
        {
            get
            {
                return _currentTest;
            }
            set
            {
                _currentTest = value;
            }
        }
        #endregion

        #region constructor
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="dataLoader"></param>
        public TestProvider(IDataLoader dataLoader)
        {
            InitTests(dataLoader);
        }
        #endregion

        #region private methods
        /// <summary>
        /// Инициализация тестов.
        /// </summary>
        private void InitTests(IDataLoader dataLoader)
        {
            _testCollection = new List<Test>();

            if (!Directory.Exists(ConfigurationManager.AppSettings["defaultPathToQuestions"]))
            {
                return;
            }

            var files = Directory.GetFiles(ConfigurationManager.AppSettings["defaultPathToTests"]);

            foreach (var file in files)
            {
                _testCollection.Add(dataLoader.Load<Test>(file));
            }
        }
        #endregion

        #region public methods

        #endregion
    }
}
