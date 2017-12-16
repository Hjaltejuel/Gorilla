using System;

namespace UITEST.Model
{
    public interface INavigationService
    {
        bool Navigate(Type sourcePageType, object parameter);

        void GoBack();
    }
}
