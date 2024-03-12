using GraphApp.Interfaces;
using GraphApp.Model;
using System;
using System.Collections.Generic;

namespace GraphApp.Services
{
    public class AccessControlService : IAccessControlService
    {
        /// <summary>
        /// Карта сопоставления флага, показывающий открыт ли материал, и материал. 
        /// </summary>
        private Dictionary<EducationMaterialNode, bool> _educationMaterialMap;

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
            EducationMaterialsCollection = new EducationMaterialNode[
                testProvider.TestCollection.Count + practicProvider.PracticCollection.Count];
            _educationMaterialMap = new Dictionary<EducationMaterialNode, bool>(
                testProvider.TestCollection.Count + practicProvider.PracticCollection.Count);

            InitFields(testProvider, practicProvider);
        }

        public void OpenNext(EducationMaterialNode material)
        {
            if (!_educationMaterialMap[material])
            {
                throw new ArgumentException("Невозможно открыть следующий элемент, так как текущий элемент закрыт.");
            }

            var materialIndex = EducationMaterialsCollection.FindIndex<EducationMaterialNode>(material);

            _educationMaterialMap[EducationMaterialsCollection[materialIndex]] = true;
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
        private void InitFields(ITestProvider testProvider, IPracticProvider practicProvider)
        {
            {
                var material = new EducationMaterialNode(
                    testProvider.TestCollection[0],
                    CheckCanGetMaterial
                );
                EducationMaterialsCollection[0] = material;
                _educationMaterialMap[material] = true;
            }

            {
                var practic = practicProvider.PracticCollection[0];
                var material = new EducationMaterialNode(
                    practic,
                    CheckCanGetMaterial
                );
                EducationMaterialsCollection[1] = material;
                _educationMaterialMap[material] = false;
            }
        }
    }
}
