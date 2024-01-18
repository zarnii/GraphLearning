﻿using GraphApp.Model;
using GraphApp.Model.Serializing;
using System.Collections.Generic;

namespace GraphApp.Interfaces
{
	/// <summary>
	/// Сервис сохранения данных.
	/// </summary>
	public interface IDataSaver
	{
		void Save(string path, List<SerializableVertex> vertices, List<SerializableConnection> connections);
	}
}