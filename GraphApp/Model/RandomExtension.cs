using System;
using System.Collections.Generic;

namespace GraphApp.Model
{
    public static class RandomExtension
    {
        /// <summary>
        /// Случайный выбор из коллекции.
        /// </summary>
        /// <typeparam name="TItem">Тип, хранящийся в коллекции.</typeparam>
        /// <param name="random">Экземпляр Randome.</param>
        /// <param name="collection">Коллекция.</param>
        /// <returns>Случайный элемент коллекции.</returns>
        /// <exception cref="ArgumentNullException">Передан NULL в качестве аргумента.</exception>
        public static TItem Randchoice<TItem>(this Random random, IList<TItem> collection)
        {
            if (random == null)
            {
                throw new ArgumentNullException(nameof(random), "Пустой экземпляр random.");
            }

            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection), "Пустой экземпляр коллекции.");
            }

            return collection[random.Next(0, collection.Count)];
        }
    }
}
