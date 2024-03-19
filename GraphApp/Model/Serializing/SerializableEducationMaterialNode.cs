namespace GraphApp.Model.Serializing
{
    /// <summary>
    /// Сериализуемый обучающий материал.
    /// </summary>
    public class SerializableEducationMaterialNode
    {
        /// <summary>
        /// Последовательный номер.
        /// </summary>
        public int IndexNumber { get; set; }

        /// <summary>
        /// Флаг, показывающий открыт ли материал.
        /// </summary>
        public bool Flag { get; set; }
    }
}
