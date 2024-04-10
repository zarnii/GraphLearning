using System.Text;
using System.Windows.Input;
using GraphApp.Command;
using GraphApp.Interfaces;

namespace GraphApp.ViewModel
{
    public class SettingsViewModel: ViewModel
    {
        private INavigationService _navigationService;

        public ICommand GoBack { get; private set; }

        public string Statistic { get; private set; }

        public SettingsViewModel(IAccessControlService accessControlService, INavigationService navigationService) 
        {
            _navigationService = navigationService;
            StatisticInit(accessControlService);

            GoBack = new RelayCommand(GoBackCommand);
        }

        private void StatisticInit(IAccessControlService accessControlService)
        {
            var stringBuilder = new StringBuilder();

            foreach (var map in accessControlService.EducationMaterialMap)
            {
                stringBuilder.AppendLine(
                    $"Название задание: {map.Key.EducationMaterialTitle}, " +
                    $"количество попыток: {map.Value.AttemptsNumber}"
                );
            }

            Statistic = stringBuilder.ToString();
        }

        private void GoBackCommand(object parameter)
        {
            _navigationService.NavigateTo<MainMenuViewModel>();
        }
    }
}
