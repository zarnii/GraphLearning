using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Strategy
{
	/// <summary>
	/// Алгоритм.
	/// </summary>
	public abstract class Algorithm
	{
		/// <summary>
		/// Выполнение алгоритма.
		/// </summary>
		/// <param name="parameter">Параметр.</param>
		public abstract void Execute(object parameter);
	}
}
