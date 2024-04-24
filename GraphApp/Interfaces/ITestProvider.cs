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
		/// Коллекция Тестов.
		/// </summary>
		List<Test> TestCollection { get; }
	}
}
