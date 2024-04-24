using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Model
{
    /// <summary>
    /// Класс, инкапсулирующий KeyValuePair.
    /// </summary>
    /// <typeparam name="TKey">Тип ключа.</typeparam>
    /// <typeparam name="TValue">Тип значения.</typeparam>
    public class KeyValuePairClass<TKey, TValue>
    {
        /// <summary>
        /// Пара.
        /// </summary>
        public KeyValuePair<TKey, TValue> Pair { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="value">Значение.</param>
        public KeyValuePairClass(TKey key, TValue value)
        {

            Pair = new KeyValuePair<TKey, TValue>(key, value);
        }
    }
}
