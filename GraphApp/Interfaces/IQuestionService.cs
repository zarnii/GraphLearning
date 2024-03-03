using GraphApp.Model;
using System.Collections.Generic;

namespace GraphApp.Interfaces
{
	/// <summary>
	/// Сервис вопросов.
	/// </summary>
	public interface IPracticeService
	{
		/// <summary>
		/// Текущий вопрос.
		/// </summary>
		Question CurrentQuestion { get; set; }

		/// <summary>
		/// Коллекция вопросов.
		/// </summary>
		List<Question> Questions { get; }
	}
}
