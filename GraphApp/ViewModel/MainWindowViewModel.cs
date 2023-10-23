using GraphApp.Command;
using GraphApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

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
		/// Режим мыши.
		/// </summary>
		private MouseMode _mouseMode { get; set; }
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
		/// Создание вершины.
		/// </summary>
		private void CreateVertex()
		{
			SystemSounds.Beep.Play();
		}
		#endregion

	}
}
