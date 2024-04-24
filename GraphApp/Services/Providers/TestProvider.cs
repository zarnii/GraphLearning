using GraphApp.Interfaces;
using GraphApp.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace GraphApp.Services.Providers
{
    /// <summary>
    /// Поставщик вопросов.
    /// </summary>
    public class TestProvider : IEducationMaterialProvider
    {
        #region fields
        /// <summary>
        /// Текущий вопрос.
        /// </summary>
        private IDataLoader _dataLoader;
        #endregion

        #region properties
        #endregion

        #region constructor
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="dataLoader"></param>
        public TestProvider(IDataLoader dataLoader)
        {
            _dataLoader = dataLoader;
        }
        #endregion

        #region private methods
        #endregion

        #region public methods
        /// <summary>
        /// Получение коллекции обучающего материала.
        /// </summary>
        /// <returns>Коллекция обучающего материала.</returns>
        public IList<EducationMaterial> GetMaterialCollection()
        {

            var pathToTests = ConfigurationManager.AppSettings["defaultPathToTests"];

            if (!Directory.Exists(pathToTests))
            {
                return null;
            }

            var files = Directory.GetFiles(pathToTests, "*json");
            var testCollection = new List<EducationMaterial>(files.Length);

            foreach (var file in files)
            {
                testCollection.Add(_dataLoader.Load<Test>(file));
            }

            return testCollection;
        }
        #endregion
    }
}
