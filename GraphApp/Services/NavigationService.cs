using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using GraphApp.Interfaces;
using GraphApp.ViewModel;

namespace GraphApp.Services
{
	/// <summary>
	/// Сервис навигации.
	/// </summary>
    public class NavigationService : INavigationService
	{
		#region fields
		/// <summary>
		/// Фабрика vm.
		/// </summary>
		private Func<Type, ViewModel.ViewModel> _viewModelFactory;

		/// <summary>
		/// Текущая vm.
		/// </summary>
		private ViewModel.ViewModel _currentView;

		/// <summary>
		/// Изменение свойства.
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;
		#endregion

		#region properties
		/// <summary>
		/// Текущая vm.
		/// </summary>
		public ViewModel.ViewModel CurrentView
		{
			get
			{
				return _currentView;
			}
			private set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}

				_currentView = value;
				OnPropertyChanged();
			}
		}
		#endregion

		#region constructor
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="viewModelFactory">Фабрика vm.</param>
		public NavigationService(Func<Type, ViewModel.ViewModel> viewModelFactory)
		{
			_viewModelFactory = viewModelFactory;
		}
		#endregion

		#region public methods
		/// <summary>
		/// Навигация.
		/// </summary>
		/// <typeparam name="TViewModel">ViewModel, на которую нужно произвести навигацию</typeparam>
		public void NavigateTo<TViewModel>(ViewModel.ViewModel parentViewModel)
			where TViewModel : ViewModel.ViewModel
		{
			var view = _viewModelFactory.Invoke(typeof(TViewModel));
			view.Parent = parentViewModel;

			CurrentView = view;
			
		}


		/// <summary>
		/// Навигация.
		/// </summary>
		/// <param name="viewModelType">ViewModel, на которую нужно произвести навигацию.</param>
		public void NavigateTo(Type viewModelType, ViewModel.ViewModel parentViewModel)
		{
			var view = _viewModelFactory.Invoke(viewModelType);
			view.Parent = parentViewModel;

			CurrentView = view;
		}
		#endregion

		#region private methods
		/// <summary>
		/// Оповещение подписчиков об изменении свойства.
		/// </summary>
		/// <param name="propertyName"></param>
		private void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}
