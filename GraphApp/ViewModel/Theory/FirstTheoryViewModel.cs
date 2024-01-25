using GraphApp.Command;
using GraphApp.Interfaces;
using System;
using System.Windows.Input;

namespace GraphApp.ViewModel
{
	public class FirstTheoryViewModel : ViewModel
	{
		#region fields
		private INavigationService _navigationService;

		private ICommand _goBack;
		#endregion

		#region properties
		public ICommand GoBack
		{
			get
			{
				return _goBack;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value), "Пустая команда перехода назад.");
				}

				_goBack = value;
			}
		}
		#endregion

		#region constructor
		public FirstTheoryViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
			GoBack = new RelayCommand(GoBackCommand);
		}
		#endregion

		#region public methods
		#endregion

		#region private methods
		private void GoBackCommand(object parameter)
		{
			_navigationService.NavigateTo(Parent.GetType(), null);
		}
		#endregion
	}
}
