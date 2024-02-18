using GraphApp.Command;
using GraphApp.View;
using GraphApp.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace GraphApp.ViewModel
{
	class TestViewModel : ViewModel, INotifyPropertyChanged
	{
		private double _x;
		private double _y;
		public event PropertyChangedEventHandler? PropertyChanged;

		public double X
		{
			get
			{
				return _x;
			}
			set
			{
				_x = value;
				OnPropertyChanged();
			}
		}

		public double Y
		{
			get
			{
				return _y;
			}
			set
			{
				_y = value;
				OnPropertyChanged();
			}
		}

		public int CanvasHeight { get; set; }
		public int CanvasWidth { get; set; }
		public ICommand MouseScale { get; set; }
		public ObservableCollection<Vertex> Shapes { get; set; }

		public TestViewModel()
		{
			CanvasHeight = 200;
			CanvasWidth = 200;

			Shapes = new ObservableCollection<Vertex>()
			{
				new Vertex()
				{
					X = 10,
					Y = 20,
				},
				new Vertex()
				{
					X = 30,
					Y = 20,
				},
				new Vertex()
				{
					X = 50,
					Y = 20,
				}
			};

			MouseScale = new RelayCommand(MouseScaleCommand);
			X = 0.1;
			Y = 0.1;

		}

		private void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void MouseScaleCommand(object parameter)
		{
			double zoom = ((MouseWheelEventArgs)parameter).Delta > 0
				? 0.1
				: -0.1;

			if (X + zoom < 0.1 || Y + zoom < 0.1)
			{
				return;
			}

			X += zoom;
			Y += zoom;
		}
	}
}
