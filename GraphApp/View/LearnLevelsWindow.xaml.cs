using GraphApp.ViewModel;
using System.Windows.Controls;

namespace GraphApp.View
{
	/// <summary>
	/// Логика взаимодействия для LearnLevelsWindow.xaml
	/// </summary>
	public partial class LearnLevelsWindow : Page
	{
		public LearnLevelsWindow(LearnLevelsViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;
		}
	}
}
