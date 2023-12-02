using System;

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
			set
			{
				if (value.Item1 == null)
				{
					throw new ArgumentNullException(nameof(value.Item1), "Пустое значение картежа.");
				}

				if (value.Item1 == null)
				{
					throw new ArgumentNullException(nameof(value.Item2), "Пустое значение картежа.");
				}

				_connectedVertices = value;
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
		#endregion

		#region constructor
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="connectedVertices">Соеденяемые вершины.</param>
		/// <param name="weight">Вес соединения.</param>
		/// <param name="connectionType">Тип соединения.</param>
		public Connection((Vertex, Vertex) connectedVertices, double weight = 0, ConnectionType connectionType = ConnectionType.Unidirectional)
		{
			ConnectedVertices = connectedVertices;
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
