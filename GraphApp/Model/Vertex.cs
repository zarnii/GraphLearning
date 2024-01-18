using System;
using System.Text.Json.Serialization;

namespace GraphApp.Model
{
	/// <summary>
	/// Вершина графа.
	/// </summary>
	public class Vertex
	{
		#region field
		/// <summary>
		/// X координата вершины.
		/// </summary>
		private double _x;

		/// <summary>
		/// Y координата вершины.
		/// </summary>
		private double _y;

		/// <summary>
		/// Название вершины.
		/// </summary>
		private string _name;

		/// <summary>
		/// Номер вершины на поле.
		/// </summary>
		private int _number;

		/// <summary>
		/// Обработчик удаления вершины.
		/// </summary>
		private Action<Vertex> _onDelete;
		#endregion

		#region properties
		/// <summary>
		/// Координаты вершины.
		/// </summary>
		public (double, double) Сoordinates
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

		public double X
		{
			get
			{
				return _x;
			}
			set
			{
				_x = value;
			}
		}

		public double Y
		{
			get
			{
				return _y;
			}
			set
			{
				_y = value;
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
		#endregion

		#region constructors
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="x">Х координата вершины.</param>
		/// <param name="y">Y координата вершины.</param>
		/// <param name="number">Номер вершины на поле.</param>
		/// <param name="name">Имя вершины.</param>
		public Vertex(double x, double y, int number, string name = "default")
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
		public Vertex((double, double) coordinates, int number, string name = "default")
			: this(coordinates.Item1, coordinates.Item2, number, name) { }

		public Vertex()
		{ }
		#endregion

		#region public methods
		#endregion

		#region private methods
		#endregion
	}
}
