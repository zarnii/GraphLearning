using GraphApp.Command;
using GraphApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace GraphApp.ViewModel
{
	/// <summary>
	/// Модель представления страницы редактора.
	/// </summary>
	public class VisualEditorViewModel: ViewModel
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
		/// Цвет вершины по умолчанию.
		/// </summary>
		private Color _defaultVertexColor = Colors.Red;

		/// <summary>
		/// Команда изменения режима мыши.
		/// </summary>
		private ICommand _changeMouseMode;

		/// <summary>
		/// Команда добавыления вершины.
		/// </summary>
		private ICommand _clickOnField;

		/// <summary>
		/// Команда нажатия на вершину.
		/// </summary>
		private ICommand _clickOnVertex;

		/// <summary>
		/// Команда нажатия на связь.
		/// </summary>
		private ICommand _clickOnConnection;

		/// <summary>
		/// Команда перемещения вершины.
		/// </summary>
		private ICommand _moveVertex;

		/// <summary>
		/// Режим мыши.
		/// </summary>
		private static MouseMode _mouseMode;
		#endregion

		#region properties
		/// <summary>
		/// Свойство команды изменения режима мыши.
		/// </summary>
		public ICommand ChangeMouseMode
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
		public ICommand ClickOnField
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
		public ICommand ClickOnVertex
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

		/// <summary>
		/// Команда нажатия на связь.
		/// </summary>
		public ICommand ClickOnConnection
		{
			get
			{
				return _clickOnConnection;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value), "Пустая команда нажатия на связь.");
				}

				_clickOnConnection = value;
			}
		}

		/// <summary>
		/// Команда перемещения вершины.
		/// </summary>
		public ICommand MoveVertex
		{
			get
			{
				return _moveVertex;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Пустая команда передмещения верин");
				}

				_moveVertex = value;
			}
		}

		/// <summary>
		/// Выбранные вершины
		/// </summary>
		public List<VisualVertex> SelectedVertices { get; set; }

		/// <summary>
		/// Лист вершин.
		/// </summary>
		public ObservableCollection<VisualVertex> Vertices { get; set; }

		/// <summary>
		/// Лист связей.
		/// </summary>
		public ObservableCollection<VisualConnection> Connections { get; set; }
		#endregion

		#region constructor
		/// <summary>
		/// Конструктор.
		/// </summary>
		public VisualEditorViewModel()
		{
			Vertices = new ObservableCollection<VisualVertex>();
			Connections = new ObservableCollection<VisualConnection>();
			SelectedVertices = new List<VisualVertex>();

			ChangeMouseMode = new RelayCommand(SetMouseMode);
			ClickOnField = new RelayCommand(ClickOnFieldCommand);
			ClickOnVertex = new RelayCommand(ClickOnVertexCommand);
			ClickOnConnection = new RelayCommand(ClickOnConnectionCommand);
			MoveVertex = new RelayCommand(MoveVertexCommand);


			AddVertex(new Point(200, 200));
			AddVertex(new Point(100, 100));
			AddVertex(new Point(100, 200));


			AddConnection((Vertices[0], Vertices[1]));

			AddConnection((Vertices[0], Vertices[2]));

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
		/// <param name="parameter">Аргументы события.</param>
		private void ClickOnFieldCommand(object parameter)
		{
			if (_mouseMode != MouseMode.Create)
			{
				return;
			}

			var mbEventArgs = (MouseButtonEventArgs)parameter;
			var point = mbEventArgs.GetPosition((Rectangle)mbEventArgs.OriginalSource);

			AddVertex(point);
		}

		/// <summary>
		/// Команда нажатия на вершину.
		/// </summary>
		/// <param name="parameter">Нажатая вершина.</param>
		private void ClickOnVertexCommand(object parameter)
		{
			if (_mouseMode == MouseMode.Delete)
			{
				DeleteVertex((VisualVertex)parameter);
			}
			else if (_mouseMode == MouseMode.Connect)
			{
				if (SelectedVertices.Count < 2)
				{
					SelectedVertices.Add((VisualVertex)parameter);
				}

				if (SelectedVertices.Count == 2)
				{
					AddConnection((SelectedVertices[0], SelectedVertices[1]));
					SelectedVertices.Clear();
				}
			}
		}

		/// <summary>
		/// Команда нажатия на связь.
		/// </summary>
		/// <param name="parameter"></param>
		private void ClickOnConnectionCommand(object parameter)
		{
			if (_mouseMode == MouseMode.Delete)
			{
				DeleteConnection((VisualConnection)parameter);
			}
		}

		/// <summary>
		/// Команда добавление новой вершины.
		/// </summary>
		/// <param name="point">Координаты вершины.</param>
		private void AddVertex(Point point)
		{
			Vertices.Add(new VisualVertex(
				(point.X - _defaultVertexWidth / 2, point.Y - _defaultVertexHeight / 2),
				_defaultVertexWidth,
				_defaultVertexHeight,
				Vertices.Count + 1,
				_defaultVertexColor
			));
		}

		/// <summary>
		/// Удаление вершины.
		/// </summary>
		/// <param name="vertex"></param>
		private void DeleteVertex(VisualVertex vertex)
		{
			vertex.Delete();
			Vertices.Remove(vertex);
		}

		/// <summary>
		/// Создание связи.
		/// </summary>
		/// <param name="connectedVertices">Соеденяемые вершины.</param>
		private void AddConnection((VisualVertex, VisualVertex) connectedVertices)
		{
			var connection = new VisualConnection(connectedVertices);
			connection.OnDelete += DeleteConnection;

			Connections.Add(connection);
		}

		/// <summary>
		/// Удаление связи.
		/// </summary>
		/// <param name="connection">Удаляемая связь.</param>
		private void DeleteConnection(VisualConnection connection)
		{
			connection.OnDelete -= DeleteConnection;
			Connections.Remove(connection);
		}

		/// <summary>
		/// Нажатие на поле.
		/// </summary>
		/// <param name="parameter">Аргументы события.</param>
		private void ClickOnRectangle(object parameter)
		{
			var mbEventArgs = (MouseButtonEventArgs)parameter;
			var point = mbEventArgs.GetPosition((Rectangle)mbEventArgs.OriginalSource);
			ClickOnField.Execute(point);
		}

		/// <summary>
		/// Передвижение вершин.
		/// </summary>
		/// <param name="parameter">Аргументы события.</param>
		private void MoveVertexCommand(object parameter)
		{
			if (_mouseMode != MouseMode.Default)
			{
				return;
			}

			var ddEventArgs = (DragDeltaEventArgs)parameter;
			var vertex = (VisualVertex)((FrameworkElement)ddEventArgs.OriginalSource).DataContext;

			vertex.X += ddEventArgs.HorizontalChange;
			vertex.Y += ddEventArgs.VerticalChange;
		}
		#endregion
	}
}
