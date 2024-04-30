using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.Model.Exception;
using GraphApp.Model.Serializing;
using GraphApp.Services.Providers;
using Microsoft.Extensions.DependencyInjection;
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
        /// Событие изменение карты прогресса.
        /// </summary>
        public event EventHandler EducationMaterialMapChanged;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="testProvider">Поставщик тестов.</param>
        /// <param name="practicProvider">Поставщик практик.</param>
        public AccessControlService(
            [FromKeyedServices(typeof(TestProvider))]IEducationMaterialProvider testProvider, 
            [FromKeyedServices(typeof(PracticProvider))]IEducationMaterialProvider practicProvider,
            [FromKeyedServices(typeof(CreateMatrixTaskProvider))]IEducationMaterialProvider createMatrixTaskProvider,
            IDataHandlerService dataHandler)
        {
            _dataHandler = dataHandler;

            var testCollection = testProvider.GetMaterialCollection();
            var practicCollection = practicProvider.GetMaterialCollection();
            var createMatrixTaskCollection = createMatrixTaskProvider.GetMaterialCollection();

            
            EducationMaterialsCollection = new EducationMaterialNode[
                testCollection.Count + practicCollection.Count + createMatrixTaskCollection.Count];
            EducationMaterialMap = new Dictionary<EducationMaterialNode, EducationMaterialInfo>(
                testCollection.Count + practicCollection.Count + createMatrixTaskCollection.Count);
            
            InitNodesCollection(testCollection, practicCollection, createMatrixTaskCollection);
            LoadMap(dataHandler);
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

            if (!EducationMaterialMap[material].IsOpen.Value)
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
            return EducationMaterialMap[material].IsCompleted.Value;
        }

        /// <summary>
        /// Добавление попытки прохождения к обучающему материалу.
        /// </summary>
        /// <param name="material"></param>
        public void AddAttempt(EducationMaterialNode material)
        {
            EducationMaterialMap[material].AttemptsNumber++;
            EducationMaterialMapChanged?.Invoke(null, null);
        }

        public void ResetMap()
        {
            OnErrorLoadMap();
        }

        /// <summary>
        /// Проверка можно ли получить обучающий материал.
        /// </summary>
        /// <param name="material">Узел обучающего материала.</param>
        /// <returns>True, если можно.</returns>
        private bool CheckCanGetMaterial(EducationMaterialNode material)
        {
            if (EducationMaterialMap[material].IsOpen.Value)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Инициализация полей.
        /// </summary>
        private void InitNodesCollection(
            IList<EducationMaterial> testCollection, 
            IList<EducationMaterial> practicCollection,
            IList<EducationMaterial> createMatrixCollection)
        {
            var iter = 0;

            for (var i = 0; i < testCollection.Count; i++)
            {
                var educationMaterialNode = new EducationMaterialNode(
                    testCollection[i],
                    CheckCanGetMaterial
                );

                EducationMaterialsCollection[iter] = educationMaterialNode;
                iter++;
            }

            for (var i = 0; i < practicCollection.Count; i++)
            {
                var educationMaterialNode = new EducationMaterialNode(
                    practicCollection[i],
                    CheckCanGetMaterial
                );

                EducationMaterialsCollection[iter] = educationMaterialNode;
                iter++;
            }

            for (var i = 0; i < createMatrixCollection.Count; i++)
            {
                var educationMaterialNode = new EducationMaterialNode(
                    createMatrixCollection[i],
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
        private void LoadMap(IDataLoader dataLoader)
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

                if ( infoCollection.Length !=  EducationMaterialsCollection.Length)
                {
                    throw new LoadDataException();
                }

                foreach (var info in infoCollection)
                {
                    if (info.IndexNumber == null
                        || info.IsOpen == null
                        ||info.AttemptsNumber == null
                        || info.IsCompleted == null)
                    {
                        throw new LoadDataException();
                    }

                    var node = EducationMaterialsCollection
                        .Where(e => e.EducationMaterialIndexNumber == info.IndexNumber)
                        .FirstOrDefault();
                    
                    if (node == null)
                    {
                        throw new ArgumentNullException();
                    }

                    EducationMaterialMap[node] = info;
                }

                if (EducationMaterialMap.Count != EducationMaterialsCollection.Length)
                {
                    throw new LoadDataException();
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
                MessageBox.Show(
                    "При загрузке прогресса произошла ошибка. " +
                    "Возможно, файлы были удалены или изменены не должным образом. Прогресс был сброшен.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );

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
                var fileStream = File.Create($"{pathToFile}/{fileName}");
                fileStream.Close();
            }

            var educationMaterialInfoCollection = new List<EducationMaterialInfo>(EducationMaterialsCollection.Length);

            foreach (var node in EducationMaterialsCollection)
            {
                educationMaterialInfoCollection.Add(EducationMaterialMap[node]);
            }

            _dataHandler.Save(educationMaterialInfoCollection, $"{pathToFile}/{fileName}");
            EducationMaterialMapChanged?.Invoke(null, null);
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
                    IsCompleted = false,
                };
            }

            EducationMaterialMap[EducationMaterialsCollection[0]].IsOpen = true;
            SaveMap();
        }
    }
}
