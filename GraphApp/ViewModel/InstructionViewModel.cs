using GraphApp.Command;
using GraphApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GraphApp.ViewModel
{
    class InstructionViewModel: ViewModel
    {
        private INavigationService _navigationService;

        public ICommand GoBack { get; private set; }

        public InstructionViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            GoBack = new RelayCommand(GoBackCommand);
        }

        public void GoBackCommand(object parameter)
        {
            _navigationService.NavigateTo<MainMenuViewModel>();
        }
    }
}
