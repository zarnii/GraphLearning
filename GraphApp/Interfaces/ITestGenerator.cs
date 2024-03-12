using GraphApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Interfaces
{
    /// <summary>
    /// Генератор тестов.
    /// </summary>
    public interface ITestGenerator
    {
        /// <summary>
        /// Генерация теста.
        /// </summary>
        /// <param name="questionCount">Количество вопросов.</param>
        /// <returns>Тест.</returns>
        Test RandomGenerate(int questionCount);
    }
}
