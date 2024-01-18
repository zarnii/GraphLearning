using System;

namespace GraphApp.Interfaces
{
	/// <summary>
	/// Сервис маппинга объектов.
	/// </summary>
	public interface IMapper
	{
		void CreateMap<TSourse, TReceiver>(Func<object, TReceiver> factory) 
			where TSourse   : class
			where TReceiver : class;

		TReceiver Map<TReceiver>(object key) 
			where TReceiver : class;
	}
}
