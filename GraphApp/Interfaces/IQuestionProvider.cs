using GraphApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Interfaces
{
    /// <summary>
    /// Поставщик вопросов.
    /// </summary>
    public interface IQuestionProvider
    {
        /// <summary>
        /// Коллекция вопросов.
        /// </summary>
        List<Question> QuestionsCollection { get; }
    }
}
