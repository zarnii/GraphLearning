using GraphApp.Interfaces;
using GraphApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Services
{
    /// <summary>
    /// Генератор теста.
    /// </summary>
    public class TestGenerator : ITestGenerator
    {
        /// <summary>
        /// Коллекция вопросов.
        /// </summary>
        private List<Question> _questionsCollection;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="questionProvider">Поставщик вопросов.</param>
        /// <exception cref="ArgumentNullException">Пустая коллекция вопросов.</exception>
        public TestGenerator(IQuestionProvider questionProvider) 
        {
            if (questionProvider.QuestionsCollection == null)
            {
                throw new ArgumentNullException(nameof(questionProvider.QuestionsCollection), "Пустая коллекция вопросов.");
            }

            _questionsCollection = questionProvider.QuestionsCollection;
        }

        /// <summary>
        /// Генерация теста.
        /// </summary>
        /// <param name="questionCount">Количество вопросов.</param>
        /// <returns>Тест.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Количество вопросов вышло за пределы.</exception>
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
                    repeatingElements.Add(randItem);
                    questions[i] = randItem;
                    i++;
                }
            }

            return new Test() { Title = "Сгенерированный вопрос", Questions = questions };

        }
    }
}
