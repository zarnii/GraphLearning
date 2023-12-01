using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace GraphApp.Model
{
	/// <summary>
	/// Визуальная вершина, отвечающая за отрисовку на поле Canvas.
	/// </summary>
	public class VisualVertex: INotifyPropertyChanged
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
		/// Ширина.
		/// </summary>
		public int Width { get; set; }

		/// <summary>
		/// Высота.
		/// </summary>
		public int Height { get; set; }

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
		/// Событие изменения свойства.
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;
		#endregion

		#region constructors
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="x">X координата.</param>
		/// <param name="y">Y координата.</param>
		/// <param name="number">Номер вершины.</param>
		/// <param name="color">Цвет.</param>
		/// <param name="name">Название.</param>
		public VisualVertex(double x, double y, int width, int height, int number, Color color, string name = "default")
		{
			_vertex = new Vertex(x, y , number, name);
			Width = width;
			Height = height;
			Color = new SolidColorBrush(color);
		}

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="coordinates">Координаты.</param>
		/// <param name="number">Номер вершины.</param>
		/// <param name="color">Цвет.</param>
		/// <param name="name">Название.</param>
		public VisualVertex((double, double) coordinates, int width, int height, int number, Color color, string name = "default")
		{
			_vertex = new Vertex(coordinates, number, name);
			Width = width;
			Height = height;
			Color = new SolidColorBrush(color);
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
