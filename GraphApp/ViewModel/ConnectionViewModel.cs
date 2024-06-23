using GraphApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GraphApp.ViewModel
{
    /// <summary>
    /// Модель представления параметров графического соединения.
    /// </summary>
    public class ConnectionViewModel : ViewModel, INotifyPropertyChanged
    {
        /// <summary>
        /// Соединение.
        /// </summary>
        private VisualConnection _visualConnection;

        /// <summary>
        /// Типы соединения.
        /// </summary>
        public ConnectionType[] ConnectionTypes { get; private set; }

        /// <summary>
        /// Соединение.
        /// </summary>
        public VisualConnection VisualConnection
        {
            get
            {
                return _visualConnection;
            }
            set
            {
                _visualConnection = value;
                OnPropertyChanged(nameof(VisualConnection));
            }
        }

        /// <summary>
        /// Событие изменения свойства.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ConnectionViewModel()
        { 
            ConnectionTypes = new ConnectionType[3] 
            { 
                ConnectionType.NonDirectional, 
                ConnectionType.Bidirectional, 
                ConnectionType.Unidirectional
            };
        }

        /// <summary>
        /// Оповещение подписчиков о изменении свойства.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
