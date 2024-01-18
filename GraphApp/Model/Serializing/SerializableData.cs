using System.Collections.Generic;

namespace GraphApp.Model.Serializing
{
	/// <summary>
	/// Сериализуемые данные.
	/// </summary>
	public class SerializableData
	{
		public List<SerializableVertex> Vertices { get; set; }

		public List<SerializableConnection> Connections { get; set; }
	}
}
