using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Model
{
    /// <summary>
    /// Последний граф пользователя, построенный в определенном заднии.
    /// </summary>
    public static class LastUserGraph
    {
        public static IList<VisualVertex> Vertices { get; private set; }

        public static IList<VisualConnection> Connections { get; private set; }

        public static int IndexNumber { get; private set; }

        public static void SetLastGraph(IList<VisualVertex> vertices, IList<VisualConnection> connections, int indexNumber)
        {
            Vertices = vertices;
            Connections = connections;
            IndexNumber = indexNumber;
        }
    }
}
