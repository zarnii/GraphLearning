using System;
using System.Windows.Controls;
using System.Windows.Input;
using GraphApp.Command;
using GraphApp.Interfaces;

namespace GraphApp.ViewModel
{
	public class TheoryViewModel : ViewModel
	{
		#region fields
		private INavigationService _navigationService;

		private ITheoryService _userControlService;

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

		public UserControl TheoryView
		{
			get
			{
				return _userControlService.CurrentTheory.View;
			}
		}
		#endregion

		#region constructor
		public TheoryViewModel(INavigationService navigationService, ITheoryService userControlService)
		{
			_navigationService = navigationService;
			_userControlService = userControlService;
			GoBack = new RelayCommand(GoBackCommand);
		}
		#endregion

		#region public methods
		#endregion

		#region private methods
		private void GoBackCommand(object parameter)
		{
			_navigationService.NavigateTo<LearnLevelsViewModel>(null);
		}
		#endregion
	}
}
