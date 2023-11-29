using GraphApp.Command;
using GraphApp.Model;
using GraphApp.Strategy;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace GraphApp.ViewModel
{
	/// <summary>
	/// Модель представления главного окна.
	/// </summary>
	class MainWindowViewModel
	{
		#region fields
		/// <summary>
		/// Команда изменения режима мыши.
		/// </summary>
		private RelayCommand _changeMouseMode;


		/// <summary>
		/// Команда нажатия на Canvas.
		/// </summary>
		private RelayCommand _clickOnCanvas;


		/// <summary>
		/// Режим мыши.
		/// </summary>
		private MouseMode _mouseMode { get; set; }


		/// <summary>
		/// Маппинг алгоритмов и режима мыши.
		/// </summary>
		private Dictionary<MouseMode, Algorithm> _algorithmMap = new Dictionary<MouseMode, Algorithm>()
		{
			{ MouseMode.Create, new CreateVertexAlgorithm() }
		};

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
		/// Команда нажатия на на Canvas.
		/// </summary>
		public RelayCommand ClickOnCanvas
		{
			get
			{
				return _clickOnCanvas;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Пустая комманда.");
				}

				_clickOnCanvas = value;
			}
		}


		/// <summary>
		/// Лист связей.
		/// </summary>
		public List<Connection> ConnectionsList { get; set; }


		/// <summary>
		/// Лист вершин.
		/// </summary>
		public List<Vertex> VertexList { get; set; }


		/// <summary>
		/// Обработчик вершин.
		/// </summary>
		public VertexHeandler VertexHeandler { get; set; }


		/// <summary>
		/// Обработчик связей.
		/// </summary>
		public ConnectionHeandler ConnectionHeandler { get; set; }


		public MouseEventArgs MouseInfo { get; set; }
		#endregion


		#region constructor
		/// <summary>
		/// Конструктор.
		/// </summary>
		public MainWindowViewModel()
		{
			VertexList = new List<Vertex>();
			ConnectionsList = new List<Connection>();

			VertexHeandler = new VertexHeandler(VertexList);
			ConnectionHeandler = new ConnectionHeandler(ConnectionsList);

			ChangeMouseMode = new RelayCommand(SetMouseMode);
			ClickOnCanvas = new RelayCommand(ClickOnCanvasCommand);
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
		/// Обработка нажатий на поле Canvas.
		/// </summary>
		/// <param name="parameter">Параметр.</param>
		private void ClickOnCanvasCommand(object parameter)
		{
			_algorithmMap[_mouseMode].Execute(parameter);
		}
		#endregion

	}
}
