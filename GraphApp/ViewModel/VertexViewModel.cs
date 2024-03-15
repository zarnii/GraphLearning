using GraphApp.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GraphApp.ViewModel
{
    public class VertexViewModel : ViewModel, INotifyPropertyChanged
    {
        private VisualVertex _visualVertex;

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

        public event PropertyChangedEventHandler? PropertyChanged;

        public VertexViewModel()
        {

        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
