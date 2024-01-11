using GraphApp.ViewModel;
using System.Windows;

namespace GraphApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class RootWindow : Window
	{
		public RootWindow()
		{
			InitializeComponent();
			DataContext = new RootViewModel();
		}
	}
}
