using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GraphApp.Model
{
	/// <summary>
	/// Графическое соединение, отвечающая за отображение связей на поле.
	/// </summary>
	public class VisualConnection : GraphElement, INotifyPropertyChanged, IComparable<VisualConnection>
	{
		#region fields
		/// <summary>
		/// Связь.
		/// </summary>
		private Connection _connection;

		/// <summary>
		/// Соединенные вершины.
		/// </summary>
		private (VisualVertex, VisualVertex) _connectedVertices;

		/// <summary>
		/// Толщина. линии.
		/// </summary>
		private int _thickness;
        #endregion

        #region properties
        /// <summary>
        /// Весь связи.
        /// </summary>
        public double Weight
		{
			get
			{
				return _connection.Weight;
			}
			set
			{
				_connection.Weight = value;
				OnPropertyChanged(nameof(Weight));
			}
		}

		/// <summary>
		/// Номер связи.
		/// </summary>
		public int Number
		{
			get
			{
				return _connection.Number;
			}
			set
			{
				_connection.Number = value;
				OnPropertyChanged(nameof(Number));
			}
		}

		/// <summary>
		/// Связь.
		/// </summary>
		public Connection Connection
		{
			get
			{
				return _connection;
			}
		}

		/// <summary>
		/// Соединенные вершины.
		/// </summary>
		public (VisualVertex, VisualVertex) ConnectedVertices
		{
			get
			{
				return _connectedVertices;
			}
			private set
			{
				if (value.Item1 == null)
				{
					throw new ArgumentNullException(nameof(value.Item1), "Пустая вершина для создания связи.");
				}

				if (value.Item2 == null)
				{
					throw new ArgumentNullException(nameof(value.Item2), "Пустая вершина для создания связи.");
				}

				_connectedVertices = value;
			}
		}

		/// <summary>
		/// Первая соедененная вершина.
		/// </summary>
		public VisualVertex FirstConnectedVertex
		{
			get
			{
				return _connectedVertices.Item1;
			}
		}

		/// <summary>
		/// Вторая соеденненая вершина.
		/// </summary>
		public VisualVertex SecondConnectedVertex
		{
			get
			{
				return _connectedVertices.Item2;
			}
		}

		/// <summary>
		/// Толщина линии.
		/// </summary>
		public int Thickness 
		{
			get
			{
				return _thickness;
			}
			set
			{
				_thickness = value;
				OnPropertyChanged(nameof(Thickness));
			}
		}

        /// <summary>
        /// Ссылка на выполняемую функцию при удалении. 
        /// </summary>
        public Action<VisualConnection> OnDelete { get; set; }

		/// <summary>
		/// Тип связи.
		/// </summary>
		public ConnectionType ConnectionType
		{
			get
			{
				return _connection.ConnectionType;
			}
			set
			{
				_connection.ConnectionType = value;
			}
		}


		/// <summary>
		/// X координата первой вершины.
		/// </summary>
		public double X1
		{
			get
			{
				return ConnectedVertices.Item1.X + ConnectedVertices.Item1.Radius;
			}
		}

		/// <summary>
		/// Y координата первой веришны.
		/// </summary>
		public double Y1
		{
			get
			{
				return ConnectedVertices.Item1.Y + ConnectedVertices.Item1.Radius;
			}
		}

		/// <summary>
		/// X координата второй вершины.
		/// </summary>
		public double X2
		{
			get
			{
				return ConnectedVertices.Item2.X + ConnectedVertices.Item2.Radius;
			}
		}

		/// <summary>
		/// Y координата второй вершины.
		/// </summary>
		public double Y2
		{
			get
			{
				return ConnectedVertices.Item2.Y + ConnectedVertices.Item2.Radius;
			}
		}

		/// <summary>
		/// Событие изменения свойств.
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;
		#endregion

		#region constructor
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="connectedVertices">Соединенные вершины.</param>
		/// <param name="weight">Вес связи.</param>
		/// <param name="connectionType">Тип связи.</param>
		public VisualConnection((VisualVertex, VisualVertex) connectedVertices,
			int number,
			int thickness,
            double weight = 0,
			ConnectionType connectionType = ConnectionType.Unidirectional)
		{
			if (connectedVertices.Item1 == null || connectedVertices.Item2 == null)
			{
				throw new ArgumentNullException(nameof(connectedVertices), "Пустая соеденяемая вершина.");
			}

			if (thickness < 1)
			{
				throw new ArgumentNullException(nameof(thickness), "Толщина не может быть меньше 1.");
			}

			_connection = new Connection(
				(connectedVertices.Item1.Vertex, connectedVertices.Item2.Vertex),
				number,
				weight,
				connectionType
			);

			ConnectedVertices = connectedVertices;
			Weight = weight;
			ConnectionType = connectionType;
			Thickness = thickness;

			ConnectedVertices.Item1.OnDelete += Delete;
			ConnectedVertices.Item2.OnDelete += Delete;

			ConnectedVertices.Item1.PropertyChanged += UpdateVertexCoords;
			ConnectedVertices.Item2.PropertyChanged += UpdateVertexCoords;
		}
		#endregion

		#region public methods
		/// <summary>
		/// Оповещение подписчиков о удалении связи.
		/// </summary>
		public void Delete()
		{
			ConnectedVertices.Item1.OnDelete -= Delete;
			ConnectedVertices.Item2.OnDelete -= Delete;

			ConnectedVertices.Item1.PropertyChanged -= UpdateVertexCoords;
			ConnectedVertices.Item2.PropertyChanged -= UpdateVertexCoords;

			OnDelete?.Invoke(this);
		}

        /// <summary>
        /// Реализация IComparable !!! Переделать.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(VisualConnection? other)
        {
			return Number.CompareTo(other?.Number);
        }
        #endregion

        #region private methods
        /// <summary>
        /// Оповещение подписчиков об изменении свойств.
        /// </summary>
        /// <param name="propertyName"></param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>
		/// Обновление свойства координат вершин.
		/// </summary>
		/// <param name="sender">Отправитель.</param>
		/// <param name="e">Аргументы.</param>
		private void UpdateVertexCoords(object sender, PropertyChangedEventArgs e)
		{
			if (sender == ConnectedVertices.Item1)
			{
				if (e.PropertyName == nameof(ConnectedVertices.Item1.Vertex.X))
				{
					OnPropertyChanged(nameof(X1));
				}
				else if (e.PropertyName == nameof(ConnectedVertices.Item1.Vertex.Y))
				{
					OnPropertyChanged(nameof(Y1));
				}
			}
			else if (sender == ConnectedVertices.Item2)
			{
				if (e.PropertyName == nameof(ConnectedVertices.Item2.Vertex.X))
				{
					OnPropertyChanged(nameof(X2));
				}
				else if (e.PropertyName == nameof(ConnectedVertices.Item2.Vertex.Y))
				{
					OnPropertyChanged(nameof(Y2));
				}
			}
		}
        #endregion
    }
}
