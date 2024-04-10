using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Model
{
    /// <summary>
    /// Дополнительная информация о обучающем материале.
    /// </summary>
    public class EducationMaterialInfo
    {
        /// <summary>
        /// Последовательный номер.
        /// </summary>
        public int IndexNumber { get; set; }

        /// <summary>
        /// Флаг, показывающий отк
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// Номер попыткы с которой было пройдено задание. 
        /// </summary>
        public int AttemptsNumber { get; set; }
    }
}
