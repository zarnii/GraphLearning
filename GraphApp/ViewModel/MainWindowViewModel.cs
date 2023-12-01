using GraphApp.Command;
using GraphApp.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace GraphApp.ViewModel
{
	/// <summary>
	/// Модель представления главного окна.
	/// </summary>
	public class MainWindowViewModel
	{
		#region fields
		/// <summary>
		/// Ширина вершины по умолчанию.
		/// </summary>
		private int _defaultVertexWidth = 20;

		/// <summary>
		/// Высота вершины по умолчанию.
		/// </summary>
		private int _defaultVertexHeight = 20;

		/// <summary>
		/// Команда изменения режима мыши.
		/// </summary>
		private RelayCommand _changeMouseMode;

		/// <summary>
		/// Команда добавыления вершины.
		/// </summary>
		private RelayCommand _clickOnField;

		/// <summary>
		/// Команда нажатия на вершину.
		/// </summary>
		private RelayCommand _clickOnVertex;

		/// <summary>
		/// Режим мыши.
		/// </summary>
		private static MouseMode _mouseMode;
		#endregion


		#region properties
		/// <summary>
		/// Свойство команды изменения режима мыши.
		/// </summary>
		public RelayCommand ChangeMouseMode
		{
			get
			{
				return _changeMouseMode;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value), "Команда является пустой.");
				}

				_changeMouseMode = value;
			}
		}

		/// <summary>
		/// Команда добавления вершины.
		/// </summary>
		public RelayCommand ClickOnField
		{
			get
			{
				return _clickOnField;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value), "Пустая команда нажатия на поле.");
				}

				_clickOnField = value;
			}
		}

		/// <summary>
		/// Команда нажатия на вершину.
		/// </summary>
		public RelayCommand ClickOnVertex
		{
			get
			{
				return _clickOnVertex;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value), "Пустая команда нажатия на вершину.");
				}

				_clickOnVertex = value;
			}
		}

		public VisualVertex SelectedVertex { get; set; }

		/// <summary>
		/// Лист вершин.
		/// </summary>
		public ObservableCollection<VisualVertex> Vertices { get; set; }
		#endregion

		#region constructor
		/// <summary>
		/// Конструктор.
		/// </summary>
		public MainWindowViewModel()
		{
			Vertices = new ObservableCollection<VisualVertex>();

			ChangeMouseMode = new RelayCommand(SetMouseMode);
			ClickOnField = new RelayCommand(ClickOnFieldCommand);
			ClickOnVertex = new RelayCommand(ClickOnVertexCommand);
		}
		#endregion


		#region public methods
		#endregion


		#region private methods
		/// <summary>
		/// Изменение режима мыши.
		/// </summary>
		/// <param name="mode">Режим мыши.</param>
		private void SetMouseMode(object mode)
		{
			_mouseMode = (MouseMode)mode;
		}

		/// <summary>
		/// Команда нажатия на поле.
		/// </summary>
		/// <param name="parameter">Параметр.</param>
		private void ClickOnFieldCommand(object parameter)
		{
			if (_mouseMode == MouseMode.Create)
			{
				AddVertexCommand((Point)parameter);
			}
		}

		/// <summary>
		/// Команда нажатия на вершину.
		/// </summary>
		/// <param name="parameter"></param>
		private void ClickOnVertexCommand(object parameter)
		{
			if (_mouseMode == MouseMode.Delete)
			{
				Vertices.Remove((VisualVertex)parameter);
			}
		}

		/// <summary>
		/// Команда добавление новой вершины.
		/// </summary>
		/// <param name="parameter">Координаты верпшины.</param>
		private void AddVertexCommand(Point point)
		{
			Vertices.Add(new VisualVertex(
				(point.X - _defaultVertexWidth / 2, point.Y - _defaultVertexHeight / 2),
				_defaultVertexWidth,
				_defaultVertexHeight,
				Vertices.Count + 1,
				Colors.Black
			));
		}
		#endregion

	}
}
