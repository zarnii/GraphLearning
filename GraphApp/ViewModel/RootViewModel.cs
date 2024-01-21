using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using GraphApp.Command;
using GraphApp.Interfaces;
using GraphApp.View;

namespace GraphApp.ViewModel
{
	/// <summary>
	/// Модель представления главного окна.
	/// </summary>
	public class RootViewModel
	{
		#region fields
		/// <summary>
		/// Сервис навигации.
		/// </summary>
		private INavigationService _navigationService;
		#endregion

		#region properties
		/// <summary>
		/// Сервис навигации.
		/// </summary>
		public INavigationService NavigationService
		{
			get 
			{ 
				return _navigationService; 
			}
		}
		#endregion

		#region constructor
		/// <summary>
		/// Конструктор.
		/// </summary>
		public RootViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
			_navigationService.NavigateTo<MainMenuWindow>();
		}
		#endregion

		#region public methods
		#endregion

		#region private metgods
		#endregion
	}
}
