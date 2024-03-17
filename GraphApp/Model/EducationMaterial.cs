using System;

namespace GraphApp.Model
{
    /// <summary>
    /// Обучающий материал.
    /// </summary>
    public abstract class EducationMaterial: IComparable<EducationMaterial>
    {
        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Порядковый номер.
        /// </summary>
        public int IndexNumber { get; set; }

        /// <summary>
        /// Сравнение двух объектов.
        /// </summary>
        /// <param name="other">Сравниваемый объект.</param>
        /// <returns>1, если текущий объект больше other. -1, если текущий объект меньше other.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public int CompareTo(EducationMaterial? other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("Пустой объект для сравнения.");
            }

            if (IndexNumber > other.IndexNumber)
            {
                return 1;
            }
            else if (IndexNumber < other.IndexNumber)
            {
                return -1;
            }

            return 0;
        }
    }
}
