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
        /// Набранное количество очков.
        /// </summary>
        public int Points { get; }

        /// <summary>
        /// Проверяемый тест.
        /// </summary>
        public Test VerifableTest { get; set; }

        /// <summary>
        /// Выбранные ответы на вопрос.
        /// </summary>
        public Dictionary<Question, Answer> SelectedAnswerByQuestion { get; set; }

        /// <summary>
        /// Проверенные вопросы по ответам.
        /// </summary>
        Dictionary<Question, List<VisualAnswer>> QuestionVerifiedAnswerMap { get; }

        /// <summary>
        /// Провера ответов.
        /// </summary>
        void CheckAnswer();
    }
}