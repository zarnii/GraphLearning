using GraphApp.Model;

namespace GraphApp.Interfaces
{
	/// <summary>
	/// Сервис вопросов.
	/// </summary>
	public interface IQuestionService
	{
		/// <summary>
		/// Ключ в app.config до текущего вопроса.
		/// </summary>
		string CurrentQuestionPathKey { get; set; }

		/// <summary>
		/// Получение текущего открытого вопроса.
		/// </summary>
		/// <returns>Текущий вопрос.</returns>
		Question GetCurrentQuestion();
	}
}
