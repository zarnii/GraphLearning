using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace GraphApp.Model
{
	/*
	 ВНИМАНИЕ! Если добовляешь поле, то меняй метод преобразования в SerializableVertex.
	 */

	/// <summary>
	/// Визуальная вершина, отвечающая за отрисовку на поле Canvas.
	/// </summary>
	public class VisualVertex : GraphElement, INotifyPropertyChanged, IComparable<VisualVertex>
	{
		#region fields
		/// <summary>
		/// Вершина.
		/// </summary>
		private Vertex _vertex;

		/// <summary>
		/// Цвет.
		/// </summary>
		private Brush _color;

		/// <summary>
		/// Радиус.
		/// </summary>
		private int _radius;

		private const int MinimalRadius = 5;
		#endregion

		#region properties
		/// <summary>
		/// Вершина.
		/// </summary>
		public Vertex Vertex
		{
			get
			{
				return _vertex;
			}
			private set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value), "Пустая вершина.");
				}

				_vertex = value;
			}
		}

		/// <summary>
		/// Радиус.
		/// </summary>
		public int Radius 
		{
			get
			{
				return _radius;
			}
			set
			{
				if (value < MinimalRadius)
				{
					throw new ArgumentOutOfRangeException($"Минимальный размер - {MinimalRadius}");
				}

				_radius = value;
				OnPropertyChanged(nameof(Radius));
				OnPropertyChanged(nameof(X));
				OnPropertyChanged(nameof(Y));
			}
		}

		/// <summary>
		/// Коориднаты.
		/// </summary>
		public (double, double) Coordinates
		{
			get
			{
				return _vertex.Сoordinates;
			}
			set
			{
				_vertex.Сoordinates = value;
				OnPropertyChanged(nameof(Coordinates));
			}
		}

		/// <summary>
		/// X координата (нужно для отображения на Canvas).
		/// </summary>
		public double X
		{
			get
			{
				return _vertex.Сoordinates.Item1;
			}
			set
			{
				_vertex.X = value;
				OnPropertyChanged(nameof(X));
			}
		}

		/// <summary>
		/// Y координата (нужно для отображения на Canvas).
		/// </summary>
		public double Y
		{
			get
			{
				return _vertex.Сoordinates.Item2;
			}
			set
			{
				_vertex.Y = value;
				OnPropertyChanged(nameof(Y));
			}
		}

		/// <summary>
		/// Номер.
		/// </summary>
		public int Number
		{
			get
			{
				return _vertex.Number;
			}
			set
			{
				_vertex.Number = value;
				OnPropertyChanged(nameof(Number));
			}
		}

		/// <summary>
		/// Название.
		/// </summary>
		public string Name
		{
			get
			{
				return _vertex.Name;
			}
			set
			{
				_vertex.Name = value;
				OnPropertyChanged(nameof(Name));
			}
		}

		/// <summary>
		/// Цвет.
		/// </summary>
		public Brush Color
		{
			get
			{
				return _color;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value), "Пустой цвет.");
				}

				_color = value;
				OnPropertyChanged(nameof(Color));
			}
		}

		/// <summary>
		/// При удалении.
		/// </summary>
		public Action OnDelete { get; set; }


		/// <summary>
		/// Событие изменения свойства.
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;
		#endregion

		#region constructors
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="x">Х координата.</param>
		/// <param name="y">Y координата.</param>
		/// <param name="radius">Радиус.</param>
		/// <param name="number">Номер.</param>
		/// <param name="color">Цвет.</param>
		/// <param name="name">Имя.</param>
		public VisualVertex(double x, double y, int radius, int number, Color color, string name = "default")
			: this ((x, y), radius, number, color, name)
		{ }

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="coordinates">Координаты.</param>
		/// <param name="radius">Радиус.</param>
		/// <param name="number">Номер.</param>
		/// <param name="color">Цвет.</param>
		/// <param name="name">Имя.</param>
		public VisualVertex((double, double) coordinates, int radius, int number, Color color, string name = "default")
		{
			_vertex = new Vertex(coordinates, number, name);
			Radius = radius;
			Color = new SolidColorBrush(color);
		}

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="vertex">Вершина.</param>
        /// <param name="radius">Радиус.</param>
        /// <param name="color">Цвет.</param>
        public VisualVertex(Vertex vertex, int radius, Color color)
		{
			Vertex = vertex;
			Radius = radius;
			Color = new SolidColorBrush(color);
        }

        /// <summary>
        /// Реализация IComparable.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(VisualVertex? other)
		{
			return Name.CompareTo(other?.Name);
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

		#region public methods
		/// <summary>
		/// Оповещение подписчиков об удалении вершины.
		/// </summary>
		public void Delete()
		{
			OnDelete?.Invoke();
		}
        #endregion
    }
}
