using GraphApp.Model;
using GraphApp.Model.Serializing;
using System.Collections.Generic;

namespace GraphApp.Interfaces
{
	/// <summary>
	/// Сервис сохранения данных.
	/// </summary>
	public interface IDataSaver
	{
		void Save(List<SerializableVertex> vertices, List<SerializableConnection> connections);
	}
}
