namespace GraphApp.Model.Serializing
{
    public class SerializableCreateMatrixTask : EducationMaterial
    {
        public CreatableMatrixType CreatableMatrixType { get; set; }

        public SerializableData Graph { get; set; }
    }
}
