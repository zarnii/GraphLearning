using GraphApp.Command;
using GraphApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows;

namespace GraphApp.ViewModel
{
	/// <summary>
	/// Модель представления страницы редактора.
	/// </summary>
	public class VisualEditorViewModel
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
		/// Команда нажатия на связь.
		/// </summary>
		private RelayCommand _clickOnConnection;

		/// <summary>
		/// Команда перемещения вершины.
		/// </summary>
		private RelayCommand _moveVertex;

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

		/// <summary>
		/// Команда нажатия на связь.
		/// </summary>
		public RelayCommand ClickOnConnection
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
		public RelayCommand MoveVertex
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
			//MoveVertex = new RelayCommand(MoveVertexCommand); to-do


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
		/// <param name="parameter">Параметр.</param>
		private void ClickOnFieldCommand(object parameter)
		{
			if (_mouseMode == MouseMode.Create)
			{
				AddVertex((Point)parameter);
			}
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
		#endregion
	}
}
