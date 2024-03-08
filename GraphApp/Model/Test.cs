using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphApp.Model;

namespace GraphApp.Model
{
    /// <summary>
    /// Практическое задание в виде теста.
    /// </summary>
    public class Test
    {
        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Вопросы.
        /// </summary>
        public Question[] Questions { get; set; }
    }
}
