using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphApp.View
{
    /// <summary>
    /// Логика взаимодействия для TestVEView.xaml
    /// </summary>
    public partial class VisualEditorView : UserControl
    {
        public VisualEditorView()
        {
            InitializeComponent();

            ButtonCursor.PreviewMouseUp += Button_MouseLeftButtonDown;
            ButtonCreate.PreviewMouseUp += Button_MouseLeftButtonDown;
            ButtonConnect.PreviewMouseUp += Button_MouseLeftButtonDown;
            ButtonDelete.PreviewMouseUp += Button_MouseLeftButtonDown;
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var defaultStyle = (Style)FindResource("ButtonStyle");
            var selectedStyle = (Style)FindResource("ButtonStyleSelected");

            ButtonCursor.Style = defaultStyle;
            ButtonCreate.Style = defaultStyle;
            ButtonConnect.Style = defaultStyle;
            ButtonDelete.Style = defaultStyle;

            ((Button)sender).Style = selectedStyle;
        }
    }
}
