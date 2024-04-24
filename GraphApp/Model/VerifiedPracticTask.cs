using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Model
{
    /// <summary>
    /// Проверенное практическое задание.
    /// </summary>
    public class VerifiedPracticTask
    {
        /// <summary>
        /// Флаг, показывающий выполнено ли условие по количеству вершин.
        /// </summary>
        public bool VertexCountIsDone { get; set; }

        /// <summary>
        /// Флаг, показывающий выполнено ли условие по позиции вершин.
        /// </summary>
        public bool VertexPositionIsDone { get; set; }

        /// <summary>
        /// Флаг, показывающий выполнено ли условие по размеру вершин.
        /// </summary>
        public bool VertexSizeIsDone { get; set; }

        /// <summary>
        /// Флаг, показывающий выполнено ли условие по названию вершин.
        /// </summary>
        public bool VertexNameIsDone { get; set; }

        /// <summary>
        /// Флаг, показывающий выполнено ли условие по связям.
        /// </summary>
        public bool ConnectionIsDone { get; set; }

        /// <summary>
        /// Флаг, показывающий выполнено ли условие по количество связей.
        /// </summary>
        public bool ConnectionCountIsDone { get; set; }

        /// <summary>
        /// Флаг, показывающий выполнено ли условие по весу связей.
        /// </summary>
        public bool ConnectionWeightIsDone { get; set; }

        /// <summary>
        /// Флаг, показывающий выполнено ли условие по типу связей.
        /// </summary>
        public bool ConnectionTypeIsDone { get; set; }
    }
}
