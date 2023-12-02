﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GraphApp.Model
{
	/// <summary>
	/// Графическое соединение, отвечающая за отображение связей на поле.
	/// </summary>
	public class VisualConnection: INotifyPropertyChanged
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
				return ConnectedVertices.Item1.X + ConnectedVertices.Item1.Width / 2;
			}
		}

		/// <summary>
		/// Y координата первой веришны.
		/// </summary>
		public double Y1
		{
			get
			{
				return ConnectedVertices.Item1.Y + ConnectedVertices.Item1.Height / 2;
			}
		}

		/// <summary>
		/// X координата второй вершины.
		/// </summary>
		public double X2
		{
			get
			{
				return ConnectedVertices.Item2.X + ConnectedVertices.Item2.Width / 2;
			}
		}

		/// <summary>
		/// Y координата второй вершины.
		/// </summary>
		public double Y2
		{
			get
			{
				return ConnectedVertices.Item2.Y + ConnectedVertices.Item2.Height / 2;
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
			double weight = 0,
			ConnectionType connectionType = ConnectionType.Unidirectional)
		{
			_connection = new Connection((connectedVertices.Item1.Vertex, connectedVertices.Item2.Vertex), weight, connectionType);
			ConnectedVertices = connectedVertices;
			Weight = weight;
			ConnectionType = connectionType;

			ConnectedVertices.Item1.OnDelete += Delete;
			ConnectedVertices.Item2.OnDelete += Delete;
		}
		#endregion

		#region public methods
		public void Delete()
		{
			OnDelete?.Invoke(this);
		}

		public void SetOnDelete(Action<VisualConnection> onDelete)
		{
			OnDelete += onDelete;
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
		#endregion
	}
}
