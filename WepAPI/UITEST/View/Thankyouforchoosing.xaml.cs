using BDSA2017.Assignment08.UWP.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UITEST.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ThankYouForChoosing : Page
    {
        private readonly ThankYouForChoosingViewModel _vm;

        public ThankYouForChoosing()
        {
           
            this.InitializeComponent();
            LoadingRing.IsActive = true;
            _vm = App.ServiceProvider.GetService<ThankYouForChoosingViewModel>();
            _vm.ChoosingReadyEvent += LoadingRingSwitch;
        }

        private void LoadingRingSwitch()
        {
            LoadingRing.IsActive = false;
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Storyboard fadeIn = this.Resources["FadeIn"] as Storyboard;
            fadeIn.Begin();
            var strings = e.Parameter as string[];
            await _vm.load(strings);
            

        }
    }
}
