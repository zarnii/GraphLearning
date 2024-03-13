using GraphApp.Model;

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

        /// <summary>
        /// Открыть слудующий материал.
        /// </summary>
        /// <param name="material">Узел обучающего материала.</param>
        void OpenNext(EducationMaterialNode material);
    }
}