using GraphApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Interfaces
{
    /// <summary>
    /// Сервис проверки практического задания.
    /// </summary>
    public interface IVerifyPracticTaskService
    {
        /// <summary>
        /// Проверяемая коллекция вершин.
        /// </summary>
        public IList<VisualVertex> VerifiedVertices { get; set; }

        /// <summary>
        /// Проверяемая коллекция связей.
        /// </summary>
        public IList<VisualConnection> VerifiedConnections { get; set; }

        /// <summary>
        /// Проверяемое задание.
        /// </summary>
        public PracticTask VerifiedPracticTask { get; set; }

        /// <summary>
        /// Проверка практического задания.
        /// </summary>
        /// <returns>Проверенное практическое задание.</returns>
        VerifiedPracticTask VerifyPracticTask();
    }
}
