using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GraphApp.Model
{
	/// <summary>
	/// Вершина графа.
	/// </summary>
	public class Vertex : INotifyPropertyChanged
	{
		#region field
		/// <summary>
		/// X координата вершины.
		/// </summary>
		private int _x;

		/// <summary>
		/// Y координата вершины.
		/// </summary>
		private int _y;

		/// <summary>
		/// Название вершины.
		/// </summary>
		private string _name;

		/// <summary>
		/// Номер вершины на поле.
		/// </summary>
		private int _number;

		/// <summary>
		/// Связи вершины.
		/// </summary>
		private List<Vertex> _connections = new List<Vertex>();

		/// <summary>
		/// Оповещение подписчиков о необходимости удаления вершины из связей.
		/// </summary>
		public delegate void NotifyConnection(Vertex vertex);

		/// <summary>
		/// Событие удаления вершины.
		/// </summary>
		public event NotifyConnection OnDelete;

		/// <summary>
		/// Событие изменения свойства.
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;
		#endregion

		#region properties
		/// <summary>
		/// Координаты вершины.
		/// </summary>
		public (int, int) Сoordinates
		{
			get
			{
				return (_x, _y);
			}
			set
			{
				_x = value.Item1;
				_y = value.Item2;
			}
		}

		/// <summary>
		/// Имя вершины.
		/// </summary>
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				if (String.IsNullOrWhiteSpace(value))
				{
					throw new ArgumentNullException(nameof(value), "Пустое имя вершины");
				}

				_name = value;
			}
		}

		/// <summary>
		/// Номер вершины на поле.
		/// </summary>
		public int Number
		{
			get
			{
				return _number;
			}
			set
			{
				_number = value;
			}
		}

		/// <summary>
		/// Связи вершины.
		/// </summary>
		public List<Vertex> Connections
		{
			get
			{
				return _connections;
			}
			private set
			{
				_connections = value;
			}
		}
		#endregion

		#region constructors
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="x">Х координата вершины.</param>
		/// <param name="y">Y координата вершины.</param>
		/// <param name="number">Номер вершины на поле.</param>
		/// <param name="name">Имя вершины.</param>
		public Vertex(int x, int y, int number, string name = "default")
		{
			_x = x;
			_y = y;
			_number = number;
			_name = name;
		}

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="coordinates">Кортеж координат x,y.</param>
		/// <param name="number">Номер вершины на поле.</param>
		/// <param name="name">Имя вершины.</param>
		public Vertex((int, int) coordinates, int number, string name = "default")
			: this(coordinates.Item1, coordinates.Item2, number, name) { }
		#endregion

		#region public methods
		/// <summary>
		/// Добавбление вершины.
		/// </summary>
		/// <param name="vertex">Вершина.</param>
		/// <exception cref="ArgumentNullException">Добавляемая вершина является null</exception>
		public void AddConnection(Vertex vertex)
		{
			if (vertex == null)
			{
				throw new ArgumentNullException(nameof(vertex), "Добавляемая вершина является null");
			}

			vertex.OnDelete += DeleteConnection;
			_connections.Add(vertex);
		}

		/// <summary>
		/// Удаление вершины.
		/// </summary>/
		public void Delete()
		{
			OnDelete?.Invoke(this);
		}
		#endregion

		#region private methods
		/// <summary>
		/// Удаление вершины из списка связей.
		/// </summary>
		/// <param name="vertex">Цдаляемая вершина.</param>
		private void DeleteConnection(Vertex vertex)
		{
			_connections.Remove(vertex);
		}

		/// <summary>
		/// Оповещение подписчиков о изменении свойства.
		/// </summary>
		/// <param name="propertyName"></param>
		private void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}
