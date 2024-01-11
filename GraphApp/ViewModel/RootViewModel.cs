using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using GraphApp.Command;
using GraphApp.View;

namespace GraphApp.ViewModel
{
	/// <summary>
	/// Модель представления главного окна.
	/// </summary>
	public class RootViewModel: INotifyPropertyChanged
	{
		#region fields
		/// <summary>
		/// Страница графического редактора.
		/// </summary>
		private Page _visualEditor;

		/// <summary>
		/// Страница главного меню.
		/// </summary>
		private Page _mainMenu;

		/// <summary>
		/// Текущая страница.
		/// </summary>
		private Page _currentPage;

		/// <summary>
		/// Оповещение подписчиков об изменении свойства.
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
			set
			{
				_currentPage = value;
				
				OnPropertyChanged(nameof(CurrentPage));
			}
		}
		#endregion

		#region constructor
		/// <summary>
		/// Конструктор.
		/// </summary>
		public RootViewModel()
		{
			_visualEditor = new VisualEditorWindow();
			_mainMenu = new MainMenuWindow();

			(_mainMenu.DataContext as MainMenuViewModel).OpenVisualEditor = new RelayCommand(SetVisualEditor);

			CurrentPage = _mainMenu;
		}
		#endregion

		#region public methods
		#endregion

		#region private metgods
		/// <summary>
		/// Оповещение подписчиков об изменении свойства.
		/// </summary>
		/// <param name="propertyName">Имя свойства.</param>
		private void OnPropertyChanged([CallerMemberName]string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>
		/// Установка страницы редактора.
		/// </summary>
		/// <param name="parameter">Параметр.</param>
		private void SetVisualEditor(object parameter)
		{
			CurrentPage = _visualEditor;
		}
		#endregion
	}
}
