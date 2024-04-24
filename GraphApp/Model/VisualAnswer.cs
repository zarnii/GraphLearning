using System;
using System.Windows.Media;

namespace GraphApp.Model
{
    /// <summary>
    /// Визуальный ответ.
    /// </summary>
    public class VisualAnswer : Answer
    {
        /// <summary>
        /// Прозраность цвета по умолчанию.
        /// </summary>
        private const double _defaultOpasity = 0.3;

        /// <summary>
        /// Цвет.
        /// </summary>
        public Brush Color { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="answer">Ответ.</param>
        /// <param name="color">Цвет.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public VisualAnswer(Answer answer, Brush color)
        {
            if (answer == null)
            {
                throw new ArgumentNullException(nameof(answer), "Пустой экземпляр ответа.");
            }

            if (color == null)
            {
                throw new ArgumentNullException(nameof(color), "Пустой цвет.");
            }

            Text = answer.Text;
            Flag = answer.Flag;
            Color = color;
            Color.Opacity = _defaultOpasity;
        }
    }
}
