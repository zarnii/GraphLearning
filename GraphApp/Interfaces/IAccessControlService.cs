using GraphApp.Model;

namespace GraphApp.Interfaces
{
    public interface IAccessControlService
    {
        EducationMaterialNode[] EducationMaterialsCollection { get; }

        void OpenNext(EducationMaterialNode material);
    }
}