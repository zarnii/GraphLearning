using System;
using System.Collections.Generic;

namespace GraphApp.Model
{
	/// <summary>
	/// Обработчик связей.
	/// </summary>
	public class ConnectionHeandler
	{
		#region fields
		/// <summary>
		/// Лист связей.
		/// </summary>
		private List<Connection> _connectionsList;
		#endregion


		#region properties
		/// <summary>
		/// Лист связей.
		/// </summary>
		public List<Connection> Connections
		{
			get
			{
				return _connectionsList;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value), "Пустой лист связей.");
				}

				_connectionsList = value;
			}
		}
		#endregion


		#region constructor
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="connections">Лист связей.</param>
		public ConnectionHeandler(List<Connection> connections)
		{
			Connections = connections;
		}
		#endregion


		#region public methods
		/// <summary>
		/// Добавление связи.
		/// </summary>
		/// <param name="connection">Добовляемая связь.</param>
		public void AddConnection(Connection connection)
		{
			Connections.Add(connection);
			connection.DeleteConncection += DeleteConnection;
		}
		#endregion


		#region private methods
		/// <summary>
		/// Удаление связи.
		/// </summary>
		/// <param name="connection">Удаляемая связь.</param>
		private void DeleteConnection(Connection connection)
		{
			_connectionsList.Remove(connection);
		}
		#endregion
	}
}
