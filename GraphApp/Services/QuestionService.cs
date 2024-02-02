using System;
using System.Collections.Generic;
using System.Configuration;
using GraphApp.Interfaces;
using GraphApp.Model;
using System.IO;
using System.Linq;
using System.Collections;

namespace GraphApp.Services
{
	/// <summary>
	/// Сервис вопросов.
	/// </summary>
	public class QuestionService : IQuestionService
	{
		#region fields
		/// <summary>
		/// Текущий вопрос.
		/// </summary>
		private Question _currentQuestion;

		/// <summary>
		/// Ключ из файла, до текущего вопроса.
		/// </summary>
		private IList<Question> _questions;

		/// <summary>
		/// Сервис загрузки данных.
		/// </summary>
		private IDataLoader _dataLoader;
		#endregion

		#region properties
		/// <summary>
		/// Коллекция вопросов.
		/// </summary>
		public IList<Question> Questions
		{
			get
			{
				return _questions;
			}
		}

		/// <summary>
		/// Текущий вопрос.
		/// </summary>
		public Question CurrentQuestion
		{
			get
			{
				return _currentQuestion;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value), "Пустой вопрос.");
				}

				_currentQuestion = value;
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
			_questions = new List<Question>();

			InitQuestions();
		}
		#endregion

		#region public methods
		/// <summary>
		/// Инициализация вопросов.
		/// </summary>
		public void InitQuestions()
		{
			if (_questions.Count != 0)
			{
				return;
			}

			var paths = Directory.GetFiles(ConfigurationManager.AppSettings["defaultPathToQuestions"]);

			foreach (string path in paths)
			{
				_questions.Add(_dataLoader.Load<Question>(path));
			}
		}
		#endregion
	}
}
