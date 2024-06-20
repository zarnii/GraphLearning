using GraphApp.Command;
using GraphApp.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace GraphApp.ViewModel
{
    /// <summary>
    /// Модель представления параметров вершины.
    /// </summary>
    public class VertexViewModel : ViewModel, INotifyPropertyChanged
    {
        /// <summary>
        /// Вершина.
        /// </summary>
        private VisualVertex _visualVertex;

        private LimitedStack<ICommand> _undoCommandStack;

        /// <summary>
        /// Проверка ввода координат.
        /// </summary>
        public ICommand CheckCoordinates { get; private set; }

        /// <summary>
        /// Вершина.
        /// </summary>
        public VisualVertex VisualVertex
        {
            get
            {
                return _visualVertex;
            }
            set
            {
                _visualVertex = value;
                OnPropertyChanged(nameof(VisualVertex));
            }
        }

        /// <summary>
        /// Максимальное значение X координаты.
        /// </summary>
        public int MaxXCoordinate { get; set; }

        /// <summary>
        /// Максимальное значеник Y координаты.
        /// </summary>
        public int MaxYCoordinate { get; set; }

        /// <summary>
        /// Событие изменения свойства.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public VertexViewModel(LimitedStack<ICommand> undoCommandStack)
        {
            CheckCoordinates = new RelayCommand(CheckCoordinatesCommand);
        }
        
        /// <summary>
        /// Оповещение подписчиков о изменении свойства.
        /// </summary>
        /// <param name="propertyName"></param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Команда проверки коориднат.
        /// </summary>
        /// <param name="parameter"></param>
        private void CheckCoordinatesCommand(object parameter)
        {
            if (_visualVertex.X + _visualVertex.Radius > MaxXCoordinate)
            {
                _visualVertex.X = MaxXCoordinate - _visualVertex.Radius;
            }

            if (_visualVertex.Y + _visualVertex.Radius > MaxYCoordinate)
            {
                _visualVertex.Y = MaxYCoordinate - _visualVertex.Radius;
            }
        }
    }
}
