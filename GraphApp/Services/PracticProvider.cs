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
    public class PracticProvider : IPracticProvider
    {
        private IDataLoader _dataLoader;

        private IMapper _mapper;

        public List<PracticTask> PracticCollection { get; private set; }

        public PracticTask CurrentPractic { get; private set; }

        public PracticProvider(IDataLoader dataLoader, IMapper mapper)
        {
            _dataLoader = dataLoader;
            _mapper = mapper;
            InitPractic();
        }

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
