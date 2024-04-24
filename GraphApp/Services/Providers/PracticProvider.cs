using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.Model.Serializing;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace GraphApp.Services.Providers
{
    /// <summary>
    /// Поставщик практических заданий
    /// </summary>
    public class PracticProvider : IEducationMaterialProvider
    {
        private IDataLoader _dataLoader;

        private IMapper _mapper;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="dataLoader">Сервис загрузки данных.</param>
        /// <param name="mapper">Маппер.</param>
        public PracticProvider(IDataLoader dataLoader, IMapper mapper)
        {
            _dataLoader = dataLoader;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение коллекции обучающего материала.
        /// </summary>
        /// <returns>Коллекция обучающего материала.</returns>
        public IList<EducationMaterial> GetMaterialCollection()
        {
            var pathToPratcits = ConfigurationManager.AppSettings["defaultPathToPracticTask"];

            if (!Directory.Exists(pathToPratcits))
            {
                return null;
            }

            var files = Directory.GetFiles(pathToPratcits, "*json");
            var practicCollection = new List<EducationMaterial>(files.Length);

            foreach (var file in files)
            {
                var serPracticTask =  _dataLoader.Load<SerializablePracticTask>(file);
                practicCollection.Add(_mapper.Map<PracticTask>(serPracticTask, null));
            }

            return practicCollection;
        }
    }
}
