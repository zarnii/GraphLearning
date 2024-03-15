namespace GraphApp.Model
{
	/// <summary>
	/// Сериализуемая связь.
	/// </summary>
	public class SerializableConnection
	{
		public int[] ConnectedVerticesNumber { get; set; }

		public int Number { get; set; }

		public int Thickness { get; set; }

        public double Weight { get; set; }

		public ConnectionType ConnectionType { get; set; }
	}
}
