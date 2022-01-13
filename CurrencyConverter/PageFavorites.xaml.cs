using System.Collections.ObjectModel;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace CurrencyConverter
{
    public partial class PageFavorites : Page 
    {
        private bool isLoaded = false;
        private CurrencyModel favModel;

        public PageFavorites()
        {
            this.DataContext = null;
            this.InitializeComponent();

            this.Loaded += onLoaded;
            this.Unloaded += onUnloaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void onUnloaded(object sender, RoutedEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= onBackRequested;
        }

        private void onLoaded(object sender, RoutedEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += onBackRequested;

            adControl.Visibility = App.purchased ? Visibility.Collapsed : Visibility.Visible;

            if (isLoaded) return;

            if (favModel == null)
            {
                favModel = new CurrencyModel(false);
                this.DataContext = favModel;
            }

            this.listFavs.SelectedIndex = -1;
            isLoaded = true;
        }

        private void onItemSelected(object sender, SelectionChangedEventArgs e)
        {
            if (!isLoaded) return;

            if (listFavs.SelectedIndex == -1) return;
            Currency item = (Currency)listFavs.SelectedItem;
            if (item != null)
            {
            }
            listFavs.SelectedIndex = -1;
        }


        private void onItemTapped(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            if (senderElement == null) return;

            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
            e.Handled = true;
        }

        private void onRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            if (senderElement == null) return;

            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }

        private void onHolding(object sender, HoldingRoutedEventArgs e)
        {
            if (e.HoldingState != HoldingState.Started) return;

            FrameworkElement senderElement = sender as FrameworkElement;
            if (senderElement == null) return;

            FlyoutBase.ShowAttachedFlyout(senderElement);
        }

        private void onFlyoutDelete(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem item = sender as MenuFlyoutItem;
            if (item != null)
            {
                Currency entry = item.DataContext as Currency;
                if (entry != null)
                {
                    Utils.confirm("Remove Favorite", "Are you sure you want to remove this currency?", () => { deleteFav(entry.code); }, () => { });
                }
            }
        }

        private void onFlyoutFrom(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem item = sender as MenuFlyoutItem;
            if (item != null)
            {
                Currency entry = item.DataContext as Currency;
                if (entry != null)
                {
                    if (App.slotFr.code == entry.code || App.slotTo.code == entry.code)
                    {
                        Utils.message("Currency", "Currency already set!", null);
                        return;
                    }

                    App.slotFr = entry;
                    CurrencyConverter.saveSlots();
                    back();
                }
            }
        }

        private void onFlyoutTo(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem item = sender as MenuFlyoutItem;
            if (item != null)
            {
                Currency entry = item.DataContext as Currency;
                if (entry != null)
                {
                    if (App.slotFr.code == entry.code || App.slotTo.code == entry.code)
                    {
                        Utils.message("Currency", "Currency already set!", null);
                        return;
                    }

                    App.slotTo = entry;
                    CurrencyConverter.saveSlots();
                    back();
                }
            }
        }

        private void onBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (back())
            {
                e.Handled = true;
            }
        }

        private void onBackClick(object sender, RoutedEventArgs e)
        {
            back();
        }

        private bool back()
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                return true;
            }

            return false;
        }

        private void deleteFav(string code)
        {
            string fav = "";
            Currency del = null;

            var list = new ObservableCollection<Currency>();            
            foreach(Currency currency in favModel.Items)
            {
                if(currency.code == code)
                {
                    del = currency;
                }
                else
                {
                    list.Add(currency);
                    fav = fav + (fav.Length == 0 ? "" : ",") + currency.code;
                }
            }

            if (del != null)
            {
                favModel.Items.Remove(del);
            }

            Prefs.setFavs(fav);

            App.favorites = new ObservableCollection<Currency>(list);

            favModel.deleted();
        }
    }
}
