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
        /// Время выполнения задания.
        /// </summary>
        public TimeSpan? LeadTime { get; set; }

        /// <summary>
        /// Сравнение двух объектов.
        /// </summary>
        /// <param name="other">Сравниваемый объект.</param>
        /// <returns>1, если текущий объект больше other. -1, если текущий объект меньше other.</returns>
        public int CompareTo(EducationMaterial? other)
        {
            return IndexNumber.CompareTo(other?.IndexNumber);
        }
    }
}
