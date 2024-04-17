using GraphApp.Model;
using System.Collections.Generic;

namespace GraphApp.Interfaces
{
    /// <summary>
    /// Сервис проверки ответов на тест.
    /// </summary>
    public interface IVerifyTestService
    {
        /// <summary>
        /// Проверка ответов.
        /// </summary>
        /// <param name="selectedAnswerByQuestion">Выбранные ответы по вопросам.</param>
        /// <param name="verifableTest">Проверяемый тест.</param>
        /// <returns>Количество правильных ответов, провереные ответы по вопросам.</returns>
        (int, Dictionary<Question, List<VisualAnswer>>) VerifyTest(
            Dictionary<Question, Answer> selectedAnswerByQuestion, 
            Test verifableTest);
    }
}