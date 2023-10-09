using GraphApp.Command;
using GraphApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.ViewModel
{
    /// <summary>
    /// Модель представления главного окна.
    /// </summary>
    class MainWindowViewModel
    {
        /// <summary>
        /// Команда изменения режима мыши.
        /// </summary>
        private RelayCommand _changeMouseMode;

		/// <summary>
		/// Режим мыши.
		/// </summary>
		private MouseMode _mouseMode { get; set; }

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
        /// Конструктор.
        /// </summary>
        public MainWindowViewModel()
        {
            ChangeMouseMode = new RelayCommand(SetMouseMode);
        }

        /// <summary>
        /// Изменение режима мыши.
        /// </summary>
        /// <param name="mode">Режим мыши.</param>
        private void SetMouseMode(object mode) 
        {
			_mouseMode = (MouseMode)mode;
        }

	}
}
