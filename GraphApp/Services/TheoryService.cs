using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.View.Theory;
using System;
using System.Collections.Generic;

namespace GraphApp.Services
{
	/// <summary>
	/// Сервис теорий.
	/// </summary>
	public class TheoryService : ITheoryService
	{
        #region fields
        private Theory _currentTheory;
        #endregion

        #region properies
        /// <summary>
        /// Текущая теория.
        /// </summary>
        public Theory CurrentTheory
		{
			get
			{
				return _currentTheory;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value), "Пустой theory.");
				}

				_currentTheory = value;
			}
		}

		/// <summary>
		/// Коллекция теорий.
		/// </summary>
		public List<Pair> TheoryControls { get; private set; }
        #endregion

        #region constructor
		/// <summary>
		/// Конструктор.
		/// </summary>
        public TheoryService()
		{
			InitControl();
		}
        #endregion

        #region private methods
        /// <summary>
        /// Инициализация коллекций теорий.
        /// </summary>
        private void InitControl()
		{
            TheoryControls = new List<Pair>
            {
				new Pair("Введение", new Theory(new FirstTheoryView())),
				new Pair("Основные понятия", new Theory(new SecondTheoryView())),
				new Pair("Виды графов", new Theory(new ThridTheoryView()))
            };
		}
        #endregion
    }
}
