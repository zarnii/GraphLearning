using GraphApp.Command;
using GraphApp.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace GraphApp.ViewModel
{
    public class StatisticViewModel : ViewModel
    {
        private INavigationService _navigationService;

        private IAccessControlService _accessControlService;

        public ICommand GoBack { get; private set; }

        public ICommand Clear { get; private set; }

        public ObservableCollection<(string, int)> Statistic { get; private set; }

        public StatisticViewModel(IAccessControlService accessControlService, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _accessControlService = accessControlService;
            _accessControlService.EducationMaterialMapChanged += UpdateCollection;
            StatisticInit();

            GoBack = new RelayCommand(GoBackCommand);
            Clear = new RelayCommand(ClearCommand);

        }

        private void StatisticInit()
        {
            Statistic = new ObservableCollection<(string, int)>(
                _accessControlService.EducationMaterialMap
                    .Select(x => (x.Key.EducationMaterialTitle, x.Value.AttemptsNumber.Value))
                    .ToArray()
            );
        }

        private void GoBackCommand(object parameter)
        {
            _navigationService.NavigateTo<MainMenuViewModel>();
        }

        private void ClearCommand(object parameter)
        {
            _accessControlService.ResetMap();
            UpdateCollection(null, null);
        }

        private void UpdateCollection(object? sender, EventArgs e)
        {
            var collection = _accessControlService.EducationMaterialMap.ToArray();

            for (var i = 0; i < collection.Length; i++)
            {
                Statistic[i] = (collection[i].Key.EducationMaterialTitle, collection[i].Value.AttemptsNumber.Value);
            }
        }
    }
}
