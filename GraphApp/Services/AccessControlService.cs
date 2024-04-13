using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.Model.Exception;
using GraphApp.Model.Serializing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;

namespace GraphApp.Services
{
    /// <summary>
    /// Сервис контроля доступа.
    /// </summary>
    public class AccessControlService : IAccessControlService
    {
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
        /// Карта сопоставления обучающего материала и доп информации. 
        /// </summary>
        public Dictionary<EducationMaterialNode, EducationMaterialInfo> EducationMaterialMap { get; private set; }

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
            EducationMaterialMap = new Dictionary<EducationMaterialNode, EducationMaterialInfo>(
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
            if (!EducationMaterialMap.ContainsKey(material))
            {
                return;
            }

            if (!EducationMaterialMap[material].IsOpen)
            {
                throw new ArgumentException("Невозможно открыть следующий элемент, так как текущий элемент закрыт.");
            }
            EducationMaterialMap[material].IsCompleted = true;
            
            var materialIndex = EducationMaterialsCollection.FindIndex<EducationMaterialNode>(material);

            if (materialIndex + 1 != EducationMaterialsCollection.Length)
            {
                EducationMaterialMap[EducationMaterialsCollection[materialIndex + 1]].IsOpen = true;
                SaveMap();
            }
        }

        /// <summary>
        /// Проверка, пройден ли обучающий материал.
        /// </summary>
        /// <param name="material">Обучающий материал.</param>
        /// <returns>True, если пройден.</returns>
        public bool CheckEducationMaterialIsPassed(EducationMaterialNode material)
        {
            return EducationMaterialMap[material].IsCompleted;
        }

        /// <summary>
        /// Добавление попытки прохождения к обучающему материалу.
        /// </summary>
        /// <param name="material"></param>
        public void AddAttempt(EducationMaterialNode material)
        {
            EducationMaterialMap[material].AttemptsNumber++;
        }

        /// <summary>
        /// Проверка можно ли получить обучающий материал.
        /// </summary>
        /// <param name="material">Узел обучающего материала.</param>
        /// <returns>True, если можно.</returns>
        private bool CheckCanGetMaterial(EducationMaterialNode material)
        {
            if (EducationMaterialMap[material].IsOpen)
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
                var infoCollection = dataLoader.Load<EducationMaterialInfo[]>($"{pathToFile}/{fileName}");

                foreach (var info in infoCollection)
                {
                    var node = EducationMaterialsCollection
                        .Where(e => e.EducationMaterialIndexNumber == info.IndexNumber)
                        .FirstOrDefault();
                    
                    if (node == null)
                    {
                        throw new ArgumentNullException();
                    }

                    EducationMaterialMap[node] = info;
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
            catch(LoadDataException ex)
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

            if (!File.Exists($"{pathToFile}/{fileName}"))
            {
                File.Create($"{pathToFile}/{fileName}");
            }

            var educationMaterialInfoCollection = new List<EducationMaterialInfo>(EducationMaterialsCollection.Length);

            foreach (var node in EducationMaterialsCollection)
            {
                educationMaterialInfoCollection.Add(EducationMaterialMap[node]);
            }

            _dataHandler.Save(educationMaterialInfoCollection, $"{pathToFile}/{fileName}");
        }

        /// <summary>
        /// При ошибке загрузки прогресса пользователя.
        /// </summary>
        private void OnErrorLoadMap()
        {
            foreach (var node in EducationMaterialsCollection)
            {
                EducationMaterialMap[node] = new EducationMaterialInfo()
                {
                    IndexNumber = node.EducationMaterialIndexNumber,
                    AttemptsNumber = 0,
                    IsOpen = false,
                };
            }

            EducationMaterialMap[EducationMaterialsCollection[0]].IsOpen = true;
            SaveMap();
        }
    }
}
