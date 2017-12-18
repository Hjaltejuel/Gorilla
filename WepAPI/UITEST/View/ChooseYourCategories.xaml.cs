using BDSA2017.Assignment08.UWP.ViewModel;
using System;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UITEST.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChooseYourCategories : Page
    {
        private readonly ChooseYourCategoriesViewModel _vm;
        public ChooseYourCategories()
        {

            this.InitializeComponent();
            _vm = App.ServiceProvider.GetService<ChooseYourCategoriesViewModel>();



        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            Storyboard fadeIn = this.Resources["FadeIn"] as Storyboard;
            fadeIn.Begin();

        }

       

        private void Onwards(Object sender, RoutedEventArgs e)
        {
            _vm.GoToLoading();
        }

        private void Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            _vm.add(checkbox.Content.ToString());
        }

        private void Unchecked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            _vm.remove(checkbox.Content.ToString());
        }
    }
}
