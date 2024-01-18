using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.Model.Serializing;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text.Json;

namespace GraphApp.Services
{
    /// <summary>
    /// Загрузчик данных.
    /// </summary>
    public class DataLoaderServices : IDataLoader
	{
		/// <summary>
		/// Считывание данных из json.
		/// </summary>  
		/// <param name="path">Путь до файла.</param>
		/// <returns>Лист веришн и лист связей.</returns>
		public (List<SerializableVertex>, List<SerializableConnection>) Load(string path)
		{
			var data = JsonSerializer.Deserialize<SerializableData>(File.ReadAllText($"{path}\\dump.json"));

			return (data.Vertices, data.Connections);
		}
	}
}
