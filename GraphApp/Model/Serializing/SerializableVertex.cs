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
		public int X { get; set; }

		public int Y { get; set; }

		public int Number { get; set; }

		public string Name { get; set; }
	}
}
