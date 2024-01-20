namespace GraphApp.Model.Exception
{
	/// <summary>
	/// Ошибка загрузки данных.
	/// </summary>
	public class LoadDataException : System.Exception
	{
		public LoadDataException()
			: base() { }

		public LoadDataException(string message)
			: base(message) { }

		public LoadDataException(string message, System.Exception innerException):
			base(message, innerException) { }
	}
}
