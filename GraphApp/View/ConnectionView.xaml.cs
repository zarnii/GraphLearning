using System.Windows.Controls;
using System.Windows.Input;

namespace GraphApp.View
{
    /// <summary>
    /// Логика взаимодействия для ConnectionView.xaml
    /// </summary>
    public partial class ConnectionView : UserControl
    {
        public ConnectionView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Проверка введенного символа на то, что он цифра.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckInputSymbolIsDigit(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }
    }
}
