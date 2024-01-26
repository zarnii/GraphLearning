using System;
using System.Configuration;
using GraphApp.Interfaces;
using GraphApp.Model;

namespace GraphApp.Services
{
	/// <summary>
	/// Сервис вопросов.
	/// </summary>
	public class QuestionService : IQuestionService
	{
		#region fields
		/// <summary>
		/// Ключ из файла, до текущего вопроса.
		/// </summary>
		private string _currentQuestionPathKey;

		/// <summary>
		/// Сервис загрузки данных.
		/// </summary>
		private IDataLoader _dataLoader;
		#endregion

		#region properties
		/// <summary>
		/// Ключ из файла, до текущего вопроса.
		/// </summary>
		public string CurrentQuestionPathKey
		{
			get
			{
				return _currentQuestionPathKey;
			}
			set
			{
				if (String.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException(nameof(value), "Пустой ключ.");
				}

				_currentQuestionPathKey = value;
			}
		}
		#endregion

		#region constructor
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="dataLoader"></param>
		public QuestionService(IDataLoader dataLoader)
		{
			_dataLoader = dataLoader;
		}
		#endregion

		#region public methods
		/// <summary>
		/// Получение текущего вопроса.
		/// </summary>
		/// <returns></returns>
		public Question GetCurrentQuestion()
		{
			return _dataLoader.Load<Question>(ConfigurationManager.AppSettings[_currentQuestionPathKey]);
		}
		#endregion
	}
}
