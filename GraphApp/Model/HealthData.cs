using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Model
{
	/// <summary>
	/// Данные о здоровье пользователя.
	/// </summary>
	public class HealthData
	{
		/// <summary>
		/// Количества здоровья.
		/// </summary>
		public int HealthPoint { get; set; }

		/// <summary>
		/// Время окончания тайм-аута.
		/// </summary>
		public DateTime TimeoutEndTime { get; set; }
	}
}
