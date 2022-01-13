using Microsoft.Advertising.WinRT.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace CurrencyConverter
{
    public sealed partial class AdView : UserControl
    {
        public AdView()
        {
            this.InitializeComponent();

            this.Loaded += onLoaded;
            this.Unloaded += onUnloaded;
        }

        private void onUnloaded(object sender, RoutedEventArgs e)
        {
        }

        private void onLoaded(object sender, RoutedEventArgs e)
        {
            var str = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;
            if (str == "Windows.Mobile")
            {
                adControlMo.Visibility = Visibility.Visible;
                adControlPc.Visibility = Visibility.Collapsed;
            }
            else //if (str == "Windows.Desktop")
            {
                adControlPc.Visibility = Visibility.Visible;
                adControlMo.Visibility = Visibility.Collapsed;
            }
        }

        private void onAdRefreshed(object sender, RoutedEventArgs e)
        {
            // Add code here that you wish to execute when the ad refreshes.
        }

        private void onAdError(object sender, Microsoft.Advertising.WinRT.UI.AdErrorEventArgs e)
        {
            // Add code to gracefully handle errors that occurred while serving an ad.
            // For example, you may opt to show a default experience, or reclaim the grid 
            // display area for other purposes.
        }

        private void isAdEngagedChanged(object sender, RoutedEventArgs e)
        {
            AdControl control = sender as AdControl;
            if (true == control.IsEngaged)
            {
                // Add code here to change behavior while the user engaged with ad.
                // For example, if the app is a game, you might pause the game.
            }
            else
            {
                // Add code here to update app behavior after the user is no longer
                // engaged with the ad. For example, you might unpause a game. 
            }
        }
    }
}
