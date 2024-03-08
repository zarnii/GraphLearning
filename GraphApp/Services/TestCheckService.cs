using GraphApp.Interfaces;
using GraphApp.Model;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace GraphApp.Services
{
    /// <summary>
    /// Сервис проверки ответов.
    /// </summary>
    public class TestCheckService : ITestCheckService
    {
        #region fields
        /// <summary>
        /// Цвет правильного ответа.
        /// </summary>
        private const string _correctColorHex = "#145210";

        /// <summary>
        /// Цвет неправильного ответа.
        /// </summary>
        private const string _incorrectColorHex = "#891b21";

        /// <summary>
        /// Цвет ответа по умолчанию.
        /// </summary>
        private const string _defaultColorHex = "#FF252526";

        /// <summary>
        /// Проверяемый тест.
        /// </summary>
        private Test _verifableTest;

        /// <summary>
        /// Выбранные ответы по вопросам.
        /// </summary>
        private Dictionary<Question, Answer> _selectedAnswerByQuestion;
        #endregion

        #region properties
        /// <summary>
        /// Набранное количество очков.
        /// </summary>
        public int Points { get; private set; }

        /// <summary>
        /// Проверяемый тест.
        /// </summary>
        public Test VerifableTest 
        { 
            get
            {
                return _verifableTest;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Пустой проверяемый тест");
                }

                _verifableTest = value;
            } 
        }

        /// <summary>
        /// Выбранные ответы по вопросам.
        /// </summary>
        public Dictionary<Question, Answer> SelectedAnswerByQuestion 
        {
            get
            {
                return _selectedAnswerByQuestion;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Пустой словарь.");
                }

                _selectedAnswerByQuestion = value;
            }
        }

        /// <summary>
        /// Проверенные ответы.
        /// </summary>
        public Dictionary<Question, List<VisualAnswer>> QuestionVerifiedAnswerMap { get; private set; }
        #endregion

        #region constructor
        /// <summary>
        /// Конструктор.
        /// </summary>
        public TestCheckService()
        {
            QuestionVerifiedAnswerMap = new Dictionary<Question, List<VisualAnswer>>();
        }
        #endregion

        #region public methods
        /// <summary>
        /// Провера ответов.
        /// </summary>
        public void CheckAnswer()
        {
            QuestionVerifiedAnswerMap.Clear();

            /*
                Так как VerifableTest содержит те же ptr на инстансы Question и Answer,
                что и SelectedAnswerByQuestion, то мы можем их стравнивать.
            */


            // Бежим по VerifableTest.Question для того, чтоб сохранить порядок вопросов.
            foreach (var question in VerifableTest.Questions)
            {
                var answerList = new List<VisualAnswer>();

                foreach (var answer in question.Answers)
                {
                    Answer? selectedAnswer;
                    SelectedAnswerByQuestion.TryGetValue(question, out selectedAnswer);

                    var color = SelectColorForAnswer(answer, selectedAnswer == answer);

                    answerList.Add(new VisualAnswer(answer, color));
                }

                QuestionVerifiedAnswerMap[question] = answerList;
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// Выбор цвета для отображения верности ответа.
        /// </summary>
        /// <param name="answer">Ответ.</param>
        /// <param name="isSelected">Флаг, показывающий является ли ответ выбранный.</param>
        /// <returns>Цвет.</returns>
        private Brush SelectColorForAnswer(Answer answer, bool isSelected)
        {
            // Если ответ не выбран пользователем, то его верность не раскрываем.
            if (!isSelected)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(_defaultColorHex));
            }

            if (answer.Flag)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(_correctColorHex));
            }

            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(_incorrectColorHex));
        }
        #endregion
    }
}
