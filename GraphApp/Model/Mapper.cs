﻿using GraphApp.Interfaces;
using System;
using System.Collections.Generic;

namespace GraphApp.Model
{
	public class Mapper: IMapper
	{
		private Dictionary<object, Func<object, object, object>> _map;

		public Mapper()
		{
			_map = new Dictionary<object, Func<object, object, object>>();
		}

		public void CreateMap<TSourse, TReceiver>(Func<object, object,TReceiver> factory)
			where TSourse : class
			where TReceiver : class
		{
			_map.Add(typeof(TSourse), factory);
		}

		public TReceiver Map<TReceiver>(object key, object param) 
			where TReceiver : class
		{
			return (TReceiver)_map[key.GetType()].Invoke(key, param);
		}
	}
}
