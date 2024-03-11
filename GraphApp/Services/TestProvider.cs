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
        /// Список вопросов.
        /// </summary>
        private List<Question> _questionsCollection;

        /// <summary>
        /// Список тестов.
        /// </summary>
        private List<Test> _testCollection;

        /// <summary>
        /// Сервис загрузки данных.
        /// </summary>
        private IDataLoader _dataLoader;

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
            _dataLoader = dataLoader;

            InitQuestions();
            InitTests();
        }
        #endregion

        #region private methods
        /// <summary>
        /// Инициализация вопросов.
        /// </summary>
        private void InitQuestions()
        {
            var pathToQuestions = ConfigurationManager.AppSettings["defaultPathToQuestions"];

            if (!Directory.Exists(pathToQuestions))
            {
                return;
            }

            var files = Directory.GetFiles(pathToQuestions);
            _questionsCollection = new List<Question>();

            foreach (var file in files)
            {
                _questionsCollection.Add(_dataLoader.Load<Question>(file));
            }
        }

        private void InitTests()
        {
            _testCollection = new List<Test>();

            if (!Directory.Exists(ConfigurationManager.AppSettings["defaultPathToQuestions"]))
            {
                return;
            }

            var files = Directory.GetFiles(ConfigurationManager.AppSettings["defaultPathToTests"]);

            foreach (var file in files)
            {
                _testCollection.Add(_dataLoader.Load<Test>(file));
            }
        }
        #endregion

        #region public methods
        public Test RandomGenerate(int questionCount)
        {
            if (questionCount > _questionsCollection.Count || questionCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(questionCount),
                    $"Аргумент должен находиться в диапозоне (1, {_questionsCollection.Count})");
            }

            var questions = new Question[questionCount];
            var repeatingElements = new List<Question>();
            var random = new Random();

            var i = 0;
            while (i < questionCount)
            {
                var randItem = random.Randchoice<Question>(_questionsCollection);

                if (!repeatingElements.Contains(randItem))
                {
                    questions[i] = randItem;
                    i++;
                }
            }

            return new Test() { Title = "Сгенерированный вопрос", Questions = questions };

        }
        #endregion
    }
}
