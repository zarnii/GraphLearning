namespace GraphApp.Model
{
	/// <summary>
	/// Сериализуемая связь.
	/// </summary>
	public class SerializableConnection
	{
		public int[] ConnectedVerticesNumber { get; set; }

		public int Number;

		public double Weight { get; set; }

		public ConnectionType ConnectionType { get; set; }
	}
}
