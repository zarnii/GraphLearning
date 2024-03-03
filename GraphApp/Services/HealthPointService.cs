using GraphApp.Interfaces;
using GraphApp.Model;
using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GraphApp.Services
{
	/// <summary>
	/// Сервис "жизней" пользователя.
	/// </summary>
	public class HealthPointService : IHealthPointService
	{
        #region fileds
        /// <summary>
        /// Урон, при неправильном ответе.
        /// </summary>
        private int _damage;

		/// <summary>
		/// Стандартное количество жизней.
		/// </summary>
		private int _defaultHealthPoint;

		/// <summary>
		/// Путь до файла с записью жизней.
		/// </summary>
		private string _pathToHealthFile;

		/// <summary>
		/// Длительность тайм-аута в секундах. 
		/// </summary>
		private TimeSpan _timeoutDuration;

		/// <summary>
		/// Сервис обработки данных.
		/// </summary>
		private IDataHeandlerService _dataHandler;

		/// <summary>
		/// Маппер.
		/// </summary>
		private IMapper _mapper;
        #endregion

        #region properties
        /// <summary>
        /// Количество жизней.
        /// </summary>
        public int HealthPoint { get; private set; }

		/// <summary>
		/// Время, когда пользователь снова обретет жизни.
		/// </summary>
		public DateTime TimeoutEndTime { get; private set; }
        #endregion

        #region constructor
        public HealthPointService(IDataHeandlerService dataHeandler, IMapper mapper)
		{
			_timeoutDuration = TimeSpan.FromSeconds(
				TryReadAppSettings(ConfigurationManager.AppSettings["defaultTimeOutDuration"])
			);

#if DEBUG
			_timeoutDuration = TimeSpan.FromSeconds(30);
#endif

			_defaultHealthPoint = TryReadAppSettings(ConfigurationManager.AppSettings["defaultHealthPoint"]);
			_damage = TryReadAppSettings(ConfigurationManager.AppSettings["defaultDamage"]);
			_dataHandler = dataHeandler;
			_mapper = mapper;
			_pathToHealthFile = ConfigurationManager.AppSettings["defaultPathToHealthFile"];

			
			if (CheckHealthFileIsEmpty())
			{
				HealthPoint = _defaultHealthPoint;
				TimeoutEndTime = DateTime.Now;
			}
			else
			{
				LoadData();
			}
			
		}
        #endregion

        #region private methods
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

		private void AwaitResetHealhPoint(TimeSpan delay)
		{
			Task.Run(async () =>
			{
				await Task.Delay(delay);
				HealthPoint = _defaultHealthPoint;
			});
		}

		/// <summary>
		/// Загрузка сохраненных данных.
		/// </summary>
		private void LoadData()
		{
			var serializableHealthData = 
				_dataHandler
				.Load<Model.Serializing.SerializableHealthData>(_pathToHealthFile);

			var healthData = _mapper.Map<HealthData>(serializableHealthData, null);

			// Если время тайм-аута уже вышло
			// то в поля помещаем стандартные значения,
			// иначе значение из дампа.
			if (healthData.TimeoutEndTime <= DateTime.Now)
			{
				HealthPoint = _defaultHealthPoint;
				TimeoutEndTime = DateTime.Now;
			}
			else
			{
				HealthPoint = healthData.HealthPoint;
				TimeoutEndTime = healthData.TimeoutEndTime;
				AwaitResetHealhPoint(TimeoutEndTime - DateTime.Now);
			}
		}

		/// <summary>
		/// Сохранение данных.
		/// </summary>
		private void SaveData()
		{
			var data = _mapper.Map<Model.Serializing.SerializableHealthData>(
				new Model.HealthData()
				{
					HealthPoint = HealthPoint,
					TimeoutEndTime = TimeoutEndTime,
				},
				null
			);

			_dataHandler.Save<Model.Serializing.SerializableHealthData>(data, _pathToHealthFile);
		}

		/// <summary>
		/// Проверка пустой ли файл с данными.
		/// </summary>
		/// <returns></returns>
		private bool CheckHealthFileIsEmpty()
		{
			return String.IsNullOrWhiteSpace(File.ReadAllText(_pathToHealthFile));
		}
        #endregion

        #region public methods
        /// <summary>
        /// Проверка на возможность востановить жизни.
        /// </summary>
        /// <returns>True, если время тайм-аута прошло.</returns>
        public bool TimeoutIsEnd()
		{
			return TimeoutEndTime <= DateTime.Now;
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
				TimeoutEndTime = DateTime.Now.Add(_timeoutDuration);
				SaveData();
				AwaitResetHealhPoint(_timeoutDuration);
			}
		}
        #endregion
    }
}
