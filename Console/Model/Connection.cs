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


		/// <summary>
		/// Обработчик удаления соединения.
		/// </summary>
		private Action<Connection> _deleteConncection;
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
		/// Обработчик удаления соединения.
		/// </summary>
		public Action<Connection> DeleteConncection
		{
			get
			{
				return _deleteConncection;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value), "Пустой обработчик удаления связи.");
				}

				_deleteConncection = value;
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

			ConnectedVertices.Item1.OnDelete += OnVertexDeleted;
			ConnectedVertices.Item2.OnDelete += OnVertexDeleted;
		}
		#endregion


		#region public methods
		/// <summary>
		/// Удаление связи.
		/// </summary>
		public void Delete()
		{
			_deleteConncection?.Invoke(this);
		}
		#endregion


		#region private methods
		/// <summary>
		/// При удалении вершины.
		/// </summary>
		/// <param name="vertex">Удаляемая вершина.</param>
		private void OnVertexDeleted(Vertex vertex)
		{
			_deleteConncection?.Invoke(this);
		}
		#endregion
	}
}
