using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.Model.Exception;
using GraphApp.Model.Serializing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows;

namespace GraphApp.Services
{
    /// <summary>
    /// Сервис контроля доступа.
    /// </summary>
    public class AccessControlService : IAccessControlService
    {
        /// <summary>
        /// Карта сопоставления флага, показывающий открыт ли материал, и материал. 
        /// </summary>
        private Dictionary<EducationMaterialNode, bool> _educationMaterialMap;

        /// <summary>
        /// Обработчик данных.
        /// </summary>
        private IDataHandlerService _dataHandler;

        /// <summary>
        /// Маппер.
        /// </summary>
        private IMapper _mapper;

        /// <summary>
        /// Текущий обучающий материал.
        /// </summary>
        public EducationMaterialNode CurrentEducationMaterial { get; set; }

        /// <summary>
        /// Коллекция материла.
        /// </summary>
        public EducationMaterialNode[] EducationMaterialsCollection { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="testProvider">Поставщик тестов.</param>
        /// <param name="practicProvider">Поставщик практик.</param>
        public AccessControlService(
            ITestProvider testProvider, 
            IPracticProvider practicProvider,
            IDataHandlerService dataHandler, 
            IMapper mapper)
        {
            _dataHandler = dataHandler;
            _mapper = mapper;

            EducationMaterialsCollection = new EducationMaterialNode[
                testProvider.TestCollection.Count + practicProvider.PracticCollection.Count];
            _educationMaterialMap = new Dictionary<EducationMaterialNode, bool>(
                testProvider.TestCollection.Count + practicProvider.PracticCollection.Count);

            InitNodesCollection(testProvider, practicProvider);
            LoadMap(dataHandler, mapper);
        }

        /// <summary>
        /// Открытие следующего материала.
        /// </summary>
        /// <param name="material">Текущий обучающий материал.</param>
        /// <exception cref="ArgumentException"></exception>
        public void OpenNext(EducationMaterialNode material)
        {
            if (!_educationMaterialMap.ContainsKey(material))
            {
                return;
            }

            if (!_educationMaterialMap[material])
            {
                throw new ArgumentException("Невозможно открыть следующий элемент, так как текущий элемент закрыт.");
            }

            var materialIndex = EducationMaterialsCollection.FindIndex<EducationMaterialNode>(material);

            if (materialIndex + 1 != EducationMaterialsCollection.Length)
            {
                _educationMaterialMap[EducationMaterialsCollection[materialIndex + 1]] = true;
                SaveMap();
            }
        }

        /// <summary>
        /// Проверка можно ли получить обучающий материал.
        /// </summary>
        /// <param name="material">Узел обучающего материала.</param>
        /// <returns>True, если можно.</returns>
        private bool CheckCanGetMaterial(EducationMaterialNode material)
        {
            if (_educationMaterialMap[material])
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Инициализация полей.
        /// </summary>
        private void InitNodesCollection(ITestProvider testProvider, IPracticProvider practicProvider)
        {
            var iter = 0;

            for (var i = 0; i < testProvider.TestCollection.Count; i++)
            {
                var educationMaterialNode = new EducationMaterialNode(
                    testProvider.TestCollection[i],
                    CheckCanGetMaterial
                );

                EducationMaterialsCollection[iter] = educationMaterialNode;

                iter++;
            }

            for (var i = 0; i < practicProvider.PracticCollection.Count; i++)
            {
                var educationMaterialNode = new EducationMaterialNode(
                    practicProvider.PracticCollection[i],
                    CheckCanGetMaterial
                );

                EducationMaterialsCollection[iter] = educationMaterialNode;

                iter++;
            }

            Array.Sort<EducationMaterialNode>(EducationMaterialsCollection);
        }

        /// <summary>
        /// Загрузка прогресса пользователя.
        /// </summary>
        /// <param name="dataLoader">Сервис загрузки.</param>
        /// <param name="mapper">Маппер.</param>
        private void LoadMap(IDataLoader dataLoader, IMapper mapper)
        {
            var pathToFile = ConfigurationManager.AppSettings["defaultPathToEducationMaterialMap"];
            var fileName = ConfigurationManager.AppSettings["defaultEducationMaterialMapFileName"];

            if (!Directory.Exists(pathToFile))
            {
                Directory.CreateDirectory(pathToFile);
            }

            if (!File.Exists($"{pathToFile}/{fileName}")) 
            {
                var fileStream = File.Create($"{pathToFile}/{fileName}");
                fileStream.Close();
                OnErrorLoadMap();

                return;
            }

            try
            {
                var data = dataLoader.Load<SerializableEducationMaterialNode[]>($"{pathToFile}/{fileName}");

                foreach (var node in data)
                {
                    var pair = mapper.Map<KeyValuePairClass<EducationMaterialNode, bool>>(node, EducationMaterialsCollection);
                    _educationMaterialMap.Add(pair.Pair.Key, pair.Pair.Value);
                }
            }
            catch(ArgumentNullException ex)
            {
                OnErrorLoadMap();

                MessageBox.Show(
                    "При загрузке прогресса не был найден обучающий модуль. " +
                    "Возможно, файл был удален. Прогресс был сброшен.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
            catch(LoadDataException)
            {
                OnErrorLoadMap();
            }
        }

        /// <summary>
        /// Сохранение прогресса.
        /// </summary>
        private void SaveMap()
        {
            var pathToFile = ConfigurationManager.AppSettings["defaultPathToEducationMaterialMap"];
            var fileName = ConfigurationManager.AppSettings["defaultEducationMaterialMapFileName"];

            if (!Directory.Exists(pathToFile))
            {
                Directory.CreateDirectory(pathToFile);
            }

            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }

            var serializableEducationMaterialNodes = new List<SerializableEducationMaterialNode>(_educationMaterialMap.Count);

            foreach (var node in EducationMaterialsCollection)
            {
                serializableEducationMaterialNodes.Add(
                    _mapper.Map<SerializableEducationMaterialNode>(node, null)
                );
            }

            _dataHandler.Save<List<SerializableEducationMaterialNode>>(
                serializableEducationMaterialNodes, $"{pathToFile}/{fileName}");
        }

        /// <summary>
        /// При ошибке загрузки прогресса пользователя.
        /// </summary>
        private void OnErrorLoadMap()
        {
            foreach (var node in EducationMaterialsCollection)
            {
                _educationMaterialMap[node] = false;
            }

            _educationMaterialMap[EducationMaterialsCollection[0]] = true;
            SaveMap();
        }
    }
}
