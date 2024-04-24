using System;
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
		/// Обработчик удаления вершины.
		/// </summary>
		private Action<Vertex> _onDelete;

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
		/// Обработчик удаления вершины.
		/// </summary>
		public Action<Vertex> OnDelete
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
		/// Удаление вершины.
		/// </summary>/
		public void Delete()
		{
			_onDelete?.Invoke(this);
		}
		#endregion

		#region private methods
		/// <summary>
		/// Оповещение подписчиков о изменении свойства.
		/// </summary>
		/// <param name="propertyName">Имя измененного свойства</param>
		private void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}
