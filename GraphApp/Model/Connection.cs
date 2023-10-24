using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GraphApp.Model
{
	/// <summary>
	/// Связь врешин.
	/// </summary>
	public class Connection: INotifyPropertyChanged
	{
		#region fields
		/// <summary>
		/// Связанные вершины.
		/// </summary>
		private (Vertex, Vertex) _connectedVertices;


		/// <summary>
		/// Обработчик удаления соединения.
		/// </summary>
		private Action<Connection> _onDelete;


		/// <summary>
		/// Событие изменения свойства.
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;
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
		public Action<Connection> OnDelete
		{
			get
			{
				return _onDelete;
			}
			set
			{
				_onDelete = value;
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

			ConnectedVertices.Item1.OnDelete += Delete;
			ConnectedVertices.Item2.OnDelete += Delete;
		}
		#endregion


		#region public methods
		/// <summary>
		/// Удаление связи.
		/// </summary>
		public void Delete(Vertex vertex)
		{
			ConnectedVertices.Item1.OnDelete -= Delete;
			ConnectedVertices.Item2.OnDelete -= Delete;

			OnDelete?.Invoke(this);
		}
		#endregion


		#region private methods
		/// <summary>
		/// Оповещение подписчиков об изменении свойств.
		/// </summary>
		/// <param name="propertyName">Имя измененого свойства.</param>
		private void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}
