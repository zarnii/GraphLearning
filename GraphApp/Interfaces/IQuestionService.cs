using GraphApp.Model;
using System.Collections.Generic;

namespace GraphApp.Interfaces
{
	/// <summary>
	/// Сервис вопросов.
	/// </summary>
	public interface IQuestionService
	{
		/// <summary>
		/// Текущий вопрос.
		/// </summary>
		Question CurrentQuestion { get; set; }

		/// <summary>
		/// Коллекция вопросов.
		/// </summary>
		IList<Question> Questions { get; }
	}
}
