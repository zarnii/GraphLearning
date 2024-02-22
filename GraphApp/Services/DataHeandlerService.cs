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
		/// Загрузка данных из json.
		/// </summary>
		/// <typeparam name="TLoad">Тип считываемых данных.</typeparam>
		/// <returns>Данные.</returns>
		public TLoad Load<TLoad>()
		{
			return _dataLoader.Load<TLoad>();
		}

		/// <summary>
		/// Загрузка данных из json.
		/// </summary>
		/// <typeparam name="TLoad">Тип считываемых данных.</typeparam>
		/// <param name="path">Путь до файла.</param>
		/// <returns>Данные.</returns>
		public TLoad Load<TLoad>(string path)
		{
			return _dataLoader.Load<TLoad>(path);
		}

		/// <summary>
		/// Сохранение данных.
		/// </summary>
		/// <typeparam name="TSave">Тип данных.</typeparam>
		/// <param name="data">Данные.</param>
		public void Save<TSave>(TSave data)
		{
			_dataSaver.Save<TSave>(data);
		}

		/// <summary>
		/// Сохранение данных.
		/// </summary>
		/// <typeparam name="TSave">Тип данных.</typeparam>
		/// <param name="data">Данные.</param>
		/// <param name="path">Путь.</param>
		public void Save<TSave>(TSave data, string path)
		{
			_dataSaver.Save<TSave>(data, path);
		}
		#endregion
	}
}
