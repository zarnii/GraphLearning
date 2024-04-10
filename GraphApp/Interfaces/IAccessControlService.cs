using GraphApp.Model;
using System.Collections.Generic;

namespace GraphApp.Interfaces
{
    /// <summary>
    /// Сервис контроля доступа к обучающему материалу.
    /// </summary>
    public interface IAccessControlService
    {
        /// <summary>
        /// Текущий обучающий материал.
        /// </summary>
        EducationMaterialNode CurrentEducationMaterial { get; set; }

        /// <summary>
        /// Коллекция материала.
        /// </summary>
        EducationMaterialNode[] EducationMaterialsCollection { get; }

        Dictionary<EducationMaterialNode, EducationMaterialInfo> EducationMaterialMap { get;}

        /// <summary>
        /// Открыть слудующий материал.
        /// </summary>
        /// <param name="material">Узел обучающего материала.</param>
        void OpenNext(EducationMaterialNode material);

        /// <summary>
        /// Проверка, пройден ли обучающий материал.
        /// </summary>
        /// <param name="material">Обучающий материал.</param>
        /// <returns>True, если пройден.</returns>
        bool CheckEducationMaterialIsPassed(EducationMaterialNode material);

        /// <summary>
        /// Увелечение попытки прохождения на 1.
        /// </summary>
        /// <param name="material"></param>
        void AddAttempt(EducationMaterialNode material);
    }
}