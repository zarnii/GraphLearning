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
        /// Проверка практического задания.
        /// </summary>
        /// <param name="practicTask">Практическое задание.</param>
        /// <param name="vertices">Коллекция вершин.</param>
        /// <param name="connections">Коллекция связей.</param>
        /// <returns>Проверенное практическое задание.</returns>
        VerifiedPracticTask VerifyPracticTask(PracticTask practicTask,
            IList<VisualVertex> vertices,
            IList<VisualConnection> connections);
    }
}
