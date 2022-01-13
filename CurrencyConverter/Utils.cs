using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace CurrencyConverter
{
    public class Utils
    {
        public static async void message(string title, string msg, Action okAction)
        {
            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                ContentDialog dialog = new ContentDialog();
                dialog.Title = title;
                dialog.Content = msg;
                dialog.PrimaryButtonText = "OK";

                ContentDialogResult result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    if (okAction != null) okAction();
                }
            });

        }

        public async static void confirm(string title, string msg, Action yesAction, Action noAction)
        {
            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                ContentDialog dialog = new ContentDialog();
                dialog.Title = title;
                dialog.Content = msg;
                dialog.PrimaryButtonText = "Yes";
                dialog.SecondaryButtonText = "No";

                ContentDialogResult result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    if (yesAction != null) yesAction();
                }
                else
                {
                    if (noAction != null) noAction();
                }
            });
        }

        public static double getDouble(string input)
        {
            double value = 0;

            try
            {
                if (input.Trim().Length == 0) return value;

                value = Convert.ToDouble(input);
            }
            catch (Exception)
            {
                value = 0;
            }
            return value;
        }

    }
}
