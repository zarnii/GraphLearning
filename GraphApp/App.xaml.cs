using GraphApp.Interfaces;
using GraphApp.View;
using GraphApp.ViewModel;
using GraphApp.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;

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

			serviceCollection.AddSingleton<IDataSaver, DataSaverServices>();
			serviceCollection.AddSingleton<IDataLoader, DataLoaderServices>();
			serviceCollection.AddSingleton<IDataHeandlerService, DataHeandlerService>();
			serviceCollection.AddSingleton<RootViewModel>();
			serviceCollection.AddSingleton<MainMenuViewModel>();
			serviceCollection.AddSingleton<VisualEditorViewModel>(serviceProvider =>
				new VisualEditorViewModel(serviceProvider.GetRequiredService<IDataHeandlerService>())
			);

			_serviceProvider = serviceCollection.BuildServiceProvider();
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
	}
}
