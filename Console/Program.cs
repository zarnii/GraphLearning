using GraphApp.Model;

var vertexList = new List<Vertex>();
var connectionList = new List<Connection>();

var vertexHeandler = new VertexHeandler(vertexList);
var connectionsHeandler = new ConnectionHeandler(connectionList);

vertexHeandler.AddVertex(new Vertex((1, 1), 1));
vertexHeandler.AddVertex(new Vertex((2, 2), 2));
vertexHeandler.AddVertex(new Vertex((3, 3), 3));

connectionsHeandler.AddConnection(new Connection(
	(vertexList[0], vertexList[1]),
	20,
	ConnectionType.Unidirectional
));

connectionList[0].Delete();

Console.ReadKey();