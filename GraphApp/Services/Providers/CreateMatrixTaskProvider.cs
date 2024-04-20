using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.Model.Serializing;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace GraphApp.Services.Providers
{
    public class CreateMatrixTaskProvider : IEducationMaterialProvider
    {
        private IDataLoader _dataLoader;

        private IMapper _mapper;

        public CreateMatrixTaskProvider(IDataLoader dataLoader, IMapper mapper)
        {
            _dataLoader = dataLoader;
            _mapper = mapper;
        }

        public IList<EducationMaterial> GetMaterialCollection()
        {
            var pathToTasks = ConfigurationManager.AppSettings["defaultPathToCreateMatrixTask"];

            if (!Directory.Exists(pathToTasks))
            {
                return null;
            }

            var files = Directory.GetFiles(pathToTasks, "*json");
            var taskCollection = new List<EducationMaterial>();

            foreach (var file in files)
            {
                var serTask = _dataLoader.Load<SerializableCreateMatrixTask>(file);
                taskCollection.Add(_mapper.Map<CreateMatrixTask>(serTask, null));
            }

            return taskCollection;
        }
    }
}
