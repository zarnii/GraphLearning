namespace GraphApp.Model
{
	/// <summary>
	/// Вариант ответа.
	/// </summary>
	public class Answer
	{
		/// <summary>
		/// Текст ответа.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Флаг, показывающий верный ли ответ.
		/// </summary>
		public bool Flag { get; set; }
	}
}
