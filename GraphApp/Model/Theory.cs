using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GraphApp.Model
{
    public class Theory
    {
        private UserControl _view;

        public UserControl View
        {
            get 
            { 
                return _view; 
            }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Пустой user control.");
                }

                _view = value;
            }
        }

        public Theory(UserControl view)
        {
            View = view;
        }
    }
}
