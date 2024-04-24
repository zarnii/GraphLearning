using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Model.Serializing
{
	/// <summary>
	/// Сериализуемая вершина.
	/// </summary>
	public class SerializableVertex
	{
		public double X { get; set; }

		public double Y { get; set; }

		public int Number { get; set; }

		public string Name { get; set; }

		public string ColorString { get; set; }

		public int Radius { get; set; }
	}
}
