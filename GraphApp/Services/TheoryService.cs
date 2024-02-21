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
		private Theory _currentTheory;

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

		public List<Theory> TheoryControls { get; private set; }

		public TheoryService()
		{
			InitControl();
		}

		private void InitControl()
		{
			var theoryControlList = new List<Theory>();

			theoryControlList.Add(new Theory(new FirstTheoryView()));
			theoryControlList.Add(new Theory(new SecondTheoryView()));

			TheoryControls = theoryControlList;
		}
	}
}
