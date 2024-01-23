using System.Text.Json;
using GraphApp.Model;
using System.IO;
using System.Windows.Input;
using GraphApp.Command;
using System;

namespace GraphApp.ViewModel
{
	/// <summary>
	/// Модель представления вопроса.
	/// </summary>
	public class QuastionViewModel
	{
		#region fields
		/// <summary>
		/// Команда установки флага.
		/// </summary>
		private ICommand _setFlag;

		/// <summary>
		/// Выбранный флаг.
		/// </summary>
		private bool _selecredFlag;
		#endregion

		#region properties
		/// <summary>
		/// Вопрос.
		/// </summary>
		public Quastion Quastion { get; set; }

		/// <summary>
		/// Команда выбора ответа.
		/// </summary>
		public ICommand ChoiceAnswer 
		{
			get
			{
				return _setFlag;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}

				_setFlag = value;
			}
		}
		#endregion

		#region constructor
		public QuastionViewModel(string pathToQuastion)
		{
			var json = File.ReadAllText(pathToQuastion);
			// to-do, а то пиздец какой-то.
			Quastion = JsonSerializer.Deserialize<Quastion>(json);
			ChoiceAnswer = new RelayCommand(SetFlag);
		}
		#endregion

		#region private metgods
		/// <summary>
		/// Установка флага.
		/// </summary>
		/// <param name="parameter">Флаг.</param>
		private void SetFlag(object parameter)
		{
			_selecredFlag = (bool)parameter;
		}
		#endregion

		#region public methods
		#endregion
	}
}
