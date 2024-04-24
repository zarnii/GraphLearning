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
    public class Test: EducationMaterial
    {
        /// <summary>
        /// Вопросы.
        /// </summary>
        public Question[] Questions { get; set; }
    }
}
