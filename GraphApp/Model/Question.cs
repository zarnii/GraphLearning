namespace GraphApp.Model
{
	/// <summary>
	/// Вопрос.
	/// </summary>
	public class Question
	{
		/// <summary>
		/// Заголовок вопроса.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Текст вопроса.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Варианты ответа.
		/// </summary>
		public Answer[] Answers { get; set; }
	}
}
