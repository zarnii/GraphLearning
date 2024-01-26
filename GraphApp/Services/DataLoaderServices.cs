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
		/// <typeparam name="TLoad">Сериализуемый тип.</typeparam>
		/// <returns>Считанные данные.</returns>
		public TLoad Load<TLoad>()
		{
			if (_openFile.ShowDialog() == false)
			{
				return default(TLoad);
			}

			return Load<TLoad>(_openFile.FileName);
		}

		/// <summary>
		/// Считывание данных из json.
		/// </summary>
		/// <typeparam name="TLoad">Сериализуемый тип.</typeparam>
		/// <param name="path">Путь до файла.</param>
		/// <returns>Считанные данные.</returns>
		public TLoad Load<TLoad>(string path)
		{
			var jsonData = File.ReadAllText(path);
			var data = JsonSerializer.Deserialize<TLoad>(jsonData);

			return data;
		}
	}
}
