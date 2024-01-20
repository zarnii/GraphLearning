using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.Model.Serializing;
using Microsoft.Win32;
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
		private readonly SaveFileDialog _saveFile;

		public DataSaverServices()
		{
			_saveFile = new SaveFileDialog();
			_saveFile.Filter = "Json files (*.json)|*.json";
		}

		/// <summary>
		/// Сохранить.
		/// </summary>
		/// <param name="path">Путь до места сохранения.</param>
		/// <param name="vertices">Вершины.</param>
		/// <param name="connections">Связи.</param>
		public void Save(List<SerializableVertex> vertices, List<SerializableConnection> connections)
		{
			if (_saveFile.ShowDialog() == true)
			{
				var json = JsonSerializer.Serialize(new SerializableData()
				{
					Vertices = vertices,
					Connections = connections
				});

				File.WriteAllText(_saveFile.FileName, json);
			}
		}
	}
}
