using System.Collections.Generic;

namespace GraphApp.Model
{
    /// <summary>
    /// Практическое задание в виде построения графа.
    /// </summary>
    public class PracticTask: EducationMaterial
    {
        /// <summary>
        /// Текст задания.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Коллекция вершин.
        /// </summary>
        public List<VisualVertex> Vertices { get; set; }

        /// <summary>
        /// Коллекция связей.
        /// </summary>
        public List<VisualConnection> Connections { get; set; }

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
