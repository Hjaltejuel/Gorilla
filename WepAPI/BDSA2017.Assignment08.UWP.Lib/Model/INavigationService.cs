using System;

namespace UI.Lib.Model
{
    public interface INavigationService
    {
        bool Navigate(Type sourcePageType, object parameter);

        void GoBack();
    }
}
