using System.Collections.Generic;

namespace GraphApp.Model
{
    public class CreateMatrixTask : EducationMaterial
    {
        public CreatableMatrixType CreatableMatrixType { get; set; }

        public IList<VisualVertex> Vertices { get; set; }

        public IList<VisualConnection> Connections { get; set; }
    }
}
