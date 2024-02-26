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
	/// Сервис сохранения данных в формате Json.
	/// </summary>
	public class JsonSaverService : IDataSaver
	{
		private readonly SaveFileDialog _saveFile;

		public JsonSaverService()
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
		public void Save<TSave>(TSave data)
		{
			if (_saveFile.ShowDialog() == false)
			{
				return;
			}

			Save<TSave>(data, _saveFile.FileName);
		}

		public void Save<TSave>(TSave data, string path)
		{
			var json = JsonSerializer.Serialize(data);
			File.WriteAllText(path, json);
		}
	}
}
