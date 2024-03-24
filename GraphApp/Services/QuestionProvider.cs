using GraphApp.Interfaces;
using GraphApp.Model;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace GraphApp.Services
{
    /// <summary>
    /// Поставщик вопросов.
    /// </summary>
    public class QuestionProvider : IQuestionProvider
    {
        /// <summary>
        /// Коллекция вопросов.
        /// </summary>
        public List<Question> QuestionsCollection { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="dataLoader">Сервис загрузки данных.</param>
        public QuestionProvider(IDataLoader dataLoader)
        {
            InitQuestions(dataLoader);
        }

        /// <summary>
        /// Инициализация вопросов.
        /// </summary>
        /// <param name="dataLoader">Сервис загрузки данных.</param>
        private void InitQuestions(IDataLoader dataLoader)
        {
            var pathToQuestions = ConfigurationManager.AppSettings["defaultPathToQuestions"];

            if (!Directory.Exists(pathToQuestions))
            {
                return;
            }

            var files = Directory.GetFiles(pathToQuestions);
            QuestionsCollection = new List<Question>(files.Length);

            foreach (var file in files)
            {
                QuestionsCollection.Add(dataLoader.Load<Question>(file));
            }
        }
    }
}
