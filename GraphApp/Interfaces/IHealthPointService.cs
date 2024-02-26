using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Interfaces
{
	/// <summary>
	/// Сервис "жизней" пользователя.
	/// </summary>
	public interface IHealthPointService
	{
		/// <summary>
		/// Количество жизней.
		/// </summary>
		int HealthPoint { get; }

		/// <summary>
		/// Время, когда пользователь снова обретет жизни.
		/// </summary>
		DateTime TimeoutEndTime { get; }

		/// <summary>
		/// Нанесение урона.
		/// </summary>
		void Hit();

		/// <summary>
		/// Проверка, закончился ли тайм-аут.
		/// </summary>
		/// <returns></returns>
		bool TimeoutIsEnd();
	}
}
