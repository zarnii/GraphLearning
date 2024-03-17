using GraphApp.Interfaces;
using GraphApp.Model;
using System;
using System.Collections.Generic;

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
        /// Поставщик тестов.
        /// </summary>
        private ITestProvider _testProvider;

        /// <summary>
        /// Поставщик практических заданий.
        /// </summary>
        private IPracticProvider _practicProvider;

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
        public AccessControlService(ITestProvider testProvider, IPracticProvider practicProvider)
        {
            _testProvider = testProvider;
            _practicProvider = practicProvider;

            EducationMaterialsCollection = new EducationMaterialNode[
                testProvider.TestCollection.Count + practicProvider.PracticCollection.Count];
            _educationMaterialMap = new Dictionary<EducationMaterialNode, bool>(
                testProvider.TestCollection.Count + practicProvider.PracticCollection.Count);

            InitFields();
        }

        public void OpenNext(EducationMaterialNode material)
        {
            if (!_educationMaterialMap[material])
            {
                throw new ArgumentException("Невозможно открыть следующий элемент, так как текущий элемент закрыт.");
            }

            var materialIndex = EducationMaterialsCollection.FindIndex<EducationMaterialNode>(material);

            _educationMaterialMap[EducationMaterialsCollection[materialIndex + 1]] = true;
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
        /// <param name="testProvider">Поставщик тестов.</param>
        /// <param name="practicProvider">Поставщик практик.</param>
        private void InitFields()
        {
            var iter = 0;

            for (var i = 0; i < _testProvider.TestCollection.Count; i++)
            {
                EducationMaterialsCollection[iter] = new EducationMaterialNode(
                    _testProvider.TestCollection[i],
                    CheckCanGetMaterial
                );
                iter++;
            }

            for (var i = 0; i < _practicProvider.PracticCollection.Count; i++)
            {
                EducationMaterialsCollection[iter] = new EducationMaterialNode(
                    _practicProvider.PracticCollection[i],
                    CheckCanGetMaterial
                );
                iter++;
            }

            Array.Sort<EducationMaterialNode>(EducationMaterialsCollection);
        }
    }
}
