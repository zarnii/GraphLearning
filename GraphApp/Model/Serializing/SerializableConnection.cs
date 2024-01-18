﻿namespace GraphApp.Model
{
	/// <summary>
	/// Сериализуемая связь.
	/// </summary>
	public class SerializableConnection
	{
		public int[] ConnectedVerticesNumber { get; set; }

		public double Weight { get; set; }

		public ConnectionType connectionType { get; set; }
	}
}