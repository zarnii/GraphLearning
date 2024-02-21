using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace GraphApp.Interfaces
{
    public interface INavigationService: INotifyPropertyChanged
    {
        ViewModel.ViewModel CurrentView { get; }

        void NavigateTo<TViewModel>() 
            where TViewModel: ViewModel.ViewModel;

        void NavigateTo(Type viewModelType);
    }
}