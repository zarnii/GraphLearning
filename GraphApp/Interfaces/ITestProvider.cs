using GraphApp.Model;
using System.Collections.Generic;

namespace GraphApp.Interfaces
{
	/// <summary>
	/// Поставщик тестов.
	/// </summary>
	public interface ITestProvider
	{
		/// <summary>
		/// Текущий тест.
		/// </summary>
		Test CurrentTest { get; set; }

		/// <summary>
		/// Коллекция Тестов.
		/// </summary>
		List<Test> TestCollection { get; }
	}
}
