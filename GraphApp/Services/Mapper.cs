using GraphApp.Interfaces;
using System;
using System.Collections.Generic;

namespace GraphApp.Services
{
    /// <summary>
    /// Маппер объектов.
    /// </summary>
    public class Mapper : IMapper
    {
        /// <summary>
        /// Карты объектов.
        /// </summary>
        private Dictionary<object, Func<object, object, object>> _map;

        public Mapper()
        {
            _map = new Dictionary<object, Func<object, object, object>>();
        }

        /// <summary>
        /// Создание карты объекта.
        /// </summary>
        /// <typeparam name="TSourse">Объект источник.</typeparam>
        /// <typeparam name="TReceiver">Объект приемник.</typeparam>
        /// <param name="factory">Фабрика.</param>
        public void CreateMap<TSourse, TReceiver>(Func<object, object, TReceiver> factory)
            where TSourse : class
            where TReceiver : class
        {
            _map.Add(typeof(TSourse), factory);
        }

        /// <summary>
        /// Маппинг объекта.
        /// </summary>
        /// <typeparam name="TReceiver">Тип объекта приемника.</typeparam>
        /// <param name="source">Источник.</param>
        /// <param name="param">Необходимые параметры.</param>
        /// <returns>Новый объект типа TReceiver.</returns>
        public TReceiver Map<TReceiver>(object source, object param)
            where TReceiver : class
        {
            return (TReceiver)_map[source.GetType()].Invoke(source, param);
        }
    }
}
