using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace GraphApp.Interfaces
{
    public interface INavigationService: INotifyPropertyChanged
    {
        Page CurrentPage { get; }

        void NavigateTo<Page>() 
            where Page : System.Windows.Controls.Page;

        void NavigateTo(Type pageType);
    }
}