using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.Model.Serializing;

namespace GraphApp.Services
{
    /// <summary>
    /// Поставщик практических заданий.
    /// </summary>
    public class PracticProvider : IPracticProvider
    {
        /// <summary>
        /// Сервис загрузки данных.
        /// </summary>
        private IDataLoader _dataLoader;

        /// <summary>
        /// Маппер.
        /// </summary>
        private IMapper _mapper;

        /// <summary>
        /// Коллекция практических заданий.
        /// </summary>
        public List<PracticTask> PracticCollection { get; private set; }

        /// <summary>
        /// Текущее прктическое задание.
        /// </summary>
        public PracticTask CurrentPractic { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="dataLoader">Сервис загрузки данных.</param>
        /// <param name="mapper">Маппер.</param>
        public PracticProvider(IDataLoader dataLoader, IMapper mapper)
        {
            _dataLoader = dataLoader;
            _mapper = mapper;
            InitPractic();
        }

        /// <summary>
        /// Инициализация практических заданий.
        /// </summary>
        private void InitPractic()
        {
            var pathToPratcits = ConfigurationManager.AppSettings["defaultPathToPracticTask"];

            if (!Directory.Exists(pathToPratcits))
            {
                return;
            }

            var files = Directory.GetFiles(pathToPratcits);
            PracticCollection = new List<PracticTask>();

            foreach (var file in files)
            {
                var serPracticTask = _dataLoader.Load<SerializablePracticTask>(file);
                PracticCollection.Add(_mapper.Map<PracticTask>(serPracticTask, null));
            }
        }
    }
}
