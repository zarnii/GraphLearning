using GraphApp.Model;
using GraphApp.Model.Serializing;
using System.Collections.Generic;


namespace GraphApp.Interfaces
{
	/// <summary>
	/// Сервис загрузки данных.
	/// </summary>
	public interface IDataLoader
	{
		(List<SerializableVertex>, List<SerializableConnection>) Load(string path);
	}
}
