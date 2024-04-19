using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.Model.Exception;
using GraphApp.Model.Serializing;
using GraphApp.Services;
using GraphApp.Services.FactoryViewModel;
using GraphApp.Services.Providers;
using GraphApp.ViewModel;
using GraphApp.ViewModel.Verify;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;


namespace GraphApp
{
    /// <summary>
    /// Приложение.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Поставщик сервисов.
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        private readonly BrushConverter _brushConverter;

        public App()
        {
            _brushConverter = new BrushConverter();

            var dependencyCollection = new ServiceCollection();

            #region window
            dependencyCollection.AddSingleton<RootWindow>();
            #endregion

            #region services
            dependencyCollection.AddSingleton<IMapper, Mapper>();
            dependencyCollection.AddSingleton<IDataSaver, JsonSaverService>();
            dependencyCollection.AddSingleton<IDataLoader, JsonLoaderService>();
            dependencyCollection.AddSingleton<IDataHandlerService, JsonDataHandlerService>();
            dependencyCollection.AddSingleton<INavigationService, NavigationService>();
            dependencyCollection.AddKeyedSingleton<IEducationMaterialProvider, TestProvider>(typeof(TestProvider));
            dependencyCollection.AddKeyedSingleton<IEducationMaterialProvider, PracticProvider>(typeof(PracticProvider));
            dependencyCollection.AddKeyedSingleton<IEducationMaterialProvider, CreateMatrixTaskProvider>(typeof(CreateMatrixTaskProvider));
            dependencyCollection.AddSingleton<ITheoryService, TheoryService>();
            dependencyCollection.AddSingleton<IHealthPointService, HealthPointService>();
            dependencyCollection.AddTransient<IVisualEditorService, VisualEditorService>();
            dependencyCollection.AddSingleton<IQuestionProvider, QuestionProvider>();
            dependencyCollection.AddSingleton<ITestGenerator, TestGenerator>();
            dependencyCollection.AddSingleton<IAccessControlService, AccessControlService>();
            dependencyCollection.AddSingleton<IMessageBuffer, MessageBuffer>();
            dependencyCollection.AddKeyedSingleton<IFactoryViewModel, FactoryVerifyTestViewModel>(typeof(FactoryVerifyTestViewModel));
            dependencyCollection.AddKeyedSingleton<IFactoryViewModel, FactoryVerifyPracticTaskViewModel>(typeof(FactoryVerifyPracticTaskViewModel));
            dependencyCollection.AddKeyedSingleton<IFactoryViewModel, FactoryVerifyCreateMatrixTask>(typeof(FactoryVerifyCreateMatrixTask));
            #endregion

            #region viewModel
            dependencyCollection.AddSingleton<RootViewModel>();
            dependencyCollection.AddSingleton<MainMenuViewModel>();
            dependencyCollection.AddTransient<PlaygroundViewModel>();
            dependencyCollection.AddSingleton<EducationViewModel>();
            dependencyCollection.AddTransient<TestViewModel>();
            dependencyCollection.AddSingleton<TheoryViewModel>();
            dependencyCollection.AddSingleton<ScrollTestViewModel>();
            dependencyCollection.AddTransient<VerifyTestViewModel>();
            dependencyCollection.AddTransient<PracticViewModel>();
            dependencyCollection.AddTransient<VertexViewModel>();
            dependencyCollection.AddTransient<ConnectionViewModel>();
            dependencyCollection.AddSingleton<InstructionViewModel>();
            dependencyCollection.AddTransient<SettingsViewModel>();
            dependencyCollection.AddTransient<CreateMatrixTaskViewModel>();
            #endregion

            #region factory
            // Фабрика vm.
            dependencyCollection.AddSingleton<Func<Type, ViewModel.ViewModel>>((vmType) =>
            {
                return (ViewModel.ViewModel)_serviceProvider.GetRequiredService(vmType);
            });

            // Фабрика VerifyTestViewModel.
            dependencyCollection.AddSingleton<Func<Dictionary<Question, List<VisualAnswer>>, string, VerifyTestViewModel>>((dict, message) =>
            {
                return new VerifyTestViewModel(
                    _serviceProvider.GetRequiredService<INavigationService>(),
                    dict,
                    message
                );
            });

            // Фабрика VerifyPracticTaskViewModel.
            dependencyCollection.AddSingleton<
                Func<VerifiedPracticTask, 
                    PracticTask, 
                    IList<VisualVertex>, 
                    IList<VisualConnection>,
                    VerifyPracticViewModel>
            >((verifiedTask, verifableTask, vertices, connections) =>
            {
                return new VerifyPracticViewModel(
                    _serviceProvider.GetRequiredService<INavigationService>(),
                    _serviceProvider.GetRequiredService<IAccessControlService>(),
                    verifiedTask,
                    verifableTask,
                    vertices,
                    connections
                );
            });

            // Фабрика VerifyCreateMatrixTaskViewModel.
            dependencyCollection.AddSingleton<
                Func<AdjacencyMatrix, 
                int[,],
                IList<VisualVertex>,
                IList<VisualConnection>,
                VerifyCreateMatrixTaskViewModel>
            >((correctMatrix, userMatrix, vertices, connections) =>
            {
                return new VerifyCreateMatrixTaskViewModel(
                    _serviceProvider.GetRequiredService<INavigationService>(),
                    _serviceProvider.GetRequiredService<IAccessControlService>(),
                    correctMatrix,
                    userMatrix,
                    vertices,
                    connections
                );
            });
            #endregion

            _serviceProvider = dependencyCollection.BuildServiceProvider();

            SettingMapper();
        }

        /// <summary>
        /// При старте приложения.
        /// </summary>
        /// <param name="e">Аргументы собития запуска.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            var rootWindow = _serviceProvider.GetRequiredService<RootWindow>();
            rootWindow.Show();

            base.OnStartup(e);
            var time = new TimeSpan(0, 3, 0);

            var s = System.Text.Json.JsonSerializer.Serialize(time);
        }

        private void SettingMapper()
        {
            var mapper = _serviceProvider.GetRequiredService<IMapper>();

            // SerializableVertex => VisualVertex.
            mapper.CreateMap<SerializableVertex, VisualVertex>((tSource, param) =>
            {
                try
                {
                    var sv = tSource as SerializableVertex;

                    return new VisualVertex(
                        (sv.X, sv.Y),
                        sv.Radius,
                        sv.Number,
                        (Color)ColorConverter.ConvertFromString(sv.ColorString),
                        sv.Name
                    );
                }
                catch (FormatException ex)
                {
                    throw new LoadDataException("Ошибка формата", ex);
                }
                catch (NullReferenceException ex)
                {
                    throw new LoadDataException("Пуста ссылка", ex);
                }

            });

            // SerializableConnection => VisualConnection.
            mapper.CreateMap<SerializableConnection, VisualConnection>((tSource, param) =>
            {
                try
                {
                    var sc = tSource as SerializableConnection;
                    var vertices = param as List<VisualVertex>;

                    var firstVertex = vertices.Where(v => v.Number == sc.ConnectedVerticesNumber[0]).FirstOrDefault();
                    var secondVertex = vertices.Where(v => v.Number == sc.ConnectedVerticesNumber[1]).FirstOrDefault();

                    return new VisualConnection((firstVertex, secondVertex), sc.Number, sc.Thickness, sc.Weight, sc.ConnectionType);
                }
                catch (ArgumentNullException ex)
                {
                    throw new LoadDataException(String.Empty, ex);
                }

            });

            // VisualVertex => SerializableVertex.
            mapper.CreateMap<VisualVertex, SerializableVertex>((tSource, param) =>
            {
                var vv = tSource as VisualVertex;

                return new SerializableVertex()
                {
                    X = vv.X,
                    Y = vv.Y,
                    Number = vv.Number,
                    Name = vv.Name,
                    Radius = vv.Radius,
                    ColorString = _brushConverter.ConvertToString(vv.Color)
                };

            });

            // VisualConnection => SerializableConnection.
            mapper.CreateMap<VisualConnection, SerializableConnection>((tSource, param) =>
            {
                var vc = tSource as VisualConnection;
                var connectedVertices = vc.ConnectedVertices;

                var s = new SerializableConnection()
                {
                    ConnectedVerticesNumber = new int[2] { connectedVertices.Item1.Number, connectedVertices.Item2.Number },
                    Thickness = vc.Thickness,
                    Weight = vc.Weight,
                    ConnectionType = vc.ConnectionType
                };

                return s;
            });

            // PracticTask => SerializablePracticTask.
            mapper.CreateMap<PracticTask, SerializablePracticTask>((tSource, param) =>
            {
                var pt = tSource as PracticTask;

                var serVertices = new List<SerializableVertex>();
                var serConnections = new List<SerializableConnection>();

                foreach (var vertex in pt.Vertices)
                {
                    serVertices.Add(mapper.Map<SerializableVertex>(vertex, null));
                }

                foreach (var connection in pt.Connections)
                {
                    serConnections.Add(mapper.Map<SerializableConnection>(connection, null));
                }

                var spt = new SerializablePracticTask()
                {
                    Title = pt.Title,
                    Text = pt.Text,
                    IndexNumber = pt.IndexNumber,
                    LeadTime = pt.LeadTime,
                    Vertices = serVertices,
                    Connections = serConnections,
                    NeedCheckVertexCount = pt.NeedCheckVertexCount,
                    NeedCheckVertexPosition = pt.NeedCheckVertexPosition,
                    NeedCheckVertexSize = pt.NeedCheckVertexSize,
                    NeedCheckVertexName = pt.NeedCheckVertexName,
                    NeedCheckConnection = pt.NeedCheckConnection,
                    NeedCheckConnectionCount = pt.NeedCheckConnectionCount,
                    NeedCheckConnectionWeight = pt.NeedCheckConnectionWeight,
                    NeedCheckConnectionType = pt.NeedCheckConnectionType
                };

                return spt;
            });

            // SerializablePracticTask => PracticTask.
            mapper.CreateMap<SerializablePracticTask, PracticTask>((tSource, param) =>
            {
                var spt = tSource as SerializablePracticTask;

                var vertices = new List<VisualVertex>();
                var connections = new List<VisualConnection>();

                foreach (var vertex in spt.Vertices)
                {
                    vertices.Add(mapper.Map<VisualVertex>(vertex, null));
                }

                foreach (var connection in spt.Connections)
                {
                    connections.Add(mapper.Map<VisualConnection>(connection, vertices));
                }

                var pt = new PracticTask()
                {
                    Title = spt.Title,
                    Text = spt.Text,
                    IndexNumber = spt.IndexNumber,
                    LeadTime = spt.LeadTime,
                    Vertices = vertices,
                    Connections = connections,
                    NeedCheckVertexCount = spt.NeedCheckVertexCount,
                    NeedCheckVertexPosition = spt.NeedCheckVertexPosition,
                    NeedCheckVertexSize = spt.NeedCheckVertexSize,
                    NeedCheckVertexName = spt.NeedCheckVertexName,
                    NeedCheckConnection = spt.NeedCheckConnection,
                    NeedCheckConnectionCount = spt.NeedCheckConnectionCount,
                    NeedCheckConnectionWeight = spt.NeedCheckConnectionWeight,
                    NeedCheckConnectionType = spt.NeedCheckConnectionType
                };

                return pt;
            });

            // SerializableCreateMatrixTask => CreateMatrixTask.
            mapper.CreateMap<SerializableCreateMatrixTask, CreateMatrixTask>((tSource, param) =>
            {
                var scmt = (SerializableCreateMatrixTask)tSource;
                var vertices = new List<VisualVertex>(scmt.Graph.Vertices.Count);

                foreach (var vertex in scmt.Graph.Vertices)
                {
                    vertices.Add(mapper.Map<VisualVertex>(vertex, null));
                }

                var connections = new List<VisualConnection>(scmt.Graph.Connections.Count);

                foreach (var connection in scmt.Graph.Connections)
                {
                    connections.Add(mapper.Map<VisualConnection>(connection, vertices));
                }

                return new CreateMatrixTask()
                {
                    Title = scmt.Title,
                    IndexNumber = scmt.IndexNumber,
                    LeadTime = scmt.LeadTime,
                    Vertices = vertices,
                    Connections = connections
                };
            });
        }
    }
}
