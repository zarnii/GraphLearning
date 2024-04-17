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
        /// <param name="vertices">Коллекция вершин.</param>
        /// <param name="connections">Коллекция связей.</param>
        /// <param name="practicTask">Проверяемое задание.</param>
        /// <returns>Проверенное задание.</returns>
        VerifiedPracticTask VerifyPracticTask(
            IList<VisualVertex> vertices, 
            IList<VisualConnection> connections, 
            PracticTask practicTask);
    }
}
