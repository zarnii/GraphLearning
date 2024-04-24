using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.Services.Converters;
using Petzold.Media2D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GraphApp.Services
{
    public class GraphImageSaver : IGraphImageSaver
    {
        public void SaveAsPng(
            ICollection<VisualVertex> vertices, 
            ICollection<VisualConnection> connections, 
            string pathToSaveDir,
            double surfaceWidth,
            double surfaceheigth)
        {
            if (vertices == null)
            {
                throw new ArgumentNullException(nameof(vertices));
            }

            if (connections == null)
            {
                throw new ArgumentNullException(nameof(connections));
            }

            if (String.IsNullOrEmpty(pathToSaveDir))
            {
                throw new ArgumentNullException(nameof(pathToSaveDir));
            }

            var canvas = new Canvas()
            {
                Width = surfaceWidth,
                Height = surfaceheigth,
                Background = new SolidColorBrush(Colors.Gray)
            };
            AddConnectionOnCanvas(canvas, connections);
            AddVertexOnCanvas(canvas, vertices);
            SaveAsPng(canvas, pathToSaveDir);
        }

        /// <summary>
        /// Добавление вершин на канвас.
        /// </summary>
        /// <param name="canvas">Канвас.</param>
        /// <param name="vertices">Коллекция вершин.</param>
        private void AddVertexOnCanvas(Canvas canvas, ICollection<VisualVertex> vertices)
        {
            var radiusConverter = new RadiusConverter();
            var vertexNameCoordinatesConverter = new VertexNameCoordinatesConverter();

            foreach (var vertex in vertices)
            {
                var ellipse = new Ellipse()
                {
                    Width = (int)radiusConverter.Convert(vertex.Radius, null, null, null),
                    Height = (int)radiusConverter.Convert(vertex.Radius, null, null, null),
                    Fill = vertex.Color
                };
                Canvas.SetLeft(ellipse, vertex.X);
                Canvas.SetTop(ellipse, vertex.Y);

                var name = new Label()
                {
                    Foreground = new SolidColorBrush(Colors.White),
                    Content = vertex.Name
                };

                Canvas.SetLeft(
                    name,
                    (double)vertexNameCoordinatesConverter.Convert(
                        new object[] { vertex.X, vertex.Radius },
                        null,
                        "X",
                        null
                    )
                );
                Canvas.SetTop(
                    name,
                    (double)vertexNameCoordinatesConverter.Convert(
                        new object[] { vertex.Y, vertex.Radius },
                        null,
                        "Y",
                        null
                    )
                );

                canvas.Children.Add(ellipse);
                canvas.Children.Add(name);
            }
        }

        /// <summary>
        /// Добавление связей на канвас.
        /// </summary>
        /// <param name="canvas">Канвас.</param>
        /// <param name="connections">Коллекция связей.</param>
        private void AddConnectionOnCanvas(Canvas canvas, ICollection<VisualConnection> connections)
        {
            var connectionCoordinatesConverter = new ConnectionCoordinatesConverter();
            var connectionNumberCoordinatesConverter = new ConnectionNumberCoordinatesConverter();
            var connectionTypeVisualisator = new ConnectionTypeVisualisator();
            var connectionWeightCoordinatesConverter = new ConnectionWeightCoordinatesConverter();

            foreach (var connection in connections)
            {
                var arrow = new ArrowLine()
                {
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = connection.Thickness,
                    ArrowEnds = (ArrowEnds)connectionTypeVisualisator.Convert(connection.ConnectionType, null, null, null),
                    X1 = (double)connectionCoordinatesConverter.Convert(
                        new object[5] 
                        { 
                            connection.X2, 
                            connection.Y2, 
                            connection.X1, 
                            connection.Y1, 
                            connection.FirstConnectedVertex.Radius 
                        },
                        null,
                        "X",
                        null
                    ),
                    Y1 = (double)connectionCoordinatesConverter.Convert(
                        new object[5]
                        {
                            connection.X2,
                            connection.Y2,
                            connection.X1,
                            connection.Y1,
                            connection.FirstConnectedVertex.Radius
                        },
                        null,
                        "Y",
                        null
                    ),
                    X2 = (double)connectionCoordinatesConverter.Convert(
                        new object[5]
                        {
                            connection.X1,
                            connection.Y1,
                            connection.X2,
                            connection.Y2,
                            connection.SecondConnectedVertex.Radius
                        },
                        null,
                        "X",
                        null
                    ),
                    Y2 = (double)connectionCoordinatesConverter.Convert(
                       new object[5]
                       {
                            connection.X1,
                            connection.Y1,
                            connection.X2,
                            connection.Y2,
                            connection.SecondConnectedVertex.Radius
                       },
                       null,
                       "Y",
                       null
                    )
                };
                canvas.Children.Add(arrow);

                if (connection.FirstConnectedVertex == connection.SecondConnectedVertex)
                {
                    AddCycleConnection(canvas, connection);
                }
                AddConnectionWeight(canvas, connection, connectionWeightCoordinatesConverter);
                AddConnectionNumber(canvas, connection, connectionNumberCoordinatesConverter);
            }
        }

        /// <summary>
        /// Добавление кольцевой связи.
        /// </summary>
        /// <param name="canvas">Канвас.</param>
        /// <param name="connection">Связь.</param>
        private void AddCycleConnection(Canvas canvas, VisualConnection connection)
        {
            var radiusConverter = new RadiusConverter();

            var ellipse = new Ellipse()
            {
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = connection.Thickness,
                Width = (int)radiusConverter.Convert(connection.FirstConnectedVertex.Radius, null, null, null),
                Height = (int)radiusConverter.Convert(connection.SecondConnectedVertex.Radius, null, null, null),
            };
            Canvas.SetLeft(ellipse, connection.X1);
            Canvas.SetTop(ellipse, connection.Y1);

            canvas.Children.Add(ellipse);
        }

        /// <summary>
        /// Добавление веса связи.
        /// </summary>
        /// <param name="canvas">Канвас.</param>
        /// <param name="connection">Связь.</param>
        /// <param name="converter">Конвертор веса связи.</param>
        private void AddConnectionWeight(Canvas canvas, VisualConnection connection, IMultiValueConverter converter)
        {
            var label = new Label()
            {
                Foreground = new SolidColorBrush(Colors.White),
                Content = connection.Weight
            };
            Canvas.SetLeft(label, (double)converter.Convert(
                    new object[4] 
                    { 
                        connection.X1, 
                        connection.X2, 
                        connection.FirstConnectedVertex, 
                        connection.SecondConnectedVertex 
                    }, 
                    null, 
                    null, 
                    null
                )
            );
            Canvas.SetTop(label, (double)converter.Convert(
                    new object[4]
                    {
                        connection.Y1,
                        connection.Y2,
                        connection.FirstConnectedVertex,
                        connection.SecondConnectedVertex
                    },
                    null,
                    null,
                    null
                )
            );
            canvas.Children.Add(label);
        }

        /// <summary>
        /// Добавление номера связи.
        /// </summary>
        /// <param name="canvas">Канвас.</param>
        /// <param name="connection">Связью</param>
        /// <param name="converter">Конвертор номера вершины.</param>
        private void AddConnectionNumber(Canvas canvas, VisualConnection connection, IMultiValueConverter converter)
        {
            var label = new Label()
            { 
                Foreground = new SolidColorBrush(Colors.White),
                Content = connection.Number
            };
            Canvas.SetLeft(label, (double)converter.Convert(
                    new object[4] 
                    {
                        connection.X1,
                        connection.X2,
                        connection.FirstConnectedVertex,
                        connection.SecondConnectedVertex
                    },
                    null,
                    "X",
                    null
                )
            );
            Canvas.SetTop(label, (double)converter.Convert(
                    new object[4] 
                    { 
                        connection.Y1,
                        connection.Y2,
                        connection.FirstConnectedVertex,
                        connection.SecondConnectedVertex
                    },
                    null,
                    "Y",
                    null
                )
            );
            canvas.Children.Add(label);
        }

        /// <summary>
        /// Сохранение в виде png файла.
        /// </summary>
        /// <param name="canvas">Канвас.</param>
        /// <param name="path">Путь сохранения.</param>
        private void SaveAsPng(Canvas canvas, string path)
        {
            var size = canvas.RenderSize;
            canvas.Arrange(new Rect(size));
            var rtb = new RenderTargetBitmap((int)canvas.Width, (int)canvas.Height, 96, 96, System.Windows.Media.PixelFormats.Default);
            rtb.Render(canvas);

            var pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            using (var fileStream = File.Create(path))
            {
                pngEncoder.Save(fileStream);
            }
        }
    }
}
