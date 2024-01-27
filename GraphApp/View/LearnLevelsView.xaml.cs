using System;
using System.Collections.Generic;
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
	/// Логика взаимодействия для LearnLevelsView.xaml
	/// </summary>
	public partial class LearnLevelsView : UserControl
	{
		public LearnLevelsView()
		{
			InitializeComponent();
		}

		private void LeftButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			Scroll.LineUp();
		}

		private void RightButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			Scroll.LineDown();
		}
	}
}
