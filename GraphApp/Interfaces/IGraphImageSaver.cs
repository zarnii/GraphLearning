using GraphApp.Model;
using System.Collections.Generic;

namespace GraphApp.Interfaces
{
    /// <summary>
    /// Сервис сохранения графа в виде изображения.
    /// </summary>
    public interface IGraphImageSaver
    {
        /// <summary>
        /// Сохранить граф в виде png файла.
        /// </summary>
        /// <param name="vertices">Коллекция вершин.</param>
        /// <param name="connections">Коллекция связей.</param>
        /// <param name="pathToSaveDir">Путь до файла.</param>
        /// <param name="surfaceWidth">Ширина поверхности.</param>
        /// <param name="surfaceHeight">Высота поверхности.</param>
        void SaveAsPng(
            ICollection<VisualVertex> vertices,
            ICollection<VisualConnection> connections,
            string pathToSaveDir,
            double surfaceWidth,
            double surfaceHeight);
    }
}
