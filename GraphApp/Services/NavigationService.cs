using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using GraphApp.Interfaces;

namespace GraphApp.Services
{
	/// <summary>
	/// Сервис навигации.
	/// </summary>
    public class NavigationService : INavigationService
	{
		#region fields
		/// <summary>
		/// Фабрика страниц.
		/// </summary>
		private Func<Type, Page> _pageFactory;

		/// <summary>
		/// Текущая страница.
		/// </summary>
		private Page _currentPage;

		/// <summary>
		/// Изменение свойства.
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;
		#endregion

		#region properties
		/// <summary>
		/// Текущая страница.
		/// </summary>
		public Page CurrentPage
		{
			get
			{
				return _currentPage;
			}
			private set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}

				_currentPage = value;
				OnPropertyChanged();
			}
		}
		#endregion

		#region constructor
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="pageFactory">Фабрика страниц.</param>
		public NavigationService(Func<Type, Page> pageFactory)
		{
			_pageFactory = pageFactory;
		}
		#endregion

		#region public methods
		/// <summary>
		/// Навигация.
		/// </summary>
		/// <typeparam name="Page">Страница, на которую нужно произвести навигацию.</typeparam>
		public void NavigateTo<Page>()
			where Page : System.Windows.Controls.Page
		{
			CurrentPage = _pageFactory.Invoke(typeof(Page));
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
