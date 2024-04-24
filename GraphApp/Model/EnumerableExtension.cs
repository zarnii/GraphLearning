using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Model
{
    /// <summary>
    /// Расширение IEnumerable.
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// Поиск индекса O(n).
        /// </summary>
        /// <typeparam name="TCollection">Ссылочный тип.</typeparam>
        /// <param name="collection">Коллекция.</param>
        /// <param name="collectionItem">Элемент коллекции.</param>
        /// <returns>Индекс элемента.</returns>
        /// <exception cref="ArgumentNullException">В качетсве аргумента передан NULL.</exception>
        /// <exception cref="ArgumentException">Элемента нет в коллеции.</exception>
        public static int FindIndex<TCollection>(this IEnumerable<TCollection> collection, TCollection collectionItem)
            where TCollection: class
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (collectionItem == null)
            {
                throw new ArgumentNullException(nameof(collectionItem));
            }

            var index = 0;

            foreach (var item in collection)
            {
                if (item == collectionItem)
                {
                    return index;
                }

                index++;
            }

            throw new ArgumentException("Объект не был найден", nameof(collectionItem));
        }
    }
}
