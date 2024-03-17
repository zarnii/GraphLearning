using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Model
{
    /// <summary>
    /// Узел, инкапсулирующий материал.
    /// </summary>
    public class EducationMaterialNode: IComparable<EducationMaterialNode>
    {
        /// <summary>
        /// Обучающий материал.
        /// </summary>
        private EducationMaterial _educationMaterial;

        /// <summary>
        /// Функция проверки получения материала.
        /// </summary>
        private Func<EducationMaterialNode, bool> _checkCanGetMaterial;

        /// <summary>
        /// Обучающий материал.
        /// </summary>
        public EducationMaterial EducationMaterial
        {
            get
            {
                if (_checkCanGetMaterial.Invoke(this))
                {
                    return _educationMaterial;
                }

                return null;
            }
        }

        /// <summary>
        /// Заголовок материала.
        /// </summary>
        public string EducationMaterialTitle
        {
            get
            {
                return _educationMaterial.Title;
            }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="educationMaterial">Обучающий материал.</param>
        /// <param name="checkCanGetMaterial">Функция проверки.</param>
        /// <exception cref="ArgumentNullException">Перелан NULL в качестве аргумента.</exception>
        public EducationMaterialNode(
            EducationMaterial educationMaterial,
            Func<EducationMaterialNode, bool> checkCanGetMaterial)
        {
            if (educationMaterial == null)
            {
                throw new ArgumentNullException(nameof(educationMaterial));
            }

            if (checkCanGetMaterial == null)
            {
                throw new ArgumentNullException(nameof(checkCanGetMaterial));
            }

            _educationMaterial = educationMaterial;
            _checkCanGetMaterial = checkCanGetMaterial;
        }

        public int CompareTo(EducationMaterialNode? other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("Пустой объект для сравнений.");
            }

            return _educationMaterial.CompareTo(other._educationMaterial);
        }
    }
}
