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
    /// Загрузчик данных.
    /// </summary>
    public class DataLoaderServices : IDataLoader
	{
		private readonly OpenFileDialog _openFile;

		public DataLoaderServices()
		{
			_openFile = new OpenFileDialog();
			_openFile.Filter = "Json files (*.json)|*.json";
		}

		/// <summary>
		/// Считывание данных из json.
		/// </summary>  
		/// <param name="path">Путь до файла.</param>
		/// <returns>Лист веришн и лист связей.</returns>
		public (List<SerializableVertex>, List<SerializableConnection>) Load()
		{
			if (_openFile.ShowDialog() == true)
			{
				var jsonData = File.ReadAllText(_openFile.FileName);
				var data = JsonSerializer.Deserialize<SerializableData>(jsonData);

				return (data.Vertices, data.Connections);
			}

			return(null, null);
		}
	}
}
