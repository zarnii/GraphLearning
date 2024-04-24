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
    public class VerifyTestService : IVerifyTestService
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
        #endregion

        #region properties
        #endregion

        #region constructor
        #endregion

        #region public methods
        public (int, Dictionary<Question, List<VisualAnswer>>) VerifyTest(Dictionary<Question, Answer> selectedAnswerByQuestion, Test verifableTest)
        {
            var point = CountPoints(selectedAnswerByQuestion);
            var questionVerifiedAnswerMap = new Dictionary<Question, List<VisualAnswer>>();

            /*
                Так как VerifableTest содержит те же ptr на инстансы Question и Answer,
                что и SelectedAnswerByQuestion, то мы можем их стравнивать.
            */

            // Бежим по VerifableTest.Question для того, чтоб сохранить порядок вопросов.
            foreach (var question in verifableTest.Questions)
            {
                var answerList = new List<VisualAnswer>();

                foreach (var answer in question.Answers)
                {
                    Answer? selectedAnswer;
                    selectedAnswerByQuestion.TryGetValue(question, out selectedAnswer);

                    var color = SelectColorForAnswer(answer, selectedAnswer == answer);

                    answerList.Add(new VisualAnswer(answer, color));
                }

                questionVerifiedAnswerMap[question] = answerList;
            }

            return (point, questionVerifiedAnswerMap);
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

        private int CountPoints(Dictionary<Question, Answer> selectedAnswerByQuestion)
        {
            var point = 0;

            foreach (var answer in selectedAnswerByQuestion.Values)
            {
                if (answer.Flag)
                {
                    point++;
                }
            }

            return point;
        }
        #endregion
    }
}
