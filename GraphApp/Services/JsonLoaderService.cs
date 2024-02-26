using GraphApp.Interfaces;
using GraphApp.Model.Exception;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text.Json;

namespace GraphApp.Services
{
	/// <summary>
	/// Загрузчик данных.
	/// </summary>
	public class JsonLoaderService : IDataLoader
	{
		private readonly OpenFileDialog _openFile;

		public JsonLoaderService()
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
		/// Считывание данных из файла json.
		/// </summary>
		/// <typeparam name="TLoad">Сериализуемый тип.</typeparam>
		/// <param name="path">Путь до файла.</param>
		/// <returns>Считанные данные.</returns>
		/// <exception cref="ArgumentNullException">Пустой путь до файла.</exception>
		/// <exception cref="LoadDataException">Неправильный формат файла.</exception>
		public TLoad Load<TLoad>(string path)
		{
			if (String.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException(nameof(path), "Пустой путь до файла.");
			}

			if (!File.Exists(path))
			{
				throw new DirectoryNotFoundException($"Файл по пути {path} не найден.");
			}

			try
			{
				var jsonData = File.ReadAllText(path);
				var data = JsonSerializer.Deserialize<TLoad>(jsonData);

				return data;
			}
			catch (JsonException ex)
			{
				throw new LoadDataException("Неудалось загрузить данные из файла.");
			}

		}
	}
}
