using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Interfaces
{
    /// <summary>
    /// Обобщенная фабрика VM.
    /// </summary>
    public interface IFactoryViewModel
    {
        /// <summary>
        /// Создание VM.
        /// </summary>
        /// <param name="param">Параметры.</param>
        /// <returns>ViewModel.</returns>
        ViewModel.ViewModel Create(object[] param);
    }
}
