using GraphApp.ViewModel;
using System.Windows.Controls;

namespace GraphApp.View
{
	/// <summary>
	/// Логика взаимодействия для MainMenuWindow.xaml
	/// </summary>
	public partial class MainMenuWindow : Page
	{
		public MainMenuWindow(MainMenuViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;
		}
	}
}
