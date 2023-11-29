using GraphApp.ViewModel;
using System.Windows;

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
		}

		// Скорее всего не правильно.
		private void Canvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			((MainWindowViewModel)DataContext).ClickOnCanvas?.Execute(e.GetPosition(this));
		}
	}
}
