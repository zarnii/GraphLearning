﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphApp.Interfaces;
using GraphApp.Model;

namespace GraphApp.Services
{
    /// <summary>
    /// Сервис проверки практических заданий.
    /// </summary>
    public class VerifyPracticTaskService : IVerifyPracticTaskService
    {
        /// <summary>
        /// Проверить практическое задание.
        /// </summary>
        /// <param name="practicTask">Практическое задание.</param>
        /// <param name="vertices">Коллеция вершин пользователя.</param>
        /// <param name="connections">Коллекция связей пользователя.</param>
        /// <returns>Проверенное задание.</returns>
        public VerifiedPracticTask VerifyPracticTask(
            PracticTask practicTask, 
            IList<VisualVertex> vertices, 
            IList<VisualConnection> connections)
        {
            var verifiedPracticTask = new VerifiedPracticTask();

            var verticesCopyActual = new VisualVertex[vertices.Count];
            vertices.CopyTo(verticesCopyActual, 0);
            var connectionsCopyActual = new VisualConnection[connections.Count];
            connections.CopyTo(connectionsCopyActual, 0);

            var verticesCopyExpectex = new VisualVertex[practicTask.Vertices.Count];
            practicTask.Vertices.CopyTo(verticesCopyExpectex, 0);
            var connectionsCopyExpected = new VisualConnection[practicTask.Connections.Count];
            practicTask.Connections.CopyTo(connectionsCopyExpected, 0);

            Array.Sort<VisualVertex>(verticesCopyActual);
            Array.Sort<VisualConnection>(connectionsCopyActual);
            Array.Sort<VisualVertex>(verticesCopyExpectex);
            Array.Sort<VisualConnection>(connectionsCopyExpected);
            

            verifiedPracticTask.VertexCountIsDone = VertexCountIsDone(verticesCopyActual, verticesCopyExpectex);
            verifiedPracticTask.VertexPositionIsDone = VertexPositionIsDone(verticesCopyActual, verticesCopyExpectex);
            verifiedPracticTask.VertexSizeIsDone = VertexSizeIsDone(verticesCopyActual, verticesCopyExpectex);
            verifiedPracticTask.VertexNameIsDone = VertexNameIsDone(verticesCopyActual, verticesCopyExpectex);
            verifiedPracticTask.ConnectionCountIsDone = ConnectionCountIsDone(connectionsCopyActual, connectionsCopyExpected);
            verifiedPracticTask.ConnectionIsDone = ConnectionIsDone(connectionsCopyActual, connectionsCopyExpected);
            verifiedPracticTask.ConnectionWeightIsDone = ConnectionWeightIsDone(connectionsCopyActual, connectionsCopyExpected);
            verifiedPracticTask.ConnectionTypeIsDone = ConnectionTypeIsDone(connectionsCopyActual, connectionsCopyExpected);

            return verifiedPracticTask;
        }

        private bool VertexCountIsDone(IList<VisualVertex> actualVertices, 
            IList<VisualVertex> expectedVertex)
        {
            return actualVertices.Count == expectedVertex.Count;
        }

        private bool VertexPositionIsDone(IList<VisualVertex> actualVertices, 
            IList<VisualVertex> expectedVertices)
        {
            if (!VertexCountIsDone(actualVertices, expectedVertices))
            {
                return false;
            }

            for (var i = 0; i < actualVertices.Count; i++)
            {
                if (actualVertices[i].X != expectedVertices[i].X
                    || actualVertices[i].Y != expectedVertices[i].Y)
                {
                    return false;
                }
            }

            return true;
        }

        private bool VertexSizeIsDone(IList<VisualVertex> actualVertices,
            IList<VisualVertex> expectexVertices)
        {
            if (!VertexCountIsDone(actualVertices, expectexVertices))
            {
                return false;
            }

            for (var i = 0; i < expectexVertices.Count; i++)
            {
                if (actualVertices[i].Width != expectexVertices[i].Width
                    || actualVertices[i].Height != expectexVertices[i].Height)
                {
                    return false;
                }
            }

            return true;
        }

        private bool VertexNameIsDone(IList<VisualVertex> actualVertices,
            IList<VisualVertex> expectexVertices)
        {
            if (!VertexCountIsDone(actualVertices, expectexVertices))
            {
                return false;
            }

            for (var i = 0; i < actualVertices.Count; i++)
            {
                if (actualVertices[i].Name != expectexVertices[i].Name)
                {
                    return false;
                }
            }

            return false;
        }

        private bool ConnectionCountIsDone(IList<VisualConnection> actualConnections,
           IList<VisualConnection> expectexConnections)
        {
            return actualConnections.Count == expectexConnections.Count;
        }

        private bool ConnectionIsDone(IList<VisualConnection> actualConnection,
            IList<VisualConnection> expectexConnection)
        {
            if (!ConnectionCountIsDone(actualConnection, expectexConnection))
            {
                return false;
            }

            for (var i = 0; i < actualConnection.Count; i++)
            {
                var actualConnectedVertices = actualConnection[i].ConnectedVertices;
                var expectedConnectedVerices = expectexConnection[i].ConnectedVertices;

                if (actualConnectedVertices.Item1.Name != expectedConnectedVerices.Item1.Name
                    || actualConnectedVertices.Item2.Name != expectedConnectedVerices.Item2.Name)
                {
                    return false;
                }
            }

            return true;
        }

        private bool ConnectionWeightIsDone(IList<VisualConnection> actualConnections,
            IList<VisualConnection> expectedConnections)
        {
            if (!ConnectionCountIsDone(actualConnections, expectedConnections))
            {
                return false;
            }

            for (var i = 0; i < actualConnections.Count; i++)
            {
                if (actualConnections[i].Weight != expectedConnections[i].Weight)
                {
                    return false;
                }
            }

            return true;
        }

        private bool ConnectionTypeIsDone(IList<VisualConnection> actualConnections,
            IList<VisualConnection> expectedConnections)
        {
            if (!ConnectionCountIsDone(actualConnections, expectedConnections))
            {
                return false;
            }

            for (var i = 0; i < actualConnections.Count; i++)
            {
                if (actualConnections[i].ConnectionType != expectedConnections[i].ConnectionType)
                {
                    return false;
                }
            }

            return true;
        }
    }
}