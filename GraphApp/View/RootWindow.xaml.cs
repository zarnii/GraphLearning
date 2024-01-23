using GraphApp.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GraphApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class RootWindow : Window
	{
		public RootWindow(RootViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;
		}
	}
}
