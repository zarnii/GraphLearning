using System;
using System.Collections.Generic;
using GraphApp.Model;

namespace GraphApp.Interfaces
{
    /// <summary>
    /// Поставщик практических заданий.
    /// </summary>
    public interface IPracticProvider
    {
        /// <summary>
        /// Коллекция практических заданий.
        /// </summary>
        public List<PracticTask> PracticCollection { get; }

        /// <summary>
        /// Текущее практическое задание.
        /// </summary>
        public PracticTask CurrentPractic { get; }

    }
}
