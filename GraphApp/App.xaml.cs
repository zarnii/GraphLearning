using GraphApp.Interfaces;
using GraphApp.View;
using GraphApp.ViewModel;
using GraphApp.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;
using GraphApp.Model;
using GraphApp.Model.Serializing;
using System.Collections.Generic;
using System.Linq;

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

		public App()
		{
			var serviceCollection = new ServiceCollection();

			serviceCollection.AddSingleton<RootWindow>(serviceProvider =>
				new RootWindow()
				{
					DataContext = serviceProvider.GetRequiredService<RootViewModel>()
				}
			);
			serviceCollection.AddSingleton<MainMenuWindow>(serviceProvider =>
				new MainMenuWindow()
				{
					DataContext = serviceProvider.GetRequiredService<MainMenuViewModel>()
				}
			);
			serviceCollection.AddSingleton<VisualEditorWindow>(serviceProvider =>
				new VisualEditorWindow()
				{
					DataContext = serviceProvider.GetRequiredService<VisualEditorViewModel>()
				}
			);

			// Фабричная функция страниц.
			serviceCollection.AddSingleton<Func<Type, Page>>(serviceProvider =>
			{
				return page => (Page)serviceProvider.GetRequiredService(page);
			});


			serviceCollection.AddSingleton<IMapper, Mapper>();
			serviceCollection.AddSingleton<IDataSaver, DataSaverServices>();
			serviceCollection.AddSingleton<IDataLoader, DataLoaderServices>();
			serviceCollection.AddSingleton<IDataHeandlerService, DataHeandlerService>();
			serviceCollection.AddSingleton<RootViewModel>();
			serviceCollection.AddSingleton<MainMenuViewModel>();
			serviceCollection.AddSingleton<VisualEditorViewModel>();

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
			mapper.CreateMap<SerializableVertex, Vertex>((tSource, param) =>
			{
				var sv = tSource as SerializableVertex;
				return new Vertex(sv.X, sv.Y, sv.Number, sv.Name);
			});

			mapper.CreateMap<SerializableConnection, VisualConnection>((tSource, param) =>
			{
				var sc = tSource as SerializableConnection;
				var vertices = param as List<VisualVertex>;

				var firstVertex = vertices.Where(v => v.Number == sc.ConnectedVerticesNumber[0]).FirstOrDefault();
				var secondVertex = vertices.Where(v => v.Number == sc.ConnectedVerticesNumber[1]).FirstOrDefault();

				return new VisualConnection((firstVertex, secondVertex), sc.Weight, sc.connectionType);
			});
		}
	}
}
