using GraphApp.Services;
using GraphApp.Interfaces;
using GraphApp.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using GraphApp.Services.FactoryViewModel;
using GraphApp.Services.Providers;
using System.Diagnostics;
using GraphApp.ViewModel.Verify;
using GraphApp.View;
using GraphApp.Model;
using System.Configuration;
using System.Collections.Specialized;

namespace GraphAppTest
{
    [TestClass]
    public class NavigationServiceTest
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationServiceTest() 
        { 
            var dependencyCollection = new ServiceCollection();

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
            dependencyCollection.AddTransient<VerifyTestViewModel>();
            dependencyCollection.AddSingleton<PracticViewModel>();
            dependencyCollection.AddTransient<VertexViewModel>();
            dependencyCollection.AddTransient<ConnectionViewModel>();
            dependencyCollection.AddSingleton<InstructionViewModel>();
            #endregion

            dependencyCollection.AddSingleton<Func<Type, ViewModel>>((vmType) =>
            {
                return (ViewModel)_serviceProvider.GetRequiredService(vmType);
            });

            _serviceProvider = dependencyCollection.BuildServiceProvider();
        }

        [TestMethod]
        [DynamicData(nameof(GenerateData), DynamicDataSourceType.Method)]
        public void NavigationService_ChangeVm_Success(Type vmType)
        {
            var vm = _serviceProvider.GetRequiredService(vmType);
            var navigationService = _serviceProvider.GetRequiredService<INavigationService>();

            Assert.ReferenceEquals(vm, navigationService.CurrentView);
        }

        private static IEnumerable<object?[]> GenerateData()
        {
            yield return new object[1] { typeof(VertexViewModel) };
            yield return new object[1] { typeof(InstructionViewModel) };
            yield return new object[1] { typeof(ConnectionViewModel) };
            yield return new object[1] { typeof(PlaygroundViewModel) };
        }
    }
}
