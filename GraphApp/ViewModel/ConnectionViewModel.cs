using GraphApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.ViewModel
{
    public class ConnectionViewModel : ViewModel, INotifyPropertyChanged
    {
        private VisualConnection _visualConnection;

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

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
