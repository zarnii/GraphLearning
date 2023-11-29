using System.Windows;

namespace GraphApp.Strategy
{
	/// <summary>
	/// Алгоритм создания вершины.
	/// </summary>
	public class CreateVertexAlgorithm : Algorithm
	{
		/// <summary>
		/// Выполнение алгоритма.
		/// </summary>
		/// <param name="parameter">Параметр для выполнения лагоритма.</param>
		public override void Execute(object parameter)
		{
			var point = (Point)parameter;

			// ...
		}
	}
}
