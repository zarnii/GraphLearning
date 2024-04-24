using GraphApp.Interfaces;

namespace GraphApp.ViewModel
{
	/// <summary>
	/// Модель представления главного окна.
	/// </summary>
	public class RootViewModel : ViewModel
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
			_navigationService.NavigateTo<MainMenuViewModel>();
		}
		#endregion

		#region public methods
		#endregion

		#region private metgods
		#endregion
	}
}
