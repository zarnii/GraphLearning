using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using GraphApp.Interfaces;

namespace GraphApp.Services
{
	/// <summary>
	/// Сервис "жизней" пользователя.
	/// </summary>
	public class HealthPointService : IHealthPointService
	{
		/// <summary>
		/// Урон, при неправильном ответе.
		/// </summary>
		private int _damage;

		/// <summary>
		/// Стандартное количество жизней.
		/// </summary>
		private int _defaultHealthPoint;

		/// <summary>
		/// Длительность тайм-аута в секундах. 
		/// </summary>
		private TimeSpan _timeoutDuration;

		/// <summary>
		/// Количество жизней.
		/// </summary>
		public int HealthPoint { get; private set; }

		/// <summary>
		/// Время, когда пользователь снова обретет жизни.
		/// </summary>
		public TimeOnly TimeoutEndTime { get; private set; }

		public HealthPointService()
		{
			_timeoutDuration = TimeSpan.FromSeconds(
				TryReadAppSettings(ConfigurationManager.AppSettings["defaultTimeOutDuration"])
			);

#if DEBUG
			_timeoutDuration = TimeSpan.FromSeconds(5);
#endif

			_defaultHealthPoint = TryReadAppSettings(ConfigurationManager.AppSettings["defaultHealthPoint"]);
			_damage = TryReadAppSettings(ConfigurationManager.AppSettings["defaultDamage"]);
			HealthPoint = _defaultHealthPoint;
		}

		private int TryReadAppSettings(string settingName)
		{
			int outValue;

			if (!Int32.TryParse(settingName, out outValue))
			{
				throw new InvalidCastException($"Неудалось преобразовать аргумент \"{settingName}\" " +
					"из App.config в Int32");
			}

			return outValue;
		}

		private void ResetHealhPoint()
		{
			Task.Run(async () =>
			{
				await Task.Delay(_timeoutDuration);
				HealthPoint = _defaultHealthPoint;
			});
		}

		/// <summary>
		/// Проверка на возможность востановить жизни.
		/// </summary>
		/// <returns>True, если время тайм-аута прошло.</returns>
		public bool TimeoutIsEnd()
		{
			return TimeoutEndTime <= TimeOnly.FromDateTime(DateTime.Now);
		}

		/// <summary>
		/// Нанесение урона.
		/// </summary>
		public void Hit()
		{
			if (HealthPoint == 0)
			{
				return;
			}

			HealthPoint -= _damage;

			if (HealthPoint == 0)
			{
				TimeoutEndTime = TimeOnly.FromDateTime(DateTime.Now).Add(_timeoutDuration);
				ResetHealhPoint();
			}
		}
	}
}
