using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Model.Serializing
{
	/// <summary>
	/// Сериализуемые данные о здоровьве пользователя.
	/// </summary>
	public class SerializableHealthData
	{
		/// <summary>
		/// Количества здоровья.
		/// </summary>
		public string HealthPoint { get; set; }

		/// <summary>
		/// Время окончания тайм-аута.
		/// </summary>
		public string TimeoutEndTime { get; set; }
	}
}
