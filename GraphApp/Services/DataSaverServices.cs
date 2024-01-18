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
	/// Сервис сохранения данных.
	/// </summary>
	public class DataSaverServices : IDataSaver
	{
		/// <summary>
		/// Сохранить.
		/// </summary>
		/// <param name="path">Путь до места сохранения.</param>
		/// <param name="vertices">Вершины.</param>
		/// <param name="connections">Связи.</param>
		public void Save(string path, List<SerializableVertex> vertices, List<SerializableConnection> connections)
		{
			var json = JsonSerializer.Serialize(new SerializableData()
			{
				Vertices = vertices,
				Connections = connections
			});

			File.WriteAllText($"{path}\\{ConfigurationManager.AppSettings["defaultSaveFileName"]}", json);
		}
	}
}
