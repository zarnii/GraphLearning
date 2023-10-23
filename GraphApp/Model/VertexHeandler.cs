using System;
using System.Collections.Generic;

namespace GraphApp.Model
{
	/// <summary>
	/// Обработчик вершин.
	/// </summary>
	public class VertexHeandler
	{
		#region fields
		/// <summary>
		/// Лист вершин.
		/// </summary>
		private List<Vertex> _vertexList;
		#endregion


		#region properties
		/// <summary>
		/// Лист вершин.
		/// </summary>
		public List<Vertex> VertexList
		{
			get
			{
				return _vertexList;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value), "Пустой лист веришн.");
				}
			}
		}
		#endregion


		#region constructor
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="vertexList">Лист вершин.</param>
		public VertexHeandler(List<Vertex> vertexList)
		{
			_vertexList = vertexList;
		}
		#endregion


		#region public methods
		/// <summary>
		/// Добавление вершины в лист вершин.
		/// </summary>
		/// <param name="vertex">Добовляемая вершина.</param>
		public void AddVertex(Vertex vertex)
		{
			VertexList.Add(vertex);
			vertex.OnDelete += DeleteVertex;
		}
		#endregion


		#region private methods
		/// <summary>
		/// Удаление вершины из спика.
		/// </summary>
		/// <param name="vertex">Удаляемая вершина.</param>
		private void DeleteVertex(Vertex vertex)
		{
			_vertexList.Remove(vertex);
		}
		#endregion
	}
}
