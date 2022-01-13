using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Services.Store;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CurrencyConverter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageMain : Page
    {
        private bool isLoaded = false;

        private string input = "";
        private string addOn = "product_sku";

        private StoreContext context;
        
        private DataTransferManager dataTransferManager;

        private string share;


        public PageMain()
        {
            this.InitializeComponent();

            App.value = 0;

            this.Loaded += onLoaded;
            this.Unloaded += onUnloaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.dataTransferManager = DataTransferManager.GetForCurrentView();
            this.dataTransferManager.DataRequested += onDataTransferRequested;

            //isLoaded = false;

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.dataTransferManager.DataRequested -= onDataTransferRequested;
            if (e.NavigationMode == NavigationMode.Back)
            {
                //this.NavigationCacheMode = NavigationCacheMode.Disabled;
                //isLoaded = false;
            }
        }

        private void onDataTransferRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            if (share == null)
            {
                args.Request.FailWithDisplayText("An error occurred!");
            }
            else
            {
                args.Request.Data.Properties.Title = App.name;
                args.Request.Data.SetText(share);
            }
        }

        #region DataModel
        private void onDataStarted(object sender, RoutedEventArgs e)
        {
            this.progressBar.Visibility = Visibility.Visible;
        }
        private void onDataLoaded(object sender, RoutedEventArgs e)
        {
            this.progressBar.Visibility = Visibility.Collapsed;
            updateData();
        }
        private void onDataErrored(object sender, RoutedEventArgs e)
        {
            this.progressBar.Visibility = Visibility.Collapsed; ;

        }
        #endregion

        private void onUnloaded(object sender, RoutedEventArgs e)
        {
            App.dataModel.Started -= onDataStarted;
            App.dataModel.Loaded -= onDataLoaded;
            App.dataModel.Errored -= onDataErrored;

            Window.Current.CoreWindow.KeyDown -= onMainKeyDown;
        }

        private async void onLoaded(object sender, RoutedEventArgs e)
        {
            App.dataModel.Started += onDataStarted;
            App.dataModel.Loaded += onDataLoaded;
            App.dataModel.Errored += onDataErrored;

            Window.Current.CoreWindow.KeyDown += onMainKeyDown;

            this.progressBar.Visibility = Visibility.Collapsed;

            updateStatics();
            updateData();

            updateControls();

            if (!App.purchased && !isLoaded)
            {
                App.purchased = await purchaseCheck();
                Prefs.setAds(App.purchased);

                updateControls();
            }

            if (!isLoaded)
            {
                long now = CurrencyConverter.getStamp(); 
                long diff = now - Prefs.getStamp();
                if (now == 0 || diff > 30*60 * 1000)     //1 hour
                {
                    App.dataModel.start();
                }
            }


            isLoaded = true;
        }

        private void onMainKeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            string item = "";

            args.Handled = true;

            switch (args.VirtualKey)
            {
                case (Windows.System.VirtualKey)190:
                case Windows.System.VirtualKey.Decimal:
                    item = (".");
                    break;

                case Windows.System.VirtualKey.Number0:
                case Windows.System.VirtualKey.NumberPad0:
                    item = ("0");
                    break;

                case Windows.System.VirtualKey.Number1:
                case Windows.System.VirtualKey.NumberPad1:
                    item = ("1");
                    break;

                case Windows.System.VirtualKey.Number2:
                case Windows.System.VirtualKey.NumberPad2:
                case Windows.System.VirtualKey.A:
                case Windows.System.VirtualKey.B:
                case Windows.System.VirtualKey.C:
                    item = ("2");
                    break;

                case Windows.System.VirtualKey.Number3:
                case Windows.System.VirtualKey.NumberPad3:
                case Windows.System.VirtualKey.D:
                case Windows.System.VirtualKey.E:
                case Windows.System.VirtualKey.F:
                    item = ("3");
                    break;

                case Windows.System.VirtualKey.Number4:
                case Windows.System.VirtualKey.NumberPad4:
                case Windows.System.VirtualKey.G:
                case Windows.System.VirtualKey.H:
                case Windows.System.VirtualKey.I:
                    item = ("4");
                    break;

                case Windows.System.VirtualKey.Number5:
                case Windows.System.VirtualKey.NumberPad5:
                case Windows.System.VirtualKey.J:
                case Windows.System.VirtualKey.K:
                case Windows.System.VirtualKey.L:
                    item = ("5");
                    break;

                case Windows.System.VirtualKey.Number6:
                case Windows.System.VirtualKey.NumberPad6:
                case Windows.System.VirtualKey.M:
                case Windows.System.VirtualKey.N:
                case Windows.System.VirtualKey.O:
                    item = ("6");
                    break;

                case Windows.System.VirtualKey.Number7:
                case Windows.System.VirtualKey.NumberPad7:
                case Windows.System.VirtualKey.P:
                case Windows.System.VirtualKey.Q:
                case Windows.System.VirtualKey.R:
                case Windows.System.VirtualKey.S:
                    item = ("7");
                    break;

                case Windows.System.VirtualKey.Number8:
                case Windows.System.VirtualKey.NumberPad8:
                case Windows.System.VirtualKey.T:
                case Windows.System.VirtualKey.U:
                case Windows.System.VirtualKey.V:
                    item = ("8");
                    break;

                case Windows.System.VirtualKey.Number9:
                case Windows.System.VirtualKey.NumberPad9:
                case Windows.System.VirtualKey.W:
                case Windows.System.VirtualKey.X:
                case Windows.System.VirtualKey.Y:
                case Windows.System.VirtualKey.Z:
                    item = ("9");
                    break;

                case Windows.System.VirtualKey.Back:
                case Windows.System.VirtualKey.Delete:
                    bkspc();
                    return;

                default:
                    return;
            }

            add(item);
        }

        private void show()
        {
            App.value = Utils.getDouble(input);
            updateData();
        }

        private void add(string ch)
        {
            if (ch == "." && input.Contains(ch)) return;

            input += ch;

            show();
        }

        private void bkspc()
        {
            int count = input.Count();
            if (count > 0)
            {
                input = input.Substring(0, count - 1);
                show();
            }
        }

        private void updateData()
        {
            //textInputFr.Text = App.slotFr.Symbol + input;

            Underline underline = new Underline();
            Run run = new Run();
            run.Text = App.slotFr.Symbol + input;
            underline.Inlines.Add(run);
            textInputFr.Inlines.Clear();
            textInputFr.Inlines.Add(underline);

            textInputTo.Text = App.slotTo.Symbol + App.slotTo.Value.ToString("N2");
        }

        private void updateStatics()
        { 
            textCodeFr.Text = App.slotFr.Code;
            textCodeTo.Text = App.slotTo.Code;

            imageFlagFr.Source = new BitmapImage(new Uri(App.slotFr.Flag));
            imageFlagTo.Source = new BitmapImage(new Uri(App.slotTo.Flag));
        }

        private void onDigitClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            add(button.Content as String);
        }

        private void onBackClick(object sender, RoutedEventArgs e)
        {
            bkspc();
        }

        private void onButtonSelect(object sender, RoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            if (senderElement == null) return;

            // If the menu was attached properly, we just need to call this handy method
            //FlyoutBase.ShowAttachedFlyout(senderElement);

            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }

        private void onFavoriteClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PageFavorites));
        }

        private void onClickFr(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PageSelect), "fr");
        }

        private void onClickTo(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PageSelect), "to");
        }

        private void onSwapTapped(object sender, TappedRoutedEventArgs e)
        {
            Currency temp = App.slotFr;
            App.slotFr = App.slotTo;
            App.slotTo = temp;

            updateData();
            updateStatics();

            CurrencyConverter.saveSlots();
        }

        private void onFlyoutShare(object sender, RoutedEventArgs e)
        {
            share = string.Format("Download {0} on Windows Store at {1}", App.name, "https://www.microsoft.com/store/apps/9n04v1d3s6rp"); 
            DataTransferManager.ShowShareUI();
        }

        private async void onFlyoutContact(object sender, RoutedEventArgs e)
        {
            string body = "";
            body += "\r\n";
            body += "\r\n";
            body += "\r\n";

            var compose = new Windows.ApplicationModel.Email.EmailMessage();
            compose.Body = body;
            compose.Subject = App.name + " for Windows 10";

            var emailRecipient = new Windows.ApplicationModel.Email.EmailRecipient("abc@gmail.com");
            compose.To.Add(emailRecipient);

            await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(compose);
        }

        private async void onFlyoutRate(object sender, RoutedEventArgs e)
        {
            var pkg = Windows.ApplicationModel.Package.Current;
            string url = string.Format("ms-windows-store://review/?PFN={0}", pkg.Id.FamilyName);

            await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
        }

        private void onFlyoutPay(object sender, RoutedEventArgs e)
        {
            purchaseAddOn();            
        }

        public async void purchaseAddOn()
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
            }

            progressBar.Visibility = Visibility.Visible;
            StorePurchaseResult result = await context.RequestPurchaseAsync(addOn);
            progressBar.Visibility = Visibility.Collapsed;

            // Capture the error message for the operation, if any.
            if (result.ExtendedError != null)
            {
                // The user may be offline or there might be some other server failure.
                Debug.WriteLine($"ExtendedError: {result.ExtendedError.Message}");

                Utils.message(App.name, result.ExtendedError.Message, null);
                return;
            }

            switch (result.Status)
            {
                case StorePurchaseStatus.AlreadyPurchased:
                    App.purchased = true;
                    Prefs.setAds(App.purchased);
                    updateControls();

                    Utils.message(App.name, "Ad removal already purchased.", null);
                    break;

                case StorePurchaseStatus.Succeeded:
                    App.purchased = true;
                    Prefs.setAds(App.purchased);
                    updateControls();

                    Utils.message(App.name, "Ad removal sucessfully purchased.", null);
                    break;

                case StorePurchaseStatus.NotPurchased:
                    Utils.message(App.name, "Unable to complete purchase.", null);
                    break;

                case StorePurchaseStatus.NetworkError:
                    Utils.message(App.name, "Unable to complete purchase due to a network error.", null);
                    break;

                case StorePurchaseStatus.ServerError:
                    Utils.message(App.name, "Unable to complete purchase due to a server error.", null);
                    break;

                default:
                    Utils.message(App.name, "Unable to complete purchase due to an unknown error.", null);
                    break;
            }
        }

        private async Task<bool> purchaseCheck()
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
            }

            // Specify the kinds of add-ons to retrieve.
            var filterList = new List<string>(new[] { "Durable" });
            var idList = new List<string>(new[] { addOn });

            StoreProductQueryResult queryResult = await context.GetStoreProductsAsync(filterList, idList);

            if (queryResult.ExtendedError != null)
            {
                // The user may be offline or there might be some other server failure.
                Debug.WriteLine($"ExtendedError: {queryResult.ExtendedError.Message}");
                return false;
            }


            foreach (var item in queryResult.Products)
            {
                StoreProduct product = item.Value;
                return product.IsInUserCollection;
            }

            return false;
        }

        public void updateControls()
        {
            flyItemPay.Visibility = App.purchased ? Visibility.Collapsed : Visibility.Visible;
            adControl.Visibility = App.purchased ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
