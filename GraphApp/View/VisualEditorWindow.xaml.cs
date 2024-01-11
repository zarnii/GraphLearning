using GraphApp.Model;
using GraphApp.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace GraphApp.View
{
	/// <summary>
	/// Логика взаимодействия для VisualEditorWindow.xaml
	/// </summary>
	public partial class VisualEditorWindow : Page
	{

		public VisualEditorWindow()
		{
			InitializeComponent();
			DataContext = new VisualEditorViewModel();
		}
		private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var point = e.GetPosition((Rectangle)sender);
			((VisualEditorViewModel)DataContext).ClickOnField?.Execute(point);
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
