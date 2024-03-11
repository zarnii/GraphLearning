using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Model.Serializing
{
    /// <summary>
    /// Сериализуемое практическое задание в виде составления графа.
    /// </summary>
    public class SerializablePracticTask
    {
        /// <summary>
        /// Заголовок задания.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Текст задания.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Коллекция вершин.
        /// </summary>
        public List<SerializableVertex> Vertices { get; set; }

        /// <summary>
        /// Коллекция связей.
        /// </summary>
        public List<SerializableConnection> Connections { get; set; }

        /// <summary>
        /// Флаг, показывающий надо ли проверять количество вершин.
        /// </summary>
        public bool NeedCheckVertexCount { get; set; }

        /// <summary>
        /// Флаг, показывающий надо ли проверять позицию вершин.
        /// </summary>
        public bool NeedCheckVertexPosition { get; set; }

        /// <summary>
        /// Флаг, показывающий надо ли проверять размер вершин.
        /// </summary>
        public bool NeedCheckVertexSize { get; set; }

        /// <summary>
        /// Флаг, показывающий надо ли проверять название вершин.
        /// </summary>
        public bool NeedCheckVertexName { get; set; }

        /// <summary>
        /// Флаг, показывающий надо ли проверять связанные вершины.
        /// </summary>
        public bool NeedCheckConnection { get; set; }

        /// <summary>
        /// Флаг, показывающий надо ли проверять количество связей.
        /// </summary>
        public bool NeedCheckConnectionCount { get; set; }

        /// <summary>
        /// Флаг, показывающий надо ли проверять вес связей.
        /// </summary>
        public bool NeedCheckConnectionWeight { get; set; }

        /// <summary>
        /// Флаг, показывающий надо ли проверять тип связей.
        /// </summary>
        public bool NeedCheckConnectionType { get; set; }
    }
}
