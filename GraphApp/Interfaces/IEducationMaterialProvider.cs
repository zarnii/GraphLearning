using GraphApp.Model;
using System.Collections.Generic;

namespace GraphApp.Interfaces
{
    /// <summary>
    /// Поставщик обучающего материала.
    /// </summary>
    public interface IEducationMaterialProvider
    {
        /// <summary>
        /// Получение коллекции обучающего материала.
        /// </summary>
        /// <returns>Коллекция обучающего материала.</returns>
        public IList<EducationMaterial> GetMaterialCollection();
    }
}
