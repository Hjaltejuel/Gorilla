using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Gorilla.Model
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
    }
}
