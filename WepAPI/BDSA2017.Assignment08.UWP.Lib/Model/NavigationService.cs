using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Lib.Model
{
    public class NavigationService : INavigationService
    {






        public bool Navigate(Type sourcePageType, object parameter)
        {
            if (Window.Current.Content is Frame rootFrame)
            {

                return rootFrame.Navigate(sourcePageType, parameter);
            }

            return false;
        }

        public void GoBack()
        {
            if (Window.Current.Content is Frame f) f.GoBack();
        }
    }
}
