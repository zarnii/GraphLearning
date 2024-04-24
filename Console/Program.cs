using GraphApp.Model;

var vertexList = new List<Vertex>();
var connectionList = new List<Connection>();

var vertexHeandler = new VertexHeandler(vertexList);
var connectionHeandler = new ConnectionHeandler(connectionList);


var vertex1 = new Vertex((1, 1), 1);
var vertex2 = new Vertex((2, 2), 2);

vertexHeandler.AddVertex(vertex1);
vertexHeandler.AddVertex(vertex2);

var conn = new Connection((vertex1, vertex2));

connectionHeandler.AddConnection(conn);
vertexList[0].Delete();

Console.ReadKey();