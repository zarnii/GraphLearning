using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.Model.Serializing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Services
{
    /// <summary>
    /// Сервис обработки данных.
    /// </summary>
    public class DataHeandlerService : IDataHeandlerService
	{
		#region fields
		/// <summary>
		/// Загрузчик данных.
		/// </summary>
		private IDataLoader _dataLoader;

		/// <summary>
		/// Сохранятор(???) данных.
		/// </summary>
		private IDataSaver _dataSaver;
		#endregion

		#region constructor
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="dataLoader">Сервис загрузки данных.</param>
		/// <param name="dataSaver">Сервис сохранения данных.</param>
		/// <exception cref="ArgumentNullException">Пустые аргументы.</exception>
		public DataHeandlerService(IDataLoader dataLoader, IDataSaver dataSaver)
		{
			if (dataLoader == null)
			{
				throw new ArgumentNullException(nameof(dataLoader), "Пустой сервис загрузки данных.");
			}

			if (dataSaver == null)
			{
				throw new ArgumentNullException(nameof(dataSaver), "Пустой сервис сохранения данных.");
			}

			_dataLoader = dataLoader;
			_dataSaver = dataSaver;
		}
		#endregion

		#region public methods
		/// <summary>
		/// Загрузка.
		/// </summary>
		/// <param name="path">Пусть до данных.</param>
		/// <returns>Лист вершин и связей.</returns>
		public (List<SerializableVertex>, List<SerializableConnection>) Load(string path)
		{
			return _dataLoader.Load(path);
		}

		/// <summary>
		/// Сохранение.
		/// </summary>
		/// <param name="path">Путь.</param>
		public void Save(string path, List<SerializableVertex> vertices, List<SerializableConnection> connections)
		{
			_dataSaver.Save(path, vertices, connections);
		}
		#endregion
	}
}
