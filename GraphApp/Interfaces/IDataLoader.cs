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
		TLoad Load<TLoad>();

		TLoad Load<TLoad>(string path);
	}
}
