using System;
using System.Text.Json.Serialization;

namespace GraphApp.Model
{
	/// <summary>
	/// Связь врешин.
	/// </summary>
	public class Connection
	{
		#region fields
		/// <summary>
		/// Связанные вершины.
		/// </summary>
		private (Vertex, Vertex) _connectedVertices;
		#endregion

		#region properties
		/// <summary>
		/// Связанные вершины.
		/// </summary>
		public (Vertex, Vertex) ConnectedVertices
		{
			get
			{
				return _connectedVertices;
			}
		}

		/// <summary>
		/// Вес соединения.
		/// </summary>
		public double Weight { get; set; }

		/// <summary>
		/// Тип соединения вершин.
		/// </summary>
		public ConnectionType ConnectionType { get; set; }

		/// <summary>
		/// Номер связи.
		/// </summary>
		public int Number { get; set; }
		#endregion

		#region constructor
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="vertices">Связанные вершины.</param>
		/// <param name="number">Номер свящи.</param>
		/// <param name="weight">Вес связи.</param>
		/// <param name="connectionType">Тип связи.</param>
		public Connection((Vertex, Vertex) vertices,
            int number,
            double weight = 0,
			ConnectionType connectionType = ConnectionType.Unidirectional)
		{
			_connectedVertices = vertices;
			Number = number;
			Weight = weight;
			ConnectionType = connectionType;
		}
		#endregion

		#region public methods
		#endregion

		#region private methods
		#endregion
	}
}
