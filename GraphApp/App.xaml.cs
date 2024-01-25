using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.Model.Exception;
using GraphApp.Model.Serializing;
using GraphApp.Services;
using GraphApp.View;
using GraphApp.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

			var serviceCollection = new ServiceCollection();

			#region window
			serviceCollection.AddSingleton<RootWindow>();
			#endregion

			#region services
			serviceCollection.AddSingleton<IMapper, Mapper>();
			serviceCollection.AddSingleton<IDataSaver, DataSaverServices>();
			serviceCollection.AddSingleton<IDataLoader, DataLoaderServices>();
			serviceCollection.AddSingleton<IDataHeandlerService, DataHeandlerService>();
			serviceCollection.AddSingleton<INavigationService, NavigationService>();
			#endregion

			#region viewModel
			serviceCollection.AddSingleton<RootViewModel>();
			serviceCollection.AddSingleton<MainMenuViewModel>();
			serviceCollection.AddSingleton<VisualEditorViewModel>();
			serviceCollection.AddSingleton<LearnLevelsViewModel>();
			serviceCollection.AddSingleton<FirstTheoryViewModel>();
			#endregion

			#region other
			// Фабричная функция vm.
			serviceCollection.AddSingleton<Func<Type, ViewModel.ViewModel>>((vmType) =>
			{
				return (ViewModel.ViewModel)_serviceProvider.GetRequiredService(vmType);
			});
			#endregion

			_serviceProvider = serviceCollection.BuildServiceProvider();

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
		}

		private void SettingMapper()
		{
			var mapper = _serviceProvider.GetRequiredService<IMapper>();
			mapper.CreateMap<SerializableVertex, VisualVertex>((tSource, param) =>
			{
				try
				{
					var sv = tSource as SerializableVertex;

					return new VisualVertex(
						(sv.X, sv.Y),
						sv.Width,
						sv.Height,
						sv.Number,
						(Color)ColorConverter.ConvertFromString(sv.ColorString)
					);
				}
				catch (FormatException ex)
				{
					throw new LoadDataException("Ошибка формата", ex);
				}

			});

			mapper.CreateMap<SerializableConnection, VisualConnection>((tSource, param) =>
			{
				try
				{
					var sc = tSource as SerializableConnection;
					var vertices = param as List<VisualVertex>;

					var firstVertex = vertices.Where(v => v.Number == sc.ConnectedVerticesNumber[0]).FirstOrDefault();
					var secondVertex = vertices.Where(v => v.Number == sc.ConnectedVerticesNumber[1]).FirstOrDefault();

					return new VisualConnection((firstVertex, secondVertex), sc.Weight, sc.ConnectionType);
				}
				catch (ArgumentNullException ex)
				{
					throw new LoadDataException(String.Empty, ex);
				}

			});

			mapper.CreateMap<VisualVertex, SerializableVertex>((tSource, param) =>
			{
				var vv = tSource as VisualVertex;

				return new SerializableVertex()
				{
					X = vv.X,
					Y = vv.Y,
					Number = vv.Number,
					Name = vv.Name,
					Height = vv.Height,
					Width = vv.Width,
					ColorString = _brushConverter.ConvertToString(vv.Color)
				};

			});

			mapper.CreateMap<VisualConnection, SerializableConnection>((tSource, param) =>
			{
				var vc = tSource as VisualConnection;
				var connectedVertices = vc.ConnectedVertices;

				return new SerializableConnection()
				{
					ConnectedVerticesNumber = new int[2] { connectedVertices.Item1.Number, connectedVertices.Item2.Number },
					Weight = vc.Weight,
					ConnectionType = vc.ConnectionType
				};
			});
		}
	}
}
