using GraphApp.ViewModel;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using GraphApp.Model;

namespace GraphApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		public MainWindow()
		{
			InitializeComponent();
			DataContext = new MainWindowViewModel();
			var Vertices = (DataContext as MainWindowViewModel).Vertices;
		}

		private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var point = e.GetPosition((Rectangle)sender);
			((MainWindowViewModel)DataContext).ClickOnField?.Execute(point);
		}

		// Временно.
		private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
		{
			var vertex = (VisualVertex)((FrameworkElement)sender).DataContext;
			vertex.X += e.HorizontalChange;
			vertex.Y += e.VerticalChange;
		}
	}
}
